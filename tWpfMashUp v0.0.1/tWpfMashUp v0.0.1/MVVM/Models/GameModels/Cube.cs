using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class Cube : ICube
    {
        public List<int> MyProperty { get; set; }

        public int DisplayResult()
        {
            throw new NotImplementedException();
        }

        public void Roll()
        {
            throw new NotImplementedException();
        }
    }
}
