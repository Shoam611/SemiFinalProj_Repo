using System;
using System.Windows.Shapes;
using Castle.Core;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels.Interfaces;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class Cube : ICube
    {
        public Pair<int, int> RollsResultsValue { get; private set; }
        public Pair<Rectangle, Rectangle> RollsResults { get; private set; }


        public int DisplayResult()
        {
            //set res by res value (dimitry's algorythm)
            throw new NotImplementedException();
        }

        public Pair<int, int> Roll()
        {
            var rnd = new Random();
            RollsResultsValue = new Pair<int, int>(rnd.Next(1, 7), rnd.Next(1, 7));
            return RollsResultsValue;
        }
    }
}
