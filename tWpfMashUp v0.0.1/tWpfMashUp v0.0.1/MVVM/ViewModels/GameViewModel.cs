using System.Windows.Controls;
using System.Windows.Media;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class GameViewModel
    {        
        public RelayCommand GoToChatCommand { get; set; }
        public Grid GameGrid { get; set; }
        public GameViewModel()
        {
            GameGrid = new Grid();
            //GameGrid.Background = new SolidColorBrush(Colors.Blue);
        }
    }
}
