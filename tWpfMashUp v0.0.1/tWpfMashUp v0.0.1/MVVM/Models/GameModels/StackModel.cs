using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class StackModel
    {
        public StackPanel Stack { get; set; }
        public List<SoldierModel> Soldiers { get; set; }
        public StackLocation Location { get; set; }

        public struct StackLocation
        {
            public int Row;
            public int Col;
        }
    }
}
