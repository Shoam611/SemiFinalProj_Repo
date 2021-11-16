using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using Castle.Core;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class Dices : IDices
    {
        public List<int> RollsResultsValue { get; private set; }
        public List<Rectangle> RollsResults { get; private set; }
        StackPanel stackpanel = {//holds all dices// }
        public List<int> Roll()
        {
            //= new List
            for (int i = 0; i < 2; i++)
            {
                //roll rnd = new Rando
                //add to ress
                RollsResultsValue.Add(rnd);
            }
            if(/*[0] == [1]*/)
            {
                //add tow more rolls
            }
            DisplayResult()
        }

        public int DisplayResult()
        {
            for
        }

    }
}
