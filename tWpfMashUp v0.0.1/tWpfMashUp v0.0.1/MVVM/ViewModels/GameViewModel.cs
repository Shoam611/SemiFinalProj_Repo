using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using tWpfMashUp_v0._0._1.Core;
using System.Windows.Shapes;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class GameViewModel
    {
        public RelayCommand GoToChatCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }
        public Grid GameGrid { get; set; }
      //  public Grid GameGrid { get; set; }
        //  public Grid StacksGrid { get; set; }
        public GameViewModel()
        {
            GameGrid = new Grid();
            LoadedCommand = new RelayCommand(o => OnLoadedHandler());
            //GameGrid.Background = new SolidColorBrush(Colors.Blue);
        }

        private void OnLoadedHandler()
        {
            //set columns
            for (int i = 0; i < 13; i++)
            {
                if (i == 6)
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) });
                else
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //setRows
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                    GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                else
                    GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
            }
            //Add Polygons
            for (int i = 0; i < 13; i++)//cols
            {
                for (int j = 0; j < 3; j += 2)//rows
                {
                    if (i == 6) continue;
                    var triangle = new Polygon
                    {
                        Stretch = Stretch.Fill,
                        Points = j < 1 ? new PointCollection { new Point(0, 0), new Point(30, 100), new Point(60, 0) }
                                       : new PointCollection { new Point(0, 100), new Point(30, 0), new Point(60, 100) },
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Fill = j == 0 && ((i % 2 == 0 && i < 6) || (i % 2 == 1 && i > 6)) || // first row zig zag
                                j == 2 && ((i % 2 == 1 && i < 6) || (i % 2 == 0 && i > 6))    //second row zag zig
                                ? (SolidColorBrush)Application.Current.FindResource("DimBrush")
                                : (SolidColorBrush)Application.Current.FindResource("ComplimentaryBrush"),
                        Margin = new Thickness(5, 0, 5, 0)
                    };
                    Grid.SetRow(triangle, j);
                    Grid.SetColumn(triangle, i);
                    GameGrid.Children.Add(triangle);
                    var playersStack = new StackPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = j<1? VerticalAlignment.Top : VerticalAlignment.Bottom,
                        Background = new SolidColorBrush(Color.FromArgb(120, 55, 55, 55))
                    };
                    //for debug purpose; in future for moving
                    playersStack.MouseDown += (s, e) => Modal.ShowModal("Stack Pressed!");
                    triangle.MouseDown += (s, e) => Modal.ShowModal("triangle Pressed!");
                    playersStack.IsHitTestVisible= false;
                    triangle.IsHitTestVisible= false;
                    Panel.SetZIndex(playersStack, 2);
                    Panel.SetZIndex(triangle, 1);
                    Grid.SetRow(playersStack, j);
                    Grid.SetColumn(playersStack, i);
                    GameGrid.Children.Add(playersStack);
                }
            }
            var middleBoard = new StackPanel { Background = (SolidColorBrush)Application.Current.FindResource("AccentBrush") };
            Grid.SetColumn(middleBoard, 6);
            Grid.SetRowSpan(middleBoard, 3);
            GameGrid.Children.Add(middleBoard);
        }
    }
}
