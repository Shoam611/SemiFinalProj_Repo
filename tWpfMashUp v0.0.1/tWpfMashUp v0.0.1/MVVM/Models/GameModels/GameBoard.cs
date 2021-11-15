using Castle.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Extensions;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class GameBoard : IBoard
    {
        public Grid GameGrid { get; private set; }

        public StackModel[,] StacksMatrix { get; set; }
        public int MatrixColumnsCount { get => StacksMatrix.GetLength(0); }
        public int MatrixRowsCount { get => StacksMatrix.GetLength(1); }

        public SoliderModel FocusedSolider { get; set; }
        public StackModel FocusedStack { get; set; }
        public bool IsMyTurn { get; set; }
        private TaskCompletionSource<SoliderModel> pickStackForSolider;

        public GameBoard(Grid gameGrid) => GameGrid = gameGrid;

        public GameBoard Build() => this
                                    .Clear()
                                    .BuildGameBoardDefenitions(12, 2)
                                    .FillGameboardMatrix()
                                    .PlaceSolidersInInitialState()
                                    .BuildMatirxMovementAbility();

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
            FocusedStack = (StackModel)sender;
            FocusedSolider = ((StackModel)sender).Pop();
            try
            {
                await GetSelectionAsync();
            }
            finally { }
        }
        private void Stack_OnSelected(object sender, EventArgs e)
        {
            if (pickStackForSolider != null)
            {
                var newStack = (StackModel)sender;
                newStack.Push(FocusedSolider);
                pickStackForSolider.TrySetResult(FocusedSolider);
                pickStackForSolider = null;
            }
        }
        private async Task<SoliderModel> GetSelectionAsync()
        {
            pickStackForSolider = new TaskCompletionSource<SoliderModel>();
            try
            {
                return await pickStackForSolider.Task;
            }
            finally { StackModel.HasFirstSelected = false; FocusedSolider = null;FocusedStack = null; }
        }

       
        public void AddStackToGameGridAndMatrix(StackModel stck, int row, int col)
        {
            StacksMatrix[stck.Location.Col, stck.Location.Row] = stck;
            GameGrid.AddToGrid(stck.Triangle, col, row);
            GameGrid.AddToGrid(stck.UiStack, col, row);
            Panel.SetZIndex(stck.Triangle, 1);
            Panel.SetZIndex(stck.UiStack, 2);
        }

        public bool Move(SoliderModel soldier, StackModel stack)
        {
            throw new NotImplementedException();
        }

        public void ShowAvailableMoves(Pair<int, int> rollRes, SoliderModel selectedSolider)
        {
            throw new NotImplementedException();
        }

    }
}