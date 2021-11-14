using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces
{
    public interface IBoard
    {
        public StackModel[,] Location { get; set; }

        void Build();

        bool Move(SoldierModel soldier, StackModel stack);

        void ShowAvailableMoves();
    }
}
