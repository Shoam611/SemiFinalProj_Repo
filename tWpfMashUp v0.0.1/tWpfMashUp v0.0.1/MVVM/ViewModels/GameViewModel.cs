using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using tWpfMashUp_v0._0._1.Core;
using System.Windows.Shapes;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class GameViewModel
    {
        public RelayCommand GoToChatCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }
        public Grid GameGrid { get; set; }
        private IGameBoard gameBoard; //main object in the buisnes logic
        public GameViewModel(IGameBoard GameBoard)
        {
            GameGrid = new Grid();
            LoadedCommand = new RelayCommand(o => OnLoadedHandler());
            gameBoard = GameBoard;
            //GameGrid.Background = new SolidColorBrush(Colors.Blue);
        }

        private void OnLoadedHandler()
        {
            gameBoard = gameBoard.Build(GameGrid);
          
            var middleBoard = new StackPanel { Background = (SolidColorBrush)Application.Current.FindResource("AccentBrush") };
            Grid.SetColumn(middleBoard, 6);
            Grid.SetRowSpan(middleBoard, 3);
            GameGrid.Children.Add(middleBoard);
        }
    }
}
