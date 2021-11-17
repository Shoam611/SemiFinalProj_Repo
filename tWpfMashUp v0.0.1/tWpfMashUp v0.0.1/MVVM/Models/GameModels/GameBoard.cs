using System;
using Castle.Core;
using System.Threading.Tasks;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Sevices;
using tWpfMashUp_v0._0._1.Extensions;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public delegate void TurnChangedEventHandler(bool value);

    public class GameBoard : IGameBoard
    {
        private List<MoveOption> options;
        private List<int> rollsValues;
        private StoreService store;
        private GameService gameService;
        private SignalRListenerService signalRListener;
        private TaskCompletionSource<SoliderModel> pickStackForSolider;
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

            rollsValues = new List<int>();
        }

        public void UpdateRollsResult(List<int> newVals) => rollsValues = newVals;

        public GameBoard Build(Grid gameGrid) =>
                                    SetInitialVaulues(gameGrid)
                                    .Clear()
                                    .BuildGameBoardDefenitions(12, 2)
                                    .FillGameboardMatrix()
                                    .PlaceSolidersInInitialState()
                                    .BuildMatirxMovementAbility();

        GameBoard SetInitialVaulues(Grid grid)
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
                stack.OnSelected += async (s,e) =>await Stack_OnSelected(s,e);
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
                options = new List<MoveOption>();
                MarkAvailableMoves(rollsValues, FocusedSolider.Location);
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

        private async Task Stack_OnSelected(object sender, EventArgs e)
        {

            if ((StackModel)sender == FocusedStack)
            {
                pickStackForSolider = null;
                FocusedStack.MarkSoliderAsActive(false);
                FocusedStack = null;
                foreach (var opt in options) StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                options.Clear();
            }
            else if (pickStackForSolider != null &&
                (((StackModel)sender).Count == 0 ||
                    ((StackModel)sender).HasMineSoliders())
               && ((StackModel)sender).IsOption
               )
            {
                var newStack = (StackModel)sender;
                FocusedStack.Pop();
                newStack.Push(FocusedSolider);
                //start to unstable at prev version
                newStack.MarkSoliderAsActive(false);
                foreach (var opt in options) StacksMatrix[opt.Location.Col, opt.Location.Row].MarkStackAsOption(false);
                rollsValues.Remove(options.First(o => o.Location.Equals(newStack.Location)).DicerollValue);
                options.Clear();
                //end to unstable at prev version

                pickStackForSolider.TrySetResult(FocusedSolider);
                pickStackForSolider = null;

                var move = new Pair<MatrixLocation, MatrixLocation>(FocusedStack.Location, newStack.Location);
                await  gameService.UpdateServerMove(move);
                if (rollsValues.Count == 0)
                {
                    IsMyTurn = false;
                    gameService.UpdateTurnChanged();
                }
            }
        }

        private void UpdateOpponentMove(object sender, OpponentPlayedEventArgs e)
        {
            var solider = StacksMatrix[e.Source.Col, e.Source.Row].Pop();
            StacksMatrix[e.Destenation.Col, e.Destenation.Row].Push(solider);
            //IsMyTurn = true;
        }

        public void MarkAvailableMoves(List<int> rollRes, MatrixLocation selectedLocation)
        {
            foreach (var res in rollRes)
            {
                if (selectedLocation.Row == 0)
                {
                    if (selectedLocation.Col >= res)
                    {
                        var op = selectedLocation.Col - res;
                        if (StacksMatrix[op, 0].Count == 0 || StacksMatrix[op, 0].HasMineSoliders())
                        {
                            StacksMatrix[op, 0].MarkStackAsOption(true);
                            options.Add(new MoveOption { Location = new MatrixLocation { Col = op, Row = 0 }, DicerollValue = res });
                            continue;
                        }
                    }
                    if (selectedLocation.Col < res)
                    {
                        var op = res - (selectedLocation.Col + 1);
                        if (StacksMatrix[op, 0].Count == 0 || StacksMatrix[op, 0].HasMineSoliders())
                        {
                            StacksMatrix[op, 1].MarkStackAsOption(true);
                            options.Add(new MoveOption { Location = new MatrixLocation { Col = op, Row = 1 }, DicerollValue = res });
                            continue;
                        }
                    }
                }
                if (selectedLocation.Row == 1)
                {
                    if (selectedLocation.Col + res <= MatrixColumnsCount)
                    {
                        var op = res + selectedLocation.Col;
                        if (StacksMatrix[op, 0].Count == 0 || StacksMatrix[op, 0].HasMineSoliders())
                        {
                            StacksMatrix[op, 1].MarkStackAsOption(true);
                            options.Add(new MoveOption { Location = new MatrixLocation { Col = op, Row = 1 }, DicerollValue = res });
                            continue;
                        }
                    }
                }
            }

        }
    }
}