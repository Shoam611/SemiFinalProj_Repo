using System;
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

        public SoldierModel FocusedSolider { get; set; }

        public string CurrentPlayer { get; set; }
        public bool IsBlocked { get; set; }

        public GameBoard(Grid gameGrid) => GameGrid = gameGrid;

        public GameBoard Build() => this.BuildGameBoardDefenitions(12, 2)
                                    .FillGameboardMatrix()
                                    .Clear()
                                    .PlaceSolidersInInitialState();

        public bool Move(SoldierModel soldier, StackModel stack)
        {
            throw new NotImplementedException();
        }

        public void ShowAvailableMoves()
        {
            throw new NotImplementedException();
        }
    }
}