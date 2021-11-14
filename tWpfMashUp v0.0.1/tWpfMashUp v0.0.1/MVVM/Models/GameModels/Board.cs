using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class Board : IBoard
    {
        public StackModel[,] Location { get; set; }
        public string CurrentPlayer { get; set; }
        public bool IsBlocked { get; set; }
        public void Build()
        {
            throw new NotImplementedException();
        }

        public bool Move(SoldierModel soldier, StackModel stack)
        {
            throw new NotImplementedException();
        }

        public void ShowAvailableMoves()
        {
            throw new NotImplementedException();
        }
    }
}
