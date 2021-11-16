using System;
using Castle.Core;
using System.Threading.Tasks;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Extensions;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class GameBoard : IGameBoard
    {
        private StoreService store;
        private GameService gameService;
        private SignalRListenerService signalRListener;
        private TaskCompletionSource<SoliderModel> pickStackForSolider;

        public Grid GameGrid { get; private set; }

        public StackModel[,] StacksMatrix { get; set; }
        public int MatrixColumnsCount { get => StacksMatrix.GetLength(0); }
        public int MatrixRowsCount { get => StacksMatrix.GetLength(1); }
        public bool IsMyTurn { get; set; }

        public SoliderModel FocusedSolider { get; set; }
        public StackModel FocusedStack { get; set; }

        public GameBoard(SignalRListenerService signalRListener,GameService gameService,StoreService store)
        {
            this.store = store;
            this.gameService = gameService;
            this.signalRListener = signalRListener;
            signalRListener.OpponentPlayed += UpdateOpponentMove;
        }

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
                stack.OnSelected += Stack_OnSelected;
            }
            return this;
        }

        private async void Stack_OnClicked(object sender, EventArgs e)
        {
            if (IsMyTurn && ((StackModel)sender).Count > 0 && (((StackModel)sender).HasMineSoliders()))  //inform User??
            {
                FocusedStack = (StackModel)sender;
                FocusedSolider = FocusedStack.Peek();
                StackModel.HasFirstSelected = true;
                FocusedStack.MarkSoliderAsActive(true);
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
            if (pickStackForSolider != null && (((StackModel)sender).Count == 0 || ((StackModel)sender).HasMineSoliders()))
            {
                var newStack = (StackModel)sender;
                //if newStack is marked as available
                FocusedStack.Pop();
                newStack.Push(FocusedSolider);
                newStack.MarkSoliderAsActive(false);
                Pair<MatrixLocation, MatrixLocation> actionUpdate = new Pair<MatrixLocation, MatrixLocation>(FocusedStack.Location, newStack.Location);
                //CallServer to Push Update async/add action to store?
                pickStackForSolider.TrySetResult(FocusedSolider);
                pickStackForSolider = null;
                gameService.UpdateServerMove(actionUpdate);
            }
        }

        private void UpdateOpponentMove(object sender, OpponentPlayedEventArgs e)
        {
            var solider = StacksMatrix[e.Source.Col, e.Source.Row].Pop();
            StacksMatrix[e.Destenation.Col, e.Destenation.Row].Push(solider);
        }

        public void MarkAvailableMoves(Pair<int, int> rollRes, SoliderModel selectedSolider)
        {
            throw new NotImplementedException();
        }

    }
}