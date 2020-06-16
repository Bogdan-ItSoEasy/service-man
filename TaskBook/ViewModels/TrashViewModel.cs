
using System.Windows.Data;
using System.Windows.Input;
using System;
using System.Globalization;
using TaskBook.Data;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{
    internal class TrashViewModel : ViewModelBase
    {
        public ListCollectionView TrashTasks { get; private set; }

        public ICommand DelCommand { get; private set; }

        public ICommand ReturnCommand { get; private set; }

        public string SearchLine
        {
            get => _searchLine;
            set
            {
                _searchLine = value;
                TrashTasks.Refresh();
                OnPropertyChanged();
            }
        }
        private string _searchLine;


        public TrashViewModel()
        {
            var tc = TaskControl.GetInstance();
            TrashTasks = new CollectionViewSource { Source = tc.AllTasks }.View as ListCollectionView;

            if (TrashTasks != null)
            {
                TrashTasks.Filter = TrashFilter;
                TrashTasks.CustomSort = new DataCompare(-1);
            }


            DelCommand = new RelayCommand<Task>(OnDelCommand);
            ReturnCommand = new RelayCommand<Task>(OnReturnCommand);

        }

        private void OnReturnCommand(Task obj)
        {
            obj.IsTrash = false;
            TaskControl.GetInstance().RefreshTask(obj);
            TaskControl.GetInstance().SaveCurrentTask();
        }

        private void OnDelCommand(Task obj)
        {
            TaskControl.GetInstance().AllTasks.Remove(obj);
        }

        private bool TrashFilter(object obj)
        {
            if (obj is Task current)
            {
                var filter = true;

                if (!string.IsNullOrEmpty(SearchLine))
                {
                    string taskInfo;
                    if (current is BirthTask)
                        taskInfo = (current as BirthTask).TaskInfo;
                    else
                        taskInfo = current.TaskInfo;
                    filter = taskInfo.ToUpper(new CultureInfo("ru-Ru")).Contains(SearchLine.ToUpper(new CultureInfo("ru-Ru")));
                }

                return current.IsTrash && filter;
            }


            return false;
        }
    }
}
