using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using Castle.Core;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class Dices : IDicesRoller
    {
        private readonly Random rnd = new Random();
        public List<int> RollsResultsValue { get; private set; }
        public List<Rectangle> RollsResults { get; private set; }
        public StackPanel Stackpanel;
        Button rollbtn;

        public List<int> Roll()
        {
            RollsResultsValue = new();

            for (int i = 0; i < 2; i++)
            {
                var res = rnd.Next(1, 7);

                RollsResultsValue.Add(res);
            }
            if (RollsResultsValue[0] == RollsResultsValue[1])
            {
                RollsResultsValue.Add(RollsResultsValue[0]);
                RollsResultsValue.Add(RollsResultsValue[0]);
                return RollsResultsValue;
            }
            DisplayResult();
            return RollsResultsValue;
        }

        public int DisplayResult()
        {
            
            for (int i = 0; i < RollsResultsValue.Count; i++)
            {
                Stackpanel.Children.Add(new Rectangle());
            }
            return RollsResultsValue.Count;
        }

        public void ClearDices()
        {
            RollsResultsValue.Clear();
            Stackpanel.Children.Clear();
        }
    }
}
