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
        }


        //private void btnSaveData_Click(object sender, RoutedEventArgs e)
        //{
        //    ModalClosing?.Invoke(this, new ModalClosingEventArgs { ValueSelected = (sender as Button).Content.ToString() });
        //    cnvs.Visibility = Visibility.Collapsed;
        //}


    }
}
