﻿using System.Windows.Controls;
using System.Collections.Generic;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces
{
    public interface IGameBoard
    {
        SoliderModel FocusedSolider { get; set; }
        StackModel FocusedStack { get; set; }
        Grid GameGrid { get; }
        bool IsMyTurn { get; set; }
        int MatrixColumnsCount { get; }
        int MatrixRowsCount { get; }
        StackModel[,] StacksMatrix { get; set; }
        public event TurnChangedEventHandler TurnChanged;
        void UpdateRollsResult(List<int> newVals);
        void AddStackToGameGridAndMatrix(StackModel stck, int row, int col);
        GameBoard Build(Grid gameGrid);
        void MarkAvailableMoves(List<int> rollRes, MatrixLocation selectedLocation);
    }
}
