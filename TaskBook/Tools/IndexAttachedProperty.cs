using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace TaskBook.Tools
{
    public static class IndexAttachedProperty
    {
        public static int GetTabItemIndex(DependencyObject obj) => (int) obj.GetValue(TabItemIndexProperty);

        public static void SetTabItemIndex(DependencyObject obj, int value)
        {
            obj.SetValue(TabItemIndexProperty, value);
        }

        // Using a DependencyProperty as the backing store for TabItemIndexProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabItemIndexProperty =
            DependencyProperty.RegisterAttached("TabItemIndex", typeof(int), typeof(IndexAttachedProperty), new PropertyMetadata(-1));

        public static bool GetTrackTabItemIndex(DependencyObject obj)
        {
            return (bool) obj.GetValue(TrackTabItemIndexProperty);
        }

        public static void SetTrackTabItemIndex(DependencyObject obj, bool value)
        {
            obj.SetValue(TrackTabItemIndexProperty, value);
        }


        // Using a DependencyProperty as the backing store for TrackTabItemIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackTabItemIndexProperty =
            DependencyProperty.RegisterAttached("TrackTabItemIndex", typeof(bool), typeof(IndexAttachedProperty), new PropertyMetadata(false, TrackTabItemIndexPropertyChanged));

        private static void TrackTabItemIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
                return;
            var tabControl = GetParent(d, p => p is RadTabControl) as RadTabControl;
            var tabItem = GetParent(d, p => p is TabItem) as RadTabItem;
            if(tabControl == null || tabItem == null)
                return;
            int index = tabControl.Items.IndexOf(tabItem.DataContext ?? tabItem);
            SetTabItemIndex(d, index);
        }

        public static DependencyObject GetParent(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;
            return condition(item) ? item : GetParent(VisualTreeHelper.GetParent(item), condition);
        }
    }
}
