using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.VisualBasic;
using System.ComponentModel;
using TaskBook.Data;
using TaskBook.Properties;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{
    internal class AddBirthCtrlViewModel : ViewModelBase
    {
        public event EventHandler RequestClose;

        public ICommand AddCommand { get; private set; }

        public AddBirthCtrlViewModel()
        {
            _days = new List<int>();
            _years = new List<int>();
            _day = 1;
            _mounth = 1;
            _year = DateTime.Now.Year;
            for (int i = 1; i < 32; i++)
            {
                _days.Add(i);
            }
            for (int i = DateAndTime.Year(DateTime.Now); i > DateAndTime.Year(DateTime.Now) - 100; i--)
            {
                _years.Add(i);
            }

            AddCommand = new RelayCommand(OnAddCommand);
        }

        private void OnAddCommand()
        {
            if (string.IsNullOrEmpty(Surname) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Farthername))
            {
                MessageBox.Show(Resources.Description);

                return;
            }

            if (_editingTask == null)
            {
                BirthTask task = new BirthTask {IsDone = true};
                GetDataFromForm(task);

                TaskControl.GetInstance().AddTask(task);
            }
            else
            {
                GetDataFromForm(_editingTask);
                _editingTask.IsDone = true;
                _editingTask.UpdateTime();
            }
            TaskControl.GetInstance().SaveCurrentTask();

            if (RequestClose != null) RequestClose.Invoke(this, EventArgs.Empty);
        }


        private void GetDataFromForm(BirthTask task)
        {
            task.Surname = Surname;
            task.Name = Name;
            task.Farthername = Farthername;
            task.TaskDate = new DateTime(_year, _mounth, _day, 0, 0, 0, DateTimeKind.Local);
            task.TaskTime = new DateTime(1, 1, 1, _hour, _min, 0, DateTimeKind.Local);
            task.RemindDate = task.TaskDate;
            task.RemindTime = task.TaskTime;
            task.UpdateTime();
            task.ImportantId = 4;
            task.Comment = Comment;
        }

        public List<int> Days
        {
            get => _days;
            set => _days = value;
        }

        private List<int> _days;

        public List<string> Mounths { get; set; } = new List<string>()
        {
            "Января",
            "Февраля",
            "Марта",
            "Апреля",
            "Мая",
            "Июня",
            "Июля",
            "Августа",
            "Сентября",
            "Октября",
            "Ноября",
            "Декабря"
        };

        public int Hour
        {
            get => _hour;
            set => SetValue(ref _hour, value);
        }
        int _hour=9;

        public int Min
        {
            get => _min;
            set => SetValue(ref _min, value);
        }
        int _min;

        public int Day
        {
            get => _day <= 0 ? 1 : _day;
            set => SetValue(ref _day, value <= 0 ? 1 : value);
        }

        public int Mounth
        {
            get => _mounth <= 0 ? 0 : _mounth - 1;
            set => SetValue(ref _mounth, value < 0 ? 1 : value + 1);
        }
        private int _mounth;

        public int Year
        {
            get => _year <= 0 ? DateTime.Now.Year : _year;
            set => SetValue(ref _year, value <= 0 ? DateTime.Now.Year : value);
        }

        public List<int> Years
        {
            get => _years;
            set => _years = value;
        }

        private List<int> _years;
        private int _day;
        private int _year;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        string _surname;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        string _name;

        public string Farthername
        {
            get => _farthername;
            set
            {
                _farthername = value;
                OnPropertyChanged();
            }
        }

        string _farthername;

        public BirthTask EditingTask
        {
            get => _editingTask;
            set => _editingTask = value;
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }
        private string _comment;

        BirthTask _editingTask;
    }
}