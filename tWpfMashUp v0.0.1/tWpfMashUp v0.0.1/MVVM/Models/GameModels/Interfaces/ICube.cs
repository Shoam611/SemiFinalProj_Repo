using Castle.Core;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces
{
    public interface ICube
    {
        Pair<int,int> Roll();
        int DisplayResult();
    }
}
