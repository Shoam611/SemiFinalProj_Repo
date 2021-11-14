using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using tWpfMashUp_v0._0._1.Extensions;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class StackModel
    {
        public Polygon Triangle { get; set; }
        public StackPanel UiStack { get; set; }
        public Stack<SoldierModel> SoliderStack { get; private set; }
        public StackLocation Location { get; set; }

        public StackModel(StackLocation location) => Location = location;

        public void Clear()
        {
            UiStack.Children.Clear();
            SoliderStack.Clear();
        }
     
        public StackModel Build()
        {
          SoliderStack = new Stack<SoldierModel>();
          return this;
        }

        public void Add(SoldierModel solider)
        {
            //validate not empty or null
            SoliderStack.Push(solider);
            solider.SetLocation(Location);
            UiStack.Children.Add(solider.Soldier);
        }
      
        public SoldierModel Remove()
        {
            //validate not empty or null
            var solider = SoliderStack.Peek();
            UiStack.Children.Remove(solider.Soldier);
            return solider;
        }

        public struct StackLocation
        {
            public int Row;
            public int Col;
        }
    }
}
