using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace tWpfMashUp_v0._0._1.Assets.Components.CustomModal
{

    internal delegate void ModalLoadedEventHandler(out string title, out string caption);
    internal delegate void ModalLoadedWithButtonsEventHandler(out string[] vals, out string title, out string caption);


    public partial class PopupWindow : Window
    {
        internal event ModalLoadedEventHandler ModalLoaded;
        internal event ModalLoadedWithButtonsEventHandler ModalLoadedWithButtons;

        internal PopupWindow()
        {
            InitializeComponent();
            this.Loaded += LoadedHandler;
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (ModalLoadedWithButtons != null)//.GetInvocationList().Any())
            {
                var Caption = "";
                string Title = "";
                var vals = new string[] { "" };
                ModalLoadedWithButtons?.Invoke(out vals, out Title, out Caption);
                tbCaption.Text = Caption;
                tbTitle.Text = Title;
                BuildBottomButtons(vals);
            }
            else
            if (ModalLoaded != null && ModalLoaded.GetInvocationList().Any())
            {
                string Title = " "; string Caption = " ";
                ModalLoaded?.Invoke(out Title, out Caption);
                tbCaption.Text = Caption;
                tbTitle.Text = Title;
                BuildExitButton();
            }

        }

        private void BuildExitButton()
        {
            Button btn = new Button { Content = "X", Width = 25, Height = 25, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 10, 0),
            Style=App.Current.FindResource("RoundButton") as Style };
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
            //ModalClosing?.Invoke(this, new ModalClosingEventArgs { ValueSelected = (sender as Button).Content.ToString() });
            //this.Close();
        }

    }
}
