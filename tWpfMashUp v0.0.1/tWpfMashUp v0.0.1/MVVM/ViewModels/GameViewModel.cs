using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class GameViewModel
    {
        public RelayCommand GoToChatCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }
        public Grid GameGrid { get; set; }
        public Grid PolygonsGrid { get; set; }
        public Grid StacksGrid { get; set; }
        public GameViewModel()
        {
            GameGrid = new Grid();
            PolygonsGrid = new Grid();
            LoadedCommand = new RelayCommand(o => OnLoadedHandler());
            //GameGrid.Background = new SolidColorBrush(Colors.Blue);
        }

        private void OnLoadedHandler()
        {
            //set columns
            for (int i = 0; i < 13; i++)
            {
                if (i == 6)
                {
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) });
                    PolygonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) });
                }
                else
                {
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    PolygonsGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
            //setRows
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    PolygonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }
                else
                {
                    GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
                    PolygonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
                }
            }
            //SetZIndex
            Panel.SetZIndex(GameGrid, 2);
            Panel.SetZIndex(PolygonsGrid, 1);
            //Add Polygons
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                }
            }
        }
    }
}
