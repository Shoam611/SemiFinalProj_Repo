using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace tWpfMashUp_v0._0._1
{
    public partial class ModalCanvas : Canvas
    {
        public ModalCanvas() : base()
        {
            this.Loaded += ModalCanvas_Loaded;
        }

        private void ModalCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetZIndex(this, 4);
            Background = new SolidColorBrush(Color.FromArgb(190, 40, 40, 40));
            Border border = new() { Background = new SolidColorBrush(Colors.LightBlue), Height = 140, Width = 250, CornerRadius = new CornerRadius(15) };
            SizeChanged += (s, e) =>
            {
                SetLeft(border, ActualWidth / 2 - border.ActualWidth / 2);
                SetTop(border, ActualHeight / 2 - border.ActualHeight / 2);
            };
            SetLeft(border, ActualWidth / 2 - border.ActualWidth / 2);
            SetTop(border, ActualHeight / 2 - border.ActualHeight / 2);
            Children.Add(border);
            Grid subgrid = new() { Margin = new Thickness(0), Background = new SolidColorBrush(Colors.Transparent), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            border.Child = subgrid;
            BuildButtons(subgrid);
            this.IsVisibleChanged += OnVisibilityChanged;
            SetLeft(border, ActualWidth / 2 - border.ActualWidth / 2);
            SetTop(border, ActualHeight / 2 - border.ActualHeight / 2);
        }

        private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void BuildButtons(Grid subgrid)
        {

            Button closeBtn1 = new() { Height = 20, Width = 50, Content = "Yes", Margin = new Thickness(0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            Button closeBtn2 = new() { Height = 20, Width = 50, Content = "No", Margin = new Thickness(0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            closeBtn1.Click += CloseBtn_Click;
            closeBtn2.Click += CloseBtn_Click;

            subgrid.ColumnDefinitions.Add(new ColumnDefinition());
            subgrid.ColumnDefinitions.Add(new ColumnDefinition());
            subgrid.RowDefinitions.Add(new RowDefinition());
            subgrid.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(closeBtn1, 1);
            Grid.SetColumn(closeBtn1, 0);
            Grid.SetRow(closeBtn2, 1);
            Grid.SetColumn(closeBtn2, 1);

            subgrid.Children.Add(closeBtn1);
            subgrid.Children.Add(closeBtn2);
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
           // Modal.CollapseModal(((Button)sender).Content.ToString());
        }
    }
}
