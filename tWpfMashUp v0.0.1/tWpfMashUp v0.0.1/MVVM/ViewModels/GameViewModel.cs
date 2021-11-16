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
using System.Diagnostics;

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
            MaskingCanvas = new Canvas { Background = new SolidColorBrush(Color.FromArgb(85, 10, 10, 10)) };
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
            Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            Grid.RowDefinitions.Add(new RowDefinition());
            InitTopTabGrid();
            GameGrid = new Grid();
            Grid.AddToGrid(GameGrid, 0, 1);
        }

        private void InitTopTabGrid()
        {
            TopTabGrid = new Grid { Background = Application.Current.FindResource("AccentBrush") as SolidColorBrush };
            TopTabGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            TopTabGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            TopTabGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            TurnIndicator = new RadioButton
            {
                Style = App.Current.FindResource("ToggleBtn") as Style,
                HorizontalAlignment =HorizontalAlignment.Right,
                Margin = new Thickness(15, 0, 15, 0)
            };
            TopTabGrid.SizeChanged +=(s,e) => { TurnIndicator.Height = TopTabGrid.ActualHeight - 12;TurnIndicator.Width = TopTabGrid.ActualWidth / 6 ;  };
            var dicesStack = new Grid { Margin=new Thickness(8), VerticalAlignment=VerticalAlignment.Stretch , HorizontalAlignment=HorizontalAlignment.Stretch,Background=new SolidColorBrush(Colors.DarkGray)};
            Dices dices = new Dices(dicesStack);
            Button rollBtn = new Button
            {
                Content = "Roll",
                Style = App.Current.FindResource("RoundButton") as Style,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin=new Thickness(10)
            };
            rollBtn.Click += (s, e) =>
            {
                var res = dices.Roll();
                Debug.WriteLine(res);
            };

            TopTabGrid.AddToGrid(dicesStack);
            TopTabGrid.AddToGrid(rollBtn, 1);
            TopTabGrid.AddToGrid(TurnIndicator, 2);
            Grid.AddToGrid(TopTabGrid);
        }

        private void OnLoadedHandler()
        {
            gameBoard = gameBoard.Build(GameGrid);

            Grid.AddToGrid(MaskingCanvas, 0, 1);

            var middleBoard = new StackPanel { Background = (SolidColorBrush)Application.Current.FindResource("AccentBrush") };
            Grid.SetRowSpan(middleBoard, 3);
            GameGrid.AddToGrid(middleBoard, 6, 0);
        }
    }
}
