using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TaskBook.Data;

namespace TaskBook.Views
{
    /// <summary>
    /// Логика взаимодействия для DoneListView.xaml
    /// </summary>
    public partial class HistoryView
    {
        public HistoryView()
        {
            InitializeComponent();

            HistoryTasks = new ObservableCollection<HistoryTask>(TaskControl.GetInstance().GetHistoryList());
            InitDataView();
        }

        public ObservableCollection<HistoryTask> HistoryTasks
        {
            get => (ObservableCollection<HistoryTask>)GetValue(HistoryTasksProperty);
            set => SetValue(HistoryTasksProperty, value);
        }

        public string SearchLine    
        {
            get => (string) GetValue(SearchLineProperty);
            set
            {
                SetValue(SearchLineProperty, value);
                var a = CollectionViewSource.GetDefaultView(HistoryGrid.ItemsSource);
                a.Refresh();
            }
        }

        // Using a DependencyProperty as the backing store for HistoryTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HistoryTasksProperty =
            DependencyProperty.Register("HistoryTasks", typeof(ObservableCollection<HistoryTask>), typeof(HistoryView), new PropertyMetadata(new ObservableCollection<HistoryTask>()));

        public static readonly DependencyProperty SearchLineProperty = DependencyProperty.Register("SearchLine", typeof(string), typeof(HistoryView), new PropertyMetadata(default(string), 
            PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if(dependencyObject is HistoryView view)
                CollectionViewSource.GetDefaultView(view.HistoryGrid.ItemsSource).Refresh();
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(HistoryGrid.ItemsSource);
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count != 0 && view.SortDescriptions[0].PropertyName == e.Column.SortMemberPath)
                direction = view.SortDescriptions[0].Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(e.Column.SortMemberPath, direction));
            e.Handled = true;
            
        }

        private void InitDataView()
        {
            var view = CollectionViewSource.GetDefaultView(HistoryGrid.ItemsSource);
            ListSortDirection direction = ListSortDirection.Descending;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("DoneTime", direction));

            view.Filter = DoneFilter;
        }

        private bool DoneFilter(object obj)
        {
            if (obj is HistoryTask current)
                return string.IsNullOrEmpty(SearchLine) || current.TaskInfo.ToUpper().Contains(SearchLine.ToUpper());

            return false;
        }
    }
}
