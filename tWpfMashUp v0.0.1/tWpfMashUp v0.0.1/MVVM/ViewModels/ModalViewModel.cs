using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ModalViewModel : ObservableObject
    {
        Canvas cnvs;

        internal event EventHandler ModalClosing;
        internal event ModalLoadedEventHandler ModalLoaded;
        internal event ModalLoadedWithButtonsEventHandler ModalLoadedWithButtons;

        public RelayCommand OnLoadedCommand { get; set; }
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; onProppertyChange(); }
        }
        private string caption;

        public string Caption
        {
            get { return caption; }
            set { caption = value; onProppertyChange(); }
        }

        public ModalViewModel()
        {
            OnLoadedCommand = new RelayCommand(o => OnLoaded(o as RoutedEventArgs));
        }

        private void OnLoaded(RoutedEventArgs routedEventArgs)
        {
            cnvs = routedEventArgs.Source as Canvas;
            cnvs.IsVisibleChanged += OnIsVisibleChanged;
        }

        public void Init()
        {
            cnvs.Visibility = Visibility.Visible;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
              
                if (ModalLoaded != null && ModalLoaded.GetInvocationList().Any())
                {
                    string title = " "; string caption = " ";
                    ModalLoaded?.Invoke(out title, out caption);
                    Caption = caption;
                    Title = title;
                    BuildExitButton();
                }
            }
        }

        private void BuildExitButton()
        {
            Button btn = new Button
            {
                Content = "X",
                Width = 30,
                Height = 30,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 10, 0),
                Style = App.Current.FindResource("RoundButton") as Style
            };
            btn.Command = new RelayCommand(o => OnExit(btn, o as RoutedEventArgs));

            var border = cnvs.Children[0] as Border;
            var grid = border.Child as Grid;

            Grid.SetColumn(btn, 0);
            Grid.SetRow(btn, 0);
            grid.Children.Add(btn);
        }
        private void OnExit(object sender, RoutedEventArgs e)
        {
            ModalClosing?.Invoke(this, new ModalClosingEventArgs { ValueSelected = (sender as Button).Content.ToString() });
            cnvs.Visibility = Visibility.Collapsed;
        }

    }


}
