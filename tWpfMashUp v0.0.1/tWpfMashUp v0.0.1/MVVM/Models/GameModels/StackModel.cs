using System;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace tWpfMashUp_v0._0._1.MVVM.Models.GameModels
{
    public class StackModel
    {
        private Brush tColor;
        public static bool HasFirstSelected;
        //a triangle in the background,
        public Polygon Triangle { get; set; }
        // a stackpanel in foreground
        public StackPanel UiStack { get; set; }
        // a stack for data(no ui connection)
        public Stack<SoliderModel> SoliderStack { get; private set; }
        //a grid matrix location indicator
        public MatrixLocation Location { get; set; }

        public int Count { get => SoliderStack.Count; }
        public event EventHandler OnClicked;
        public event EventHandler OnSelected;
        public bool IsOption { get;private set; }
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
            tColor = Triangle.Fill;
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
                OnClicked?.Invoke(this, new EventArgs());/* HasFirstSelected = true;*/
            }
        }

        public void Push(SoliderModel solider)
        {
            //validate not empty or null
            //if active make regular;
            SoliderStack.Push(solider);
            solider.SetLocation(Location);
            if(Location.Row == 1) UiStack.Children.Insert(0,solider.Soldier);
           else  UiStack.Children.Add(solider.Soldier);
        }

        public SoliderModel Pop()
        {
            //validate not empty or null
            if (SoliderStack.Count > 0)
            {
                var solider = SoliderStack.Pop();
                UiStack.Children.Remove(solider.Soldier);
                return solider;
            }
            else return null;
        }

        internal bool HasMineSoliders() => SoliderStack.Peek().IsOwnSolider;

        public SoliderModel Peek() => SoliderStack.Peek();

        public void MarkSoliderAsActive(bool isActive)
        {
            IsOption = isActive;
            var solider = SoliderStack.Peek();
            byte c = solider.IsOwnSolider ? (byte)255 : (byte)0;
            solider.Soldier.Fill = new SolidColorBrush(Color.FromArgb(isActive? (byte)125 : (byte)255, c, c, c));
        }
       
        public void MarkStackAsOption(bool isOption)
        {
            this.IsOption = isOption;
            Triangle.Fill = isOption ? new SolidColorBrush(Colors.White) : tColor;
        }

    }
}
