using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;

namespace tWpfMashUp_v0._0._1
{
    /// <summary>
    /// Interaction logic for UserControlTest.xaml
    /// </summary>
    internal delegate void ModalLoadedEventHandler(out string title, out string caption);
    internal delegate void ModalLoadedWithButtonsEventHandler(out string[] vals, out string title, out string caption);

    public partial class UserControlTest : UserControl
    {
        internal event EventHandler ModalClosing;
        internal event ModalLoadedEventHandler ModalLoaded;
        internal event ModalLoadedWithButtonsEventHandler ModalLoadedWithButtons;
        public UserControlTest()
        {
            InitializeComponent();
            Loaded += UserControlTest_Loaded;
        }

        private void UserControlTest_Loaded(object sender, RoutedEventArgs e)
        {
            cnvs.Background = new SolidColorBrush(Color.FromArgb(200, 45, 45, 45));
            cnvs.SizeChanged += (s, e) =>
            {
                Canvas.SetLeft(modalBorder, ActualWidth / 2 - modalBorder.ActualWidth / 2);
                Canvas.SetTop(modalBorder, ActualHeight / 2 - modalBorder.ActualHeight / 2);
            };
            Canvas.SetLeft(modalBorder, this.ActualWidth / 2 - modalBorder.Width / 2);
            Canvas.SetTop(modalBorder, this.ActualHeight / 2 - modalBorder.Height / 2);
            cnvs.IsVisibleChanged += Cnvs_IsVisibleChanged;
        }

        private void Cnvs_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if ((bool)e.NewValue == true)
            //{
            //    if (ModalLoadedWithButtons != null)
            //    {
            //        var Caption = "";
            //        string Title = "";
            //        var vals = new string[] { "" };
            //        ModalLoadedWithButtons?.Invoke(out vals, out Title, out Caption);
            //        tbCaption.Text = Caption;
            //        tbTitle.Text = Title;
            //        BuildBottomButtons(vals);

            //    }
            //    else
            //    if (ModalLoaded != null && ModalLoaded.GetInvocationList().Any())
            //    {
            //        string Title = " "; string Caption = " ";
            //        ModalLoaded?.Invoke(out Title, out Caption);
            //        tbCaption.Text = Caption;
            //        tbTitle.Text = Title;
            //        BuildExitButton();
            //    }
            //}
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
            TopGrid.Children.Add(btn);
        }

        private void BuildBottomButtons(string[] vals)
        {
            int i = 0;
            foreach (var val in vals)
            {
                if (!string.IsNullOrWhiteSpace(val))
                {
                    Panel.ColumnDefinitions.Add(new ColumnDefinition());
                    Button btn = new Button { Content = val, Width = 50, Height = 30, VerticalAlignment = VerticalAlignment.Center, Style = App.Current.FindResource("RoundButton") as Style };
                    btn.Click += btnSaveData_Click;
                    Grid.SetColumn(btn, i);
                    Panel.Children.Add(btn);
                    i++;
                }
            }
        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            ModalClosing?.Invoke(this, new ModalClosingEventArgs { ValueSelected = (sender as Button).Content.ToString() });
            cnvs.Visibility = Visibility.Collapsed;
        }


    }
}
