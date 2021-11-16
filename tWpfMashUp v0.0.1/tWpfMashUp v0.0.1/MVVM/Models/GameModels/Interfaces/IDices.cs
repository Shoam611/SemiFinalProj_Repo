using Castle.Core;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces
{
    public interface IDices
    {
        Pair<int,int> Roll();
        int DisplayResult();
    }
}
