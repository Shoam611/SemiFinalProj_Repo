using System;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class StackModel
    {
        public static bool HasFirstSelected;
        //a triangle in the background,
        public Polygon Triangle { get; set; }
        // a stackpanel in foreground
        public StackPanel UiStack { get; set; }
        // a stack for data(no ui connection)
        public Stack<SoliderModel> SoliderStack { get; private set; }
        //a grid matrix location indicator
        public MatrixLocation Location { get; set; }

        public event EventHandler OnClicked;
        public event EventHandler OnSelected;

        public StackModel(MatrixLocation location) => Location = location;

        public void Clear()
        {
            UiStack.Children.Clear();
            SoliderStack.Clear();
        }

        public StackModel Build()
        {
            SoliderStack = new Stack<SoliderModel>();
            UiStack.IsHitTestVisible = true;
            Triangle.IsHitTestVisible = true;
            UiStack.MouseDown += OnMouseDown;
            Triangle.MouseDown += OnMouseDown;
            return this;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HasFirstSelected)
            {
                OnSelected?.Invoke(this, new EventArgs());
            }
            else
            {
                OnClicked?.Invoke(this, new EventArgs());HasFirstSelected = true;
            }
        }

        public void Push(SoliderModel solider)
        {
            //validate not empty or null
            SoliderStack.Push(solider);
            solider.SetLocation(Location);
            UiStack.Children.Add(solider.Soldier);
        }

        public SoliderModel Pop()
        {
            //validate not empty or null
            var solider = SoliderStack.Pop();
            UiStack.Children.Remove(solider.Soldier);
            return solider;
        }


    }
}
