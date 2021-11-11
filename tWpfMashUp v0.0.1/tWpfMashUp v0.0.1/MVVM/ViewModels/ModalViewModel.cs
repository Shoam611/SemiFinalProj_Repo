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
                //if (ModalLoadedWithButtons != null)
                //{
                //    var Caption = "";
                //    string Title = "";
                //    var vals = new string[] { "" };
                //    ModalLoadedWithButtons?.Invoke(out vals, out Title, out Caption);
                //    Caption = Caption;
                //    Title = Title;
                //    //BuildBottomButtons(vals);

                //}
                //else
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
                Width = 25,
                Height = 25,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 10, 0),
                Style = App.Current.FindResource("RoundButton") as Style
            };
            btn.Click += btnSaveData_Click;
          //  TopGrid.Children.Add(btn);
        }
        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            ModalClosing?.Invoke(this, new ModalClosingEventArgs { ValueSelected = (sender as Button).Content.ToString() });
            cnvs.Visibility = Visibility.Collapsed;
        }

    }


}
