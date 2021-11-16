using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using tWpfMashUp_v0._0._1.Core;
using System.Windows.Shapes;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;
using tWpfMashUp_v0._0._1.Sevices;
using tWpfMashUp_v0._0._1.Extensions;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class GameViewModel
    {

        public RelayCommand GoToChatCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }
        public bool IsMyTurn { get; set; }
        public Grid Grid { get; set; }
        public Grid TopTabGrid { get; set; }
        public Grid GameGrid { get; set; }
        public Canvas MaskingCanvas { get; set; }
        private IGameBoard gameBoard; 

        public GameViewModel(IGameBoard GameBoard)
        {
           
            InitGrids();
            LoadedCommand = new RelayCommand(o => OnLoadedHandler());
            gameBoard = GameBoard;
        }

        private void InitGrids()
        {
            Grid = new Grid();
            Grid.RowDefinitions.Add(new RowDefinition {Height=new GridLength(0.1,GridUnitType.Star) });
            TopTabGrid = new Grid {Background = Application.Current.FindResource("AccentBrush") as SolidColorBrush };
            Grid.AddToGrid(TopTabGrid);
            var elem = new RadioButton();
            elem.Content = "Is Your Turn";
            TopTabGrid.Children.Add(elem);
            Grid.RowDefinitions.Add(new RowDefinition());
            GameGrid = new Grid();
            Grid.AddToGrid(GameGrid,0,1);
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
