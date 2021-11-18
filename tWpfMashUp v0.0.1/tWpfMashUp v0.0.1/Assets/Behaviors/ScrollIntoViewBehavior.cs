using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace tWpfMashUp_v0._0._1.Assets.Behaviors
{
    public class ScrollIntoViewBehavior : Behavior<ListView>
    {

        protected override void OnAttached()
        {
            ListView listview = AssociatedObject;
            ((INotifyCollectionChanged)listview.Items).CollectionChanged += OnSourceCollectionChanged;
        }

        protected override void OnDetaching()
        {
            ListView listview = AssociatedObject;
            ((INotifyCollectionChanged)listview.Items).CollectionChanged -= OnSourceCollectionChanged;
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ListView listview = AssociatedObject;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                listview.ScrollIntoView(e.NewItems[0]);
            }
            //listview.ScrollIntoView(e.NewItems[0]);
        }

    }
}
