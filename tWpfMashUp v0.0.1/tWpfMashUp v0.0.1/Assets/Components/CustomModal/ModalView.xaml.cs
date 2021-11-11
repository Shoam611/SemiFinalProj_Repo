using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;

namespace tWpfMashUp_v0._0._1
{
    public partial class ModalView : UserControl
    {
        internal event EventHandler ModalClosing;
        private TaskCompletionSource<string> _pickButton;
        private string selection;
       
        public ModalView()
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

        public void Init(string caption, string title)
        {
            tbCaption.Text = caption;
            tbTitle.Text = string.IsNullOrWhiteSpace(title) ? "Warning!" : title;
            BuildExitButton();
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
            btn.Click += OnExit;
            var border = cnvs.Children[0] as Border;
            var grid = border.Child as Grid;
            Grid.SetColumn(btn, 0);
            Grid.SetRow(btn, 0);
            grid.Children.Add(btn);
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            ModalClosing?.Invoke(this, new EventArgs());
        }


        internal async Task<string> InitWithButtons(string caption, string title, string[] vals)
        {
            tbCaption.Text = caption;
            tbTitle.Text= title;
            BuildBottomButtons(vals);
            try
            {
                await GetSelectionAsync();
            }
            finally { ModalClosing?.Invoke(this, new EventArgs()); }
            return selection;
        }

        private void BuildBottomButtons(string[] vals)
        {
            for (int i = 0; i < vals.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(vals[i]))
                {
                    Panel.ColumnDefinitions.Add(new ColumnDefinition());
                    Button btn = new Button { Content = vals[i], Width = 50, Height = 30, VerticalAlignment = VerticalAlignment.Center, Style = App.Current.FindResource("RoundButton") as Style };
                    btn.Click += SelectionButtonClick;
                    Grid.SetColumn(btn, i);
                    Panel.Children.Add(btn);
                }
            }
        }

        private async Task<string> GetSelectionAsync()
        {
            _pickButton = new TaskCompletionSource<string>();
            try
            {
                return await _pickButton.Task;
            }
            finally { }
        }

        private void SelectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_pickButton != null)
            {
                selection = ((Button)sender).Content.ToString();
                _pickButton.TrySetResult(((Button)sender).Content.ToString());
                _pickButton = null;
                ModalClosing?.Invoke(this, new ModalClosingEventArgs { ValueSelected = ((Button)sender).Content.ToString() });
            }
        }
    }
}
