using System;
using Castle.Core;
using System.Threading.Tasks;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Sevices;
using tWpfMashUp_v0._0._1.Extensions;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;
using System.Collections.Generic;
using System.Linq;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public delegate void TurnChangedEventHandler(bool value);

    public class GameBoard : IGameBoard
    {
        int inHouseCount;
        int totalSolidersCount;
        int TotalSolidersCount 
        { 
            get => totalSolidersCount;
            set { totalSolidersCount = value;if (totalSolidersCount == 0) AnnounceAsWinner(); } 
        }


        private bool allowTakeOuts=>inHouseCount == TotalSolidersCount;
        private List<MoveOption> options;
        private List<int> rollsValues;
        private StoreService store;
        private GameService gameService;
        private SignalRListenerService signalRListener;
        private TaskCompletionSource<SoliderModel> pickStackForSolider;
        Grid takeOutGrid;
        public event TurnChangedEventHandler TurnChanged;

        public Grid GameGrid { get; private set; }
        public SoliderModel FocusedSolider { get; set; }
        public StackModel FocusedStack { get; set; }
        public StackModel[,] StacksMatrix { get; set; }
        public int MatrixColumnsCount { get => StacksMatrix.GetLength(0); }
        public int MatrixRowsCount { get => StacksMatrix.GetLength(1); }

        private bool isMyTurn;

        public bool IsMyTurn
        {
            get => isMyTurn;
            set
            {
                isMyTurn = value;
                TurnChanged?.Invoke(value);
            }
        }


        public GameBoard(SignalRListenerService signalRListener, GameService gameService, StoreService store)
        {
            this.store = store;
            this.gameService = gameService;
            this.signalRListener = signalRListener;
            this.signalRListener.OpponentPlayed += UpdateOpponentMove;
            this.signalRListener.OpponentFinnishedPlay += (s, e) => IsMyTurn = true; ;

            options = new List<MoveOption>();
            rollsValues = new List<int>();
            inHouseCount = 0;
        }


        public void UpdateRollsResult(List<int> newVals)
        {
            rollsValues = newVals;
            options.Clear();
            if (!HasAvailableMoves())
            {
                Modal.ShowModal($"It seems like you have no available moves \n for {rollsValues[0]} {rollsValues[1]}","Bad luck");
                rollsValues.Clear();
                gameService.UpdateTurnChangedAsync();
                isMyTurn = false;
            }
            options.Clear();
        }

        private bool HasAvailableMoves()
        {
            for (int row = 0; row < MatrixRowsCount; row++)
            {
                for (int col = 0; col < MatrixColumnsCount; col++)
                {
                    if (StacksMatrix[col, row].HasMineSoliders())
                    {
                        MarkAvailableMoves(rollsValues, new MatrixLocation { Col = col, Row = row });
                        if (options.Any())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        public GameBoard Build(Grid gameGrid) =>
                                    SetInitialVaulues(gameGrid)
                                    .Clear()
                                    .BuildGameBoardDefenitions(12, 2)
                                    .FillGameboardMatrix()
                                    .PlaceSolidersInInitialState()
                                    .BuildMatirxMovementAbility()
                                    .BuildBoardoutGrid();

        private GameBoard BuildBoardoutGrid()
        {
            takeOutGrid = new Grid { Background = new SolidColorBrush(Color.FromArgb(125, 250, 250, 250)) };
            Grid.SetRowSpan(takeOutGrid, 3);
            Grid.SetColumnSpan(takeOutGrid, 6);
            Panel.SetZIndex(takeOutGrid, 3);
            takeOutGrid.Visibility = Visibility.Collapsed;
            takeOutGrid.Height = Double.NaN; takeOutGrid.Width = Double.NaN;
            GameGrid.Children.Add(takeOutGrid);
            return this;
        }
        private void RemoveSolider(MoveOption option)
        {
            if (pickStackForSolider != null)
            {

                //take out of the view
                FocusedStack.Pop();
                //make grid disapear
                foreach (var opt in options)
                {
                    if (opt.Location.Col == 12)
                    {
                        takeOutGrid.Visibility = Visibility.Collapsed;
                        continue;
                    }
                    StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                }
                //spend the dice roll move
                rollsValues.Remove(option.DicerollValue);
                //since move was made clear options, and deselect stack and solider;
                options.Clear();
                //remove from counting
                inHouseCount--;
                TotalSolidersCount--;
                //make turn continue // exception here
                pickStackForSolider.TrySetResult(FocusedSolider);
                pickStackForSolider = null;
                //create a move update
                var move = new Pair<MatrixLocation, MatrixLocation>(FocusedStack.Location, new MatrixLocation { Col = 12, Row = 1 });
                if (totalSolidersCount == 0)
                {
                    gameService.AnnounceAsWinnerAsync();
                }
                gameService.UpdateServerMove(move);
                if (rollsValues.Count == 0 || !HasAvailableMoves())
                {
                    IsMyTurn = false;
                    //implented bug for testing
                    gameService.UpdateTurnChangedAsync();
                }
            }
        }

        private GameBoard SetInitialVaulues(Grid grid)
        {
            IsMyTurn = store.Get(CommonKeys.IsMyTurn.ToString());
            GameGrid = grid;
            inHouseCount = 5;
            TotalSolidersCount = 15;
            return this;
        }

        public void AddStackToGameGridAndMatrix(StackModel stck, int row, int col)
        {
            StacksMatrix[stck.Location.Col, stck.Location.Row] = stck;
            GameGrid.AddToGrid(stck.Triangle, col, row);
            GameGrid.AddToGrid(stck.UiStack, col, row);
            Panel.SetZIndex(stck.Triangle, 1);
            Panel.SetZIndex(stck.UiStack, 2);
        }

        private GameBoard BuildMatirxMovementAbility()
        {
            foreach (var stack in StacksMatrix)
            {
                stack.OnClicked += Stack_OnClicked;
                stack.OnSelected += (s, e) => Stack_OnSelected(s, e);
            }
            return this;
        }

        private async void Stack_OnClicked(object sender, EventArgs e)
        {
            if (IsMyTurn &&
                ((StackModel)sender).Count > 0 &&
                ((StackModel)sender).HasMineSoliders() &&
                rollsValues.Count > 0)
            {
                FocusedStack = (StackModel)sender;
                FocusedSolider = FocusedStack.Peek();
                StackModel.HasFirstSelected = true;
                FocusedStack.MarkSoliderAsActive(true);

                MarkAvailableMoves(rollsValues, FocusedSolider.Location);
                foreach (var opt in options)
                {
                    if (opt.Location.Col == 12)
                    {
                        takeOutGrid.Visibility = Visibility.Visible;
                        MouseButtonEventHandler handler = (s, e) => { };
                        handler = (s, e) => { RemoveSolider(opt); try { takeOutGrid.MouseDown -= handler; } finally { } };
                        takeOutGrid.MouseDown += handler;
                    }
                    else
                        StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(true);
                }
                try
                {
                    await GetSelectionAsync();
                }
                finally { }
            }
            else { }
        }

        private async Task<SoliderModel> GetSelectionAsync()
        {
            pickStackForSolider = new TaskCompletionSource<SoliderModel>();
            try
            {
                return await pickStackForSolider.Task;
            }
            finally
            {
                StackModel.HasFirstSelected = false;
                FocusedSolider = null;
                FocusedStack = null;
            }
        }

        private void Stack_OnSelected(object sender, EventArgs e)
        {
            //De-selecting
            if ((StackModel)sender == FocusedStack)
            {
                if (pickStackForSolider != null)
                {
                    pickStackForSolider.TrySetResult(FocusedSolider);
                    pickStackForSolider = null;
                }
                try { FocusedStack.MarkSoliderAsActive(false); } finally { FocusedStack = null; }
                
                foreach (var opt in options)
                {
                    if (opt.Location.Col == 12)
                    {
                        takeOutGrid.Visibility = Visibility.Collapsed;
                        continue;
                    }
                    StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                }
                options.Clear();
            }
            //Selecting
            else if (((StackModel)sender).IsOption)
            {
                if (pickStackForSolider != null)
                {
                    if (((StackModel)sender).Count == 0 || ((StackModel)sender).HasMineSoliders())
                    {
                        FocusedStack.MarkSoliderAsActive(false);
                        var newStack = (StackModel)sender;
                        FocusedStack.Pop();
                        newStack.Push(FocusedSolider);
                        foreach (var opt in options)
                        {
                            if (opt.Location.Col == 12)
                            {
                                takeOutGrid.Visibility = Visibility.Collapsed;
                                continue;
                            }
                            StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                        }

                        var toRemove = options.FirstOrDefault(o => o.Location.Equals(newStack.Location));
                        if (toRemove != null) rollsValues.Remove(toRemove.DicerollValue);
                        options.Clear();

                        if (newStack.Location.Row == 1 && newStack.Location.Col >= 6 && FocusedStack.Location.Col<6 )
                            inHouseCount++;
                  
                        pickStackForSolider.TrySetResult(FocusedSolider);
                        pickStackForSolider = null;

                        var move = new Pair<MatrixLocation, MatrixLocation>(FocusedStack.Location, newStack.Location);
                        gameService.UpdateServerMove(move);

                        if (rollsValues.Count == 0 || !HasAvailableMoves())
                        {
                            IsMyTurn = false;
                            gameService.UpdateTurnChangedAsync();
                        }
                    }
                }
            }
        }

        private void UpdateOpponentMove(object sender, OpponentPlayedEventArgs e)
        {
            var solider = StacksMatrix[e.Source.Col, e.Source.Row].Pop();
            if (e.Destenation.Col < 12)
                StacksMatrix[e.Destenation.Col, e.Destenation.Row].Push(solider);
        }

        public void MarkAvailableMoves(List<int> rollRes, MatrixLocation selectedLocation)
        {
            options.Clear();

            foreach (var res in rollRes)
            {
                if (allowTakeOuts)
                {
                    if (selectedLocation.Row == 0) continue;
                    if (selectedLocation.Col + res == MatrixColumnsCount)
                    {
                        options.Add(new MoveOption { Location = new MatrixLocation { Col = 12, Row = 1 }, DicerollValue = res });
                        continue;
                    }
                    if (selectedLocation.Col + res > MatrixColumnsCount)
                    {
                        bool hasOneBefore = false;
                        for (int i = 6; i < selectedLocation.Col; i++)
                        {
                            if (StacksMatrix[i, 1].HasMineSoliders())
                            { hasOneBefore = true; break; }
                        }
                        if (hasOneBefore) continue;
                        options.Add(new MoveOption { Location = new MatrixLocation { Col = 12, Row = 1 }, DicerollValue = res });
                        continue;
                    }
                }
                if (selectedLocation.Row == 1)
                {
                    if (selectedLocation.Col + res < MatrixColumnsCount)
                    {
                        var op = res + selectedLocation.Col;
                        if (StacksMatrix[op, 1].Count == 0 || StacksMatrix[op, 1].HasMineSoliders())
                        {
                            options.Add(new MoveOption { Location = new MatrixLocation { Col = op, Row = 1 }, DicerollValue = res });
                            continue;
                        }
                    }
                }
                if (selectedLocation.Row == 0)
                {
                    if (selectedLocation.Col >= res)
                    {
                        var op = selectedLocation.Col - res;
                        if (StacksMatrix[op, 0].Count == 0 || StacksMatrix[op, 0].HasMineSoliders())
                        {
                            options.Add(new MoveOption { Location = new MatrixLocation { Col = op, Row = 0 }, DicerollValue = res });
                            continue;
                        }
                    }
                    if (selectedLocation.Col < res)
                    {
                        var op = res - (selectedLocation.Col + 1);
                        if (StacksMatrix[op, 1].Count == 0 || StacksMatrix[op, 1].HasMineSoliders())
                        {
                            options.Add(new MoveOption { Location = new MatrixLocation { Col = op, Row = 1 }, DicerollValue = res });
                            continue;
                        }
                    }
                }
            }

            if (options.Count == 0)
            {
                StackModel.HasFirstSelected = false;
             //   Modal.ShowModal("No Available moves for this one :( \n Cklickagain to deselect...");
                if (FocusedStack != null && FocusedStack.HasMineSoliders())
                FocusedStack.MarkSoliderAsActive(false);
                if(pickStackForSolider != null)
                {
                pickStackForSolider.TrySetResult(FocusedSolider);
                pickStackForSolider = null;
                }
                FocusedStack = null; FocusedSolider = null;
            }
        }
        
        private void AnnounceAsWinner()
        {
            Modal.ShowModal("Winnerwinner chicken dinner", "Congrats");
            gameService.AnnounceAsWinnerAsync();
        }
    }
}