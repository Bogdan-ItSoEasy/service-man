using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using TaskBook.Data;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{

    internal class AddComCtrlViewModel : ViewModelBase
    {

        public ICommand AddCommand { get; private set; }

        public FontSetter FontSetter { get; } = new FontSetter(FontName.AddWindowFondName);
        public AddComCtrlViewModel()
        {
            AddCommand = new RelayCommand(OnAddCommand);
            ChangeTemplateCommand = new RelayCommand(OnChangeTemplateCommand);
            AddTemplateCommand = new RelayCommand(OnAddTemplateCommand);

            _templateProvider = new TemplateProvider();
            UpdateTemplates();
            SelectedKey = TemplateProvider.TemplateNotChoiced;
            
            SettingProvider.SettingsUpdate += SettingProviderOnSettingsUpdate;
        }

        private void SettingProviderOnSettingsUpdate(object sender, EventArgs eventArgs)
        {

        }

        private void UpdateTemplates()
        {
            _templates = _templateProvider.LoadTemplates();
            TemplateList = _templates.Keys.ToList();
        }

        private void OnAddTemplateCommand()
        {
            _templateProvider.SaveTemplateDialog(Data);
            UpdateTemplates();
        }

        private void OnChangeTemplateCommand()
        {
            Data = _templates[SelectedKey];
        }

        public RelayCommand ChangeTemplateCommand { get; set; }

        public RelayCommand AddTemplateCommand { get; set; }


        private TemplateProvider _templateProvider;
        private Dictionary<string, string> _templates;

        public string SelectedKey
        {
            get => _selectedKey;
            set
            {
                SetValue(ref _selectedKey, value);
                if(_templates.ContainsKey(SelectedKey) && !string.IsNullOrEmpty(_templates[SelectedKey]))
                    Data = _templates[SelectedKey];
            }
        }

        public List<string> TemplateList
        {
            get => _templateList;
            set => SetValue(ref _templateList, value);
        }

        public string Data
        {
            get => _data;
            set => SetValue(ref _data, value);
        }
        string _data;

        public DateTime DataTime
        {
            get => _dataTime;
            set => SetValue(ref _dataTime, value);
        }
        DateTime _dataTime;

        DateTime _dataDate;
        public DateTime DataDate
        {
            get => _dataDate;
            set => SetValue(ref _dataDate, value);
        }

        public int Hour
        {
            get => _hour;
            set => SetValue(ref _hour, value);
        }
        int _hour;

        public int Min
        {
            get => _min;
            set => SetValue(ref _min, value);
        }
        int _min;

        
        public int RepeateId
        {
            get => _repeateId;
            set => SetValue(ref _repeateId, value);
        }
        private int _repeateId;
        
        public int ImportantId
        {
            get => _importantId;
            set => SetValue(ref _importantId, value);
        }
        private int _importantId;
        public AddRepeater WhenRepeate
        {
            get { return _repeaters[RepeateId]; }
            set
            {
                if (value == null) return;
                for (int i = 0; i < _repeaters.Count; ++i)
                {
                    if (_repeaters[i] == value)
                    {
                        RepeateId = i;
                        return;
                    }
                }
            }
        }

        public AddImportant WhatImportant
        {
            get => _importants[ImportantId];
            set
            {
                if (value == null) return;
                for (int i = 0; i < _importants.Count; ++i)
                {
                    if (_importants[i] == value)
                    {
                        ImportantId = i;
                        return;
                    }
                }
            }
        }

        private static ObservableCollection<AddRepeater> _repeaters = new ObservableCollection<AddRepeater>()
        {
            new AddRepeater() {Id = 0, Name = "Не повторять"},
            new AddRepeater() {Id = 1, Name = "Ежедневно"},
            new AddRepeater() {Id = 2, Name = "Еженедельно"},
            new AddRepeater() {Id = 3, Name = "Ежемесячно"},
            new AddRepeater() {Id = 4, Name = "Ежеквартально"},
            new AddRepeater() {Id = 5, Name = "Ежегодно"},
            new AddRepeater() {Id = 6, Name = "По дням недели"}
        };

        private static ObservableCollection<AddImportant> _importants = new ObservableCollection<AddImportant>()
        {
            new AddImportant() {Id = 0, Name = "Обычная"},
            new AddImportant() {Id = 1, Name = "Важная"},
            new AddImportant() {Id = 2, Name = "Очень важная"},
            new AddImportant() {Id = 3, Name = "Особо важная"}
        };

        public static ObservableCollection<AddRepeater> Repeaters
        {
            get => _repeaters;
            set => _repeaters = value;
        }

        public static ObservableCollection<AddImportant> Importants
        {
            get => _importants;
            set => _importants = value;
        }

        public event EventHandler RequestClose;


        private void OnAddCommand()
        {
            if (string.IsNullOrEmpty(Data))
            {
                MessageBox.Show("Отсутствует текст напоминания");
                return;
            }

            if (RepeateId == 6 && RemindedWeekDays == 0)
            {
                MessageBox.Show("Укажите дни недели для напоминаний");
                return;
            }


            if(_editingTask == null)
            {
                Task task = new CommonTask();
                GetDataFromForm(task);
                task.CorrectTime();
                TaskControl.GetInstance().AddTask(task);
            }
            else
            {
                GetDataFromForm(_editingTask);
                _editingTask.CorrectTime();
            }
            TaskControl.GetInstance().SaveCurrentTask();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        public CommonTask EditingTask
        {
            get => _editingTask;
            set => _editingTask = value;
        }

        public bool IsWeekCheck
        {
            get => _isWeekCheck;
            set => SetValue(ref _isWeekCheck, value);
        }

        public WeekDays RemindedWeekDays
        {
            get => _remindedWeekDays;
            set => SetValue(ref _remindedWeekDays, value);
        }

        public WeekNumbers RemindedWeekNumbers
        {
            get => _remindedWeekNumbers;
            set => SetValue(ref _remindedWeekNumbers, value);
        }

        CommonTask _editingTask;
        private List<string> _templateList;
        private string _selectedKey;
        private WeekDays _remindedWeekDays;
        private bool _isWeekCheck;
        private WeekNumbers _remindedWeekNumbers;


        private void GetDataFromForm(Task task)
        {
            task.TaskInfo = Data;
            task.TaskDate = DataDate;
            task.TaskTime = new DateTime(1,1,1,Hour,Min,0);
            task.RemindDate = DataDate;
            task.RemindTime = task.TaskTime;
            task.RepeaterId = RepeateId;
            task.ImportantId = ImportantId;
            task.RemindedWeekDays = RemindedWeekDays;
            task.RemindedWeekNumber = IsWeekCheck ? RemindedWeekNumbers : default;
        }
    }

    public class AddRepeater
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AddImportant
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}