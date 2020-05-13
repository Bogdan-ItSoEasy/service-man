using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Data;
using System.Windows.Input;
using DevExpress.Xpf.CodeView;
using TaskBook.Data;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{

    public class PresentViewModel: ViewModelBase
    {
  
        public ICommand RefreshCommand { get; private set; }
        public ICommand DoneCommand { get; private set; }
        public ICommand DelCommand { get; private set; }
        public ICommand GetNextCommand { get; private set; }
        public ICommand GetPrevCommand { get; private set; }

        public FontSetter FontSetter { get; } = new FontSetter(FontName.MainWindowFontName);

        public PresentViewModel()
        {
            ViewList = new List<string>
            {
                "Все напоминания", "Дни Рождения", "Задачи"
            };
            
            _tc = TaskControl.GetInstance();

            Tc.AllTasks.CollectionChanged += (s, e) =>
            {
                if (e.Action.ToString() == "Add")
                {
                    foreach (var item in e.NewItems)
                        if (item is Task task)
                            AllTasksView.Add(task);
                }
                else if (e.Action.ToString() == "Remove")
                    foreach (var item in e.OldItems)
                        if (item is Task task)
                            AllTasksView.Remove(task);
                ViewTasks.Refresh();
                
            };
            
            AllTasksView = new ObservableCollection<Task>(Tc.AllTasks);

            Tc.CollectionUpdate += (sender, args) => Refresh();

            Reminder.GetInstance().NextDay += (sender, args) => UpdateDelimerTasks();
            CreateDelimerTasks();
            ViewTasks = new CollectionViewSource { Source = AllTasksView }.View as ListCollectionView;
            AddDelimerTasks();

            if (ViewTasks != null)
            {
                ViewTasks.Filter = TasksViewFilter;
                
                ViewTasks.CustomSort = new DataCompare();
            }

            RefreshCommand = new RelayCommand(OnRefreshCommand);
            DoneCommand = new RelayCommand<object>(OnDoneCommand);
            DelCommand = new RelayCommand<Task>(OnDelCommand);
            GetNextCommand = new RelayCommand<Task>(OnGetNextCommand);
            GetPrevCommand = new RelayCommand<Task>(OnGetPrevCommand);
        }

        private void OnGetNextCommand(Task obj)
        {
            ViewIndex = (ViewIndex + 1) % ViewList.Count;
        }

        private void OnGetPrevCommand(Task obj)
        {
            ViewIndex = (ViewIndex == 0 )? ViewList.Count - 1 : ViewIndex - 1;
        }

        List<SeparatorTask> _delimerTasks;

        public string SearchLine
        {
            get => _searchLine;
            set
            {
                _searchLine = value;
                OnRefreshCommand();
                OnPropertyChanged("SearchLine");
            }
        }
        string _searchLine;

        
        private void CreateDelimerTasks()
        {
            _delimerTasks = new List<SeparatorTask>();

            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "Прошедшие", TaskDate = new DateTime(1,1,1), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "Сегодня " + Task.GetDateView(DateTime.Today), TaskDate = DateTime.Now.Date, TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "Завтра " + Task.GetDateView(DateTime.Today.AddDays(1)), TaskDate = DateTime.Now.Date.AddDays(1), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "В течение недели", TaskDate = DateTime.Now.Date.AddDays(2), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "В течение месяца", TaskDate = DateTime.Now.Date.AddDays(7), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "В течение трех месяцев", TaskDate = DateTime.Now.Date.AddDays(30), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "В течение года", TaskDate = DateTime.Now.Date.AddDays(90), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
            _delimerTasks.Add(new SeparatorTask() { TaskInfo = "Через год", TaskDate = DateTime.Now.Date.AddDays(365), TaskTime = new DateTime(1, 1, 1, 0, 0, 0), ImportantId = -1 });
        }

        private void UpdateDelimerTasks()
        {
            RemoveDelimerTasks();
            CreateDelimerTasks();
            AddDelimerTasks();
        }

        private void RemoveDelimerTasks()
        {
            foreach (var delim in _delimerTasks)
                _allTasksView.Remove(delim);
        }


        private void AddDelimerTasks()
        {
            foreach (var task in _delimerTasks)
                AllTasksView.Add(task);
        }

        private void OnDelCommand(Task obj)
        {
            obj.IsTrash = true;
            Tc.RefreshTask(obj);
            TaskControl.GetInstance().SaveCurrentTask();
        }

        private void OnDoneCommand(object obj)
        {
            if (obj is Task task)
                task.NeedToAddInHistory = task.IsDone;
        }

        private void OnRefreshCommand()
        {
            Refresh();
        }

        public void Refresh()
        {
            ViewTasks?.Refresh();
        }

        public ObservableCollection<Task> AllTasksView
        {
            get => _allTasksView;
            set
            {
                _allTasksView = value;
                OnPropertyChanged("AllTasksView");
            }
        }
        ObservableCollection<Task> _allTasksView;


        public TaskControl Tc 
        {
            get => _tc;
            set
            {
                _tc= value;
                OnPropertyChanged("TC");
            }
        }
        private TaskControl _tc;

        
        public List<string> ViewList
        {
            get => _viewList;
            set
            {
                _viewList = value;
                OnPropertyChanged("ViewList");
            }
        }
        List<string> _viewList;

        public int ViewIndex
        {
            get => _viewIndex;
            set
            {
                switch(value)
                { 
                    case 0:
                        ViewTasks.Filter = TasksViewFilter;
                        break;
                    case 1:
                        ViewTasks.Filter = BirthDayViewFilter;
                        break;
                    case 2:
                        ViewTasks.Filter = CommonViewFilter;
                        break;
                }

                _viewIndex = value;
                OnPropertyChanged("ViewIndex");
            }
        }
        int _viewIndex;
        

        public ListCollectionView ViewTasks
        {
            get;
            private set;
        }


        private bool TasksViewFilter(object obj)
        {
            if (obj is SeparatorTask)
                return IsViewDelimerTask(obj as SeparatorTask, null);

            Task current = obj as Task;
            if (current != null)
            {
                if (current.IsTrash)
                    return false;

                var filter = true;
                if (!string.IsNullOrEmpty(SearchLine))
                {
                    string taskInfo;
                    if (current is BirthTask)
                        taskInfo = (current as BirthTask).TaskInfo;
                    else
                        taskInfo = current.TaskInfo;
                    filter = taskInfo.ToUpper().Contains(SearchLine.ToUpper());
                }
                    

                return !current.IsDone && filter;
            }

            return false;
        }

        private bool BirthDayViewFilter(object obj)
        {
            if (obj is SeparatorTask)
                return IsViewDelimerTask(obj as SeparatorTask, typeof(BirthTask));

            if (obj is BirthTask current)
            { 
                if (current.IsTrash)
                    return false;

                var filter = true;
                if (!string.IsNullOrEmpty(SearchLine))
                {
                    var taskInfo = current.TaskInfo;
                    filter = taskInfo.ToUpper().Contains(SearchLine.ToUpper());
                }

                return !current.IsDone && filter;
            }

            return false;
        }

        private bool CommonViewFilter(object obj)
        {
            if (obj is SeparatorTask)
                return IsViewDelimerTask(obj as SeparatorTask, typeof(CommonTask));

            if (obj is CommonTask current)
            {
                if (current.IsTrash)
                    return false;

                var filter = true;
                if (!string.IsNullOrEmpty(SearchLine))
                {
                    var taskInfo = current.TaskInfo;
                    filter = taskInfo.ToUpper().Contains(SearchLine.ToUpper());
                }
                return !current.IsDone && filter;
            }

            return false;
        }
        private bool IsViewDelimerTask(SeparatorTask separatorTask, Type type)
        {
            if (separatorTask.Equals(_delimerTasks[_delimerTasks.Count - 1]))
            {
                if (Tc.AllTasks.FindIndex(e => separatorTask.TaskDate <= e.TaskDate && !e.IsDone && !e.IsTrash && (e.GetType() == type || type == null)) == -1)
                    return false;
                else
                    return true;
            }
            var index = _delimerTasks.IndexOf(separatorTask);
            if (index == -1 && index >= _delimerTasks.Count - 1) return false;

            var next = _delimerTasks[index + 1];
            

            if (Tc.AllTasks.FindIndex(e => separatorTask.TaskDate <= e.TaskDate && e.TaskDate < next.TaskDate && !e.IsDone && !e.IsTrash && (e.GetType() == type || type == null)) == -1)
                return false;
            
            return true;
        }


    }
}