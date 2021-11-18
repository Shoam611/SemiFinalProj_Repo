using System;
using Castle.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Sevices;
using tWpfMashUp_v0._0._1.Extensions;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public delegate void TurnChangedEventHandler(bool value);

    public class GameBoard : IGameBoard
    {
        public event TurnChangedEventHandler TurnChanged;

        private TaskCompletionSource<SoliderModel> pickStackForSolider;
        private readonly SignalRListenerService signalRListener;
        private readonly List<MoveOption> options;
        private readonly GameService gameService;
        private readonly StoreService store;
        private List<int> rollsValues;

        public Grid GameGrid { get; private set; }
        public StackModel FocusedStack { get; set; }
        public StackModel[,] StacksMatrix { get; set; }
        public SoliderModel FocusedSolider { get; set; }
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
        }

        public void UpdateRollsResult(List<int> newVals)
        {
            rollsValues = newVals;
            options.Clear();
            if (!HasAvailableMoves())
            {
                Modal.ShowModal("You have no available moves, :(");
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
                                    .BuildMatirxMovementAbility();

        private GameBoard SetInitialVaulues(Grid grid)
        {
            IsMyTurn = store.Get(CommonKeys.IsMyTurn.ToString());
            GameGrid = grid;
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

            if ((StackModel)sender == FocusedStack)
            {
                if (pickStackForSolider != null)
                {
                    pickStackForSolider.TrySetResult(FocusedSolider);
                    pickStackForSolider = null;
                }
                FocusedStack.MarkSoliderAsActive(false);
                FocusedStack = null;
                foreach (var opt in options) StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                options.Clear();
            }
            else if (((StackModel)sender).IsOption)
            {
                if (pickStackForSolider != null)
                {
                    if (((StackModel)sender).Count == 0 || ((StackModel)sender).HasMineSoliders())
                    {
                        var newStack = (StackModel)sender;
                        FocusedStack.Pop();
                        newStack.Push(FocusedSolider);
                        //start to unstable at prev version
                        newStack.MarkSoliderAsActive(false);
                        foreach (var opt in options) StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                        var toremove = options.FirstOrDefault(o => o.Location.Equals(newStack.Location));
                        if (toremove != null) rollsValues.Remove(toremove.DicerollValue);
                        options.Clear();
                        //end to unstable at prev version
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
            StacksMatrix[e.Destenation.Col, e.Destenation.Row].Push(solider);
        }

        public void MarkAvailableMoves(List<int> rollRes, MatrixLocation selectedLocation)
        {
            options.Clear();
            foreach (var res in rollRes)
            {
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
            }
            if (options.Count == 0)
                Modal.ShowModal("No Available Moves For This One :( \n Click Again To Deselect...");
        }
    }
}