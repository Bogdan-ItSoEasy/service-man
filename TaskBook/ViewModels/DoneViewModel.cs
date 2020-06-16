
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using TaskBook.Data;
using TaskBook.Tools;
using Task = TaskBook.Data.Task;

namespace TaskBook.ViewModels
{
    class DoneViewModel : ViewModelBase
    {

        public ListCollectionView DoneTasks { get; private set; }
        public ICommand DelCommand { get; private set; }

        public ICommand ReturnCommand { get; private set; }

        public string SearchLine
        {
            get => _searchLine;
            set
            {
                _searchLine = value;
                DoneTasks.Refresh();
                OnPropertyChanged();
            }
        }
        private string _searchLine;

        public DoneViewModel()
        {
            var tc = TaskControl.GetInstance();
            DoneTasks = new CollectionViewSource { Source = tc.AllTasks}.View as ListCollectionView;
            
            if (DoneTasks != null)
            {
                DoneTasks.Filter = DoneFilter;
                DoneTasks.CustomSort = new DataCompare(-1);
                
                DelCommand = new RelayCommand<Task>(OnDelCommand);
                ReturnCommand = new RelayCommand<Task>(OnReturnCommand);
            }
        }

        private void OnDelCommand(Task obj)
        {
            obj.IsTrash = true;
            TaskControl.GetInstance().RefreshTask(obj);
            TaskControl.GetInstance().SaveCurrentTask();
        }

        private void OnReturnCommand(Task obj)
        {
            obj.IsDone = false;
            TaskControl.GetInstance().RefreshTask(obj);
            TaskControl.GetInstance().SaveCurrentTask();
        }


        private bool DoneFilter(object obj)
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

                if (current.IsTrash)
                    return false;

                return current.IsDone && filter;
            }

            return false;
        }
    }
}
