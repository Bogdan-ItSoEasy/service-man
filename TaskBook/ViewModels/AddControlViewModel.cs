using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TaskBook.Tools;
using Task = TaskBook.Data.Task;

namespace TaskBook.ViewModels
{

    public class AddControlViewModel : INotifyPropertyChanged
    {

        public AddControlViewModel()
        {
            TaskList = new List<string>()
            {
                "Напоминания",
                "Дни Рождения"
            };
            CurrentTaskListIndex = 0;


            ChangeUc = new RelayCommand(OnChangeUc);
        }

        private void OnChangeUc()
        {
            if (ChangeUcEvent != null) ChangeUcEvent.Invoke(this, new ComonEventArgs() { Args = CurrentTaskListIndex});
        }

        public ICommand ChangeUc { get; set; }

        public List<string> TaskList
        {
            get
            {
                return _taskList;
            }
            set
            {
                _taskList = value;
                OnPropertyChanged("TaskList");
            }
        }

        internal void SetEditingTask(object task)
        {
            EditingTask = task as Task;
        }

        List<string> _taskList;

        public int CurrentTaskListIndex
        {
            get
            {
                return _currentTaskList;
            }
            set
            {
                _currentTaskList = value;
                OnPropertyChanged("TaskList");
            }
        }

        public Task EditingTask { get; private set; }

        int _currentTaskList;


        public event EventHandler<ComonEventArgs> ChangeUcEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string v)
        {
            
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}
