using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using UserControl = System.Windows.Controls.UserControl;
using TaskBook.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DevExpress.Xpf.Core;
using TaskBook.Data;
using TaskBook.Tools;
using Telerik.Windows.Controls;

namespace TaskBook.Views
{
    /// <summary>
    /// Interaction logic for RemindListView.xaml
    /// </summary>
    public partial class RemindListView : UserControl, IDisposable
    {
        public RemindListView()
        {
            InitializeComponent();
            IsPinned = true;
            var rc = TaskControl.GetInstance().RemindedCollection;
            rc.CollectionChanged += CostRemindedCollectionOnCollectionChanged;
        }
        public void Dispose()
        { 
            TaskControl.GetInstance().RemindedCollection.CollectionChanged -= CostRemindedCollectionOnCollectionChanged;
            Tab.BindingGroup = null;
            Tab.ItemsSource = null;
            (DataContext as RemindListViewModel)?.Dispose();
        }

        private void CostRemindedCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add &&
                e.NewItems.Count != 0)
            { 
                Tab.SelectedIndex = 0;
                //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => ));
            }
        }

        private void Reshedule_OnClick(object sender, RoutedEventArgs e)
        {
            if (((sender as System.Windows.Controls.Button)?.Parent as Grid)?.Parent is Grid par)
                foreach (var children in par.Children)
                {
                    var grid = children as Grid;
                    if (grid != null && grid.Name == "Grid1")
                        grid.Visibility = Visibility.Hidden;

                    if (grid != null && grid.Name == "Grid2")
                        grid.Visibility = Visibility.Visible;


                    if (grid != null && grid.Name == "Grid3")
                        grid.Visibility = Visibility.Hidden;
                }
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (((sender as System.Windows.Controls.Button)?.Parent as Grid)?.Parent is Grid par)
                foreach (var children in par.Children)
                {
                    var grid = children as Grid;
                    if (grid != null && grid.Name == "Grid1")
                        grid.Visibility = Visibility.Visible;

                    if (grid != null && grid.Name == "Grid2")
                        grid.Visibility = Visibility.Hidden;

                    if (grid != null && grid.Name == "Grid3")
                        grid.Visibility = Visibility.Hidden;
                }

        }

        private void Other_OnClick(object sender, RoutedEventArgs e)
        {
            if ((((sender as System.Windows.Controls.Button)?.Parent as StackPanel)?.Parent as Grid)?.Parent is Grid par)
                foreach (var children in par.Children)
                {
                    var grid = children as Grid;
                    if (grid != null && grid.Name == "Grid1")
                        grid.Visibility = Visibility.Hidden;

                    if (grid != null && grid.Name == "Grid2")
                        grid.Visibility = Visibility.Hidden;

                    if (grid != null && grid.Name == "Grid3")
                        grid.Visibility = Visibility.Visible;
                }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(this);

            if (window != null )
            {
                if (e.ClickCount == 2)
                {
                    window.WindowState = (window.WindowState == WindowState.Maximized)
                        ? WindowState.Normal
                        : WindowState.Maximized;
                }
                

                window.DragMove();
                
                //window.WindowState = WindowState.Normal;
            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this);
            if (wnd == null)
                return;
            

            int width = 450;
            int height = 400;
            
            var task = (Tab.SelectedContent as RemindedTask)?.Task;
            if (task == null) return;

            int hour = task.TaskTime.TimeOfDay.Hours;
            int min = task.TaskTime.TimeOfDay.Minutes;
            DateTime date = task.TaskDate;
            var data = new Dictionary<string, object> {{"hour", hour}, {"min", min}, {"date", date}};


            var window = new DXWindow() {Topmost = true, Width= width, Height = height, Top= wnd.Top, Left = wnd.Left + wnd.Width/2- width/2, Content = new RescheduleView(data), ResizeMode=ResizeMode.NoResize };
            window.Closing += (s, en) =>
            {
                Properties.Settings.Default.ResheduleWindowPosition = window.RestoreBounds;
                Properties.Settings.Default.Save();
                (DataContext as PresentViewModel)?.Refresh();
            };
            PresentView.RestoreWindow(Properties.Settings.Default.ResheduleWindowPosition, window, height, width);
            if (window.ShowDialog() == true)
                (DataContext as RemindListViewModel)?.ResheduleOther(data);
        }
        

        private void Change_view_OnClick(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if(window is null)
                return;
            window.ShowInTaskbar = !window.ShowInTaskbar;
            window.Topmost = !window.Topmost;
            IsPinned = !IsPinned;
        }



        public bool IsPinned
        {
            get { return (bool)GetValue(IsPinnedProperty); }
            set { SetValue(IsPinnedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPinned.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPinnedProperty =
            DependencyProperty.Register("IsPinned", typeof(bool), typeof(RemindListView), new PropertyMetadata(true));

        private void ActiveMainWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.IsVisible ?? false)
                mainWindow.Activate();
            else
                mainWindow?.ShowHideMainWindow();
        }

        public bool IsInEditMode
        {
            get { return (bool)GetValue(IsInEditModeProperty); }
            set { SetValue(IsInEditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsInEditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(RemindListView), new PropertyMetadata(false));



        private void EditTask_OnClick(object sender, RoutedEventArgs e)
        {
            IsInEditMode = !IsInEditMode;
        }

        private void Tab_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            IsInEditMode = false;
        }
    }
}
