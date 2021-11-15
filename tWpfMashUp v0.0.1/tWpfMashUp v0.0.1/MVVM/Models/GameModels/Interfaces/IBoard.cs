using Castle.Core;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces
{
    public interface IBoard
    {
        public StackModel[,] StacksMatrix { get; set; }

        GameBoard Build();

        bool Move(SoliderModel soldier, StackModel stack);

        void ShowAvailableMoves(Pair<int, int> rollRes, SoliderModel selectedSolider);
    }
}
