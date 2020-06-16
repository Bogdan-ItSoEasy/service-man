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
            ChangeUcEvent?.Invoke(this, new CommonEventArgs() { Args = CurrentTaskListIndex});
        }

        public ICommand ChangeUc { get; set; }

        public List<string> TaskList { get;}

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


        public event EventHandler<CommonEventArgs> ChangeUcEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string v)
        {
            
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
