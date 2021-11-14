using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels;

namespace tWpfMashUp_v0._0._1.Extensions
{
    public static class BoardExtentions
    {
        public static GameBoard Clear(this GameBoard gb)
        {
            gb.GameGrid.Clear();
            gb.StacksMatrix.Clear();
            return gb;
        }

        public static StackModel[,] Clear(this StackModel[,] StacksMatrix)
        {
            if (StacksMatrix != null)
            {
                foreach (var stack in StacksMatrix)
                {
                    stack.Clear();
                }
            }
            return StacksMatrix;
        }

        /// <summary>
        /// instantiate matrix, set grid's column and row defenitions
        /// </summary>
        /// <param name="gb"></param>
        /// <param name="cols"></param>
        /// <param name="rows"></param>
        /// <returns>gameboard object with the spesified number of rows and columns</returns>
        public static GameBoard BuildGameBoardDefenitions(this GameBoard gb, int cols, int rows)
        {
            gb.StacksMatrix = new StackModel[cols, rows];
            gb.GameGrid.BuildGridDefenitions(cols, rows);
            return gb;
        }

        /// <summary>
        /// builds the polygon and stacks. appends to matrix and grid
        /// </summary>
        /// <param name="gb"></param>
        /// <returns> gameboard object with built polygon ,stacks and matrix </returns>
        public static GameBoard FillGameboardMatrix(this GameBoard gb)
        {
            for (int i = 0; i < gb.MatrixColumnsCount + 1; i++) //cols
            {
                for (int j = 0; j < gb.MatrixRowsCount + 1; j += 2) //rows
                {
                    if (i == gb.MatrixColumnsCount / 2) continue;
                    var loc = new MatrixLocation
                    {
                        Col = i < gb.StacksMatrix.GetLength(0) / 2 ? i : i - 1,
                        Row = j < gb.StacksMatrix.GetLength(1) / 2 ? j : j - 1
                    };
                    var stck = new StackModel(loc)
                        .BuildPoligon(i, j)
                        .BuildStackPanel()
                        .Build();
                    gb.AddStackToGameBoardMatrix(stck,i,j);
                }
            }
            return gb;
        }

        public static StackModel BuildPoligon(this StackModel stack, int i, int j)
        {
            stack.Triangle = stack.Triangle.BuildPoligon(i, j);
            return stack;
        }

        public static StackModel BuildStackPanel(this StackModel stack)
        {
            stack.UiStack = new StackPanel().BuildStackPanel(stack.Location.Row);
            return stack;
        }

        public static void AddStackToGameBoardMatrix(this GameBoard gb, StackModel stck, int col, int row)
        {
            gb.StacksMatrix[stck.Location.Col,stck.Location.Row] = stck;
            gb.GameGrid.AddToGrid(stck.Triangle, col, row);
            gb.GameGrid.AddToGrid(stck.UiStack, col, row);
            Panel.SetZIndex(stck.Triangle, 1);
            Panel.SetZIndex(stck.UiStack, 2);
        }
        /// <summary>
        /// expect an empty grid, create a layout for an initial game
        /// </summary>
        /// <param name="gb"></param>
        /// <returns>gameboard object with a default full matrix with soliders instances</returns>
        public static GameBoard PlaceSolidersInInitialState(this GameBoard gb)
        {
            for (int i = 0; i < gb.MatrixColumnsCount; i++)
            {
                //for the pattern - in opposing side to any solider pice there is an identical
                //amount of pieces in the different color
                //so same action should be made to the other row but with different color piece solider
            }
            return gb;
        }

        public static void AddSoliderToGameBoard(this GameBoard gb, SoldierModel solider, int row, int col) =>
            gb.StacksMatrix[col, row].Add(solider);
    }
}
