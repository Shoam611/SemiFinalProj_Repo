using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using tWpfMashUp_v0._0._1.Extensions;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class StackModel
    {
        //a triangle in the background,
        public Polygon Triangle { get; set; }
        // a stackpanel in foreground
        public StackPanel UiStack { get; set; }
        // a stack for data(no ui connection)
        public Stack<SoliderModel> SoliderStack { get; private set; }
        //a grid matrix location indicator
        public MatrixLocation Location { get; set; }

        public StackModel(MatrixLocation location) => Location = location;

        public void Clear()
        {
            UiStack.Children.Clear();
            SoliderStack.Clear();
        }
     
        public StackModel Build()
        {
          SoliderStack = new Stack<SoliderModel>();
          return this;
        }

        public void Add(SoliderModel solider)
        {
            //validate not empty or null
            SoliderStack.Push(solider);
            solider.SetLocation(Location);
            UiStack.Children.Add(solider.Soldier);
        }
      
        public SoliderModel Remove()
        {
            //validate not empty or null
            var solider = SoliderStack.Peek();
            UiStack.Children.Remove(solider.Soldier);
            return solider;
        }

       
    }
}
