using System.Threading;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class GameLogicService
    {

        void AddItem(object item)
        {
            Thread.Sleep(3000);
        }
        public async void AddItemAsync(object item)
        {
            Task.Run(() => { AddItem(item); });
        }


    }
}
