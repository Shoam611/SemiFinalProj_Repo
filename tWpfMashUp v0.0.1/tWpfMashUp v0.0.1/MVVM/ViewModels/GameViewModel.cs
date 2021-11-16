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
        public Grid Grid { get; set; }
        public Grid TopTabGrid { get; set; }
        public Grid GameGrid { get; set; }
        public Canvas MaskingCanvas { get; set; }
        private IGameBoard gameBoard;
        RadioButton TurnIndicator;
        public GameViewModel(IGameBoard GameBoard)
        {
            MaskingCanvas = new Canvas { Background = new SolidColorBrush(Color.FromArgb(85, 10,10, 10)) };
            gameBoard = GameBoard;
            InitGrids();
            GameBoard.OnTurnChanged += (e) =>
            {
                TurnIndicator.IsChecked = e;
                MaskingCanvas.Visibility = !e ? Visibility.Visible : Visibility.Collapsed;
            };
            Panel.SetZIndex(MaskingCanvas, 7);
            LoadedCommand = new RelayCommand(o => OnLoadedHandler());
        }

        private void InitGrids()
        {
            Grid = new Grid();
            GameGrid = new Grid();
            TopTabGrid = new Grid { Background = Application.Current.FindResource("AccentBrush") as SolidColorBrush };
            TurnIndicator = new RadioButton
            {
                Height = 30,
                Width = 90,
                Style = App.Current.FindResource("ToggleBtn") as Style
            };
            Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            Grid.RowDefinitions.Add(new RowDefinition());
            Grid.AddToGrid(TopTabGrid);

       
            TopTabGrid.Children.Add(TurnIndicator);
            Grid.AddToGrid(GameGrid, 0, 1);
        }

        private void OnLoadedHandler()
        {
            gameBoard = gameBoard.Build(GameGrid);
            
            Grid.AddToGrid(MaskingCanvas,0,1);

            var middleBoard = new StackPanel { Background = (SolidColorBrush)Application.Current.FindResource("AccentBrush") };
            Grid.SetRowSpan(middleBoard, 3);
            GameGrid.AddToGrid(middleBoard, 6, 0);
        }
    }
}
