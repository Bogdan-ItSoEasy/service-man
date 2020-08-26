using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using TaskBook.Tools;

namespace TaskBook.Data
{
    [DataContract]
    public class Task  : INotifyPropertyChanged
    {
        [DataMember]
        public WeekDays RemindedWeekDays
        {
            get => _remindedWeekDays;
            set
            {
                _remindedWeekDays = value;
                OnPropertyChanged("RemindedWeekDays");
            }
        }

        [DataMember]
        public WeekNumbers RemindedWeekNumber { get; set; }

        public static ObservableCollection<Repeater> RepeatersInfo { get; } = new ObservableCollection<Repeater>()
        {
            new Repeater() {Id = 0, Name = "Не повторять"},
            new Repeater() {Id = 1, Name = "Ежедневно"},
            new Repeater() {Id = 2, Name = "Еженедельно"},
            new Repeater() {Id = 3, Name = "Ежемесячно"},
            new Repeater() {Id = 4, Name = "Ежеквартально"},
            new Repeater() {Id = 5, Name = "Ежегодно"},
            new Repeater() {Id = 6, Name = "По дням недели"}

        };
        public static ObservableCollection<Important> ImportantsInfo { get; } = new ObservableCollection<Important>()
        {
            new Important() {Id = 0, Name = "Обычная"},
            new Important() {Id = 1, Name = "Важная"},
            new Important() {Id = 2, Name = "Очень важная"},
            new Important() {Id = 3, Name = "Особо важная"}
        };

        private static readonly List<string> Mounths = new List<string>()
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string v)
        {
            
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(v));
        }


        public Task()
        {
            Id = Guid.NewGuid().ToString();
            TaskType = 0;
        }

        public string ToHistoryString()
        {
            return $"{DateTime.Now:F};{TaskDate.Add(TaskTime.TimeOfDay):F};{Base64Converter.ToBase64(TaskInfo)}";
        }


        public string DayView
        {
            get
            {
                if (TaskDate.Date == DateTime.Today.Date)
                {
                    return "Сегодня";
                }
                if (TaskDate.Date == DateTime.Today.AddDays(1).Date)
                {
                    return "Завтра";
                }
                if (TaskDate.Date == DateTime.Today.AddDays(2).Date)
                {
                    return "Послезавтра";
                }
                if (TaskDate.Date <= DateTime.Today.AddDays((int)(7 - DateTime.Today.DayOfWeek)).Date)
                {
                    if (TaskDate.DayOfWeek == DayOfWeek.Monday)
                        return "В понедельник";
                    if (TaskDate.DayOfWeek == DayOfWeek.Tuesday)
                        return "Во вторник";
                    if (TaskDate.DayOfWeek == DayOfWeek.Wednesday)
                        return "В среду";
                    if (TaskDate.DayOfWeek == DayOfWeek.Thursday)
                        return "В четверг";
                    if (TaskDate.DayOfWeek == DayOfWeek.Friday)
                        return "В пятницу";
                    if (TaskDate.DayOfWeek == DayOfWeek.Saturday)
                        return "В субботу";
                    if (TaskDate.DayOfWeek == DayOfWeek.Sunday)
                        return "В воскресенье";
                }
                    
                if (TaskDate.Date <= DateTime.Today.AddDays((int)(14 - DateTime.Today.DayOfWeek)).Date)
                    return "На следующей\nнеделе";

                if (TaskDate.Date <= DateTime.Today.AddDays( (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Today.Day) ).Date)
                    return "В этом месяце";
                if (TaskDate.Date <= DateTime.Today.AddDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month) - DateTime.Today.Day).Date)
                    return "В следующем\nмесяце";

                if (TaskDate.Date <= DateTime.Today.AddDays(365 - DateTime.Now.DayOfYear).Date)
                    return "Больше месяца";

                return "В следующем\nгоду";
            }
        }

        public string RepeateView
        {
            get
            {
                if (TaskDate.Date > DateTime.Today
                        .AddDays((DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Today.Day))
                        .Date)
                {
                    return "";
                }

                switch (RepeaterId)
                {
                    case 0:
                        return "";
                    case 1:
                        return RepeatersInfo[1].Name;
                    case 2:
                        return RepeatersInfo[2].Name;
                    case 3:
                        return RepeatersInfo[3].Name;
                    case 4:
                        return RepeatersInfo[4].Name;
                    case 5:
                        return RepeatersInfo[5].Name;
                    case 6:
                        return (RemindedWeekNumber == 0 ? "" : RemindedWeekNumber.GetString() + " ") +
                               (RemindedWeekDays == 0 ? "" : RemindedWeekDays.ToString());
                    default:
                        return "";
                }
            }
        }

        public static string GetDateView(DateTime date)
        {
            return $"{date.Day} {Mounths[date.Month - 1]} {date.Year}";
        }

        public string DateView => GetDateView(TaskDate);

        public static int DateWidth => 100 * FontController.GetFontSize(FontName.MainWindowFontName) / 12;

        public static int DayWidth => 85 * FontController.GetFontSize(FontName.MainWindowFontName) / 12;

        public bool IsThatDay =>TaskDate.Date == DateTime.Today.Date || TaskDate.Date == DateTime.Today.AddDays(1).Date;


        public bool IsMissRemind()
        {
            if (IsDone || IsTrash)
                return false;

            if (TaskDate.Add(TaskTime.TimeOfDay) < DateTime.Now)
                return true;

            return false;
        }

        [DataMember]
        public virtual string TaskInfo
        {
            get => _taskInfo;
            set
            {
                _taskInfo = value;
                OnPropertyChanged("TaskInfo");
            }
        }

        public string TaskRemindInfo => TaskInfo;

        private string _taskInfo;
        [DataMember]
        public DateTime TaskDate
        {
            get => _taskDate;
            set
            {
                _taskDate = value;
                OnPropertyChanged("AllTimeView");
                OnPropertyChanged("DateView");
                OnPropertyChanged("DayView");
            }
        }

        private DateTime _taskDate;

        [DataMember]
        public DateTime TaskTime
        {
            get => _taskTime;
            set
            {
                _taskTime = value;
                OnPropertyChanged("TaskTime");
                OnPropertyChanged("AllTimeView");
            }
        }
        private DateTime _taskTime;

        public DateTime AllTimeView => TaskDate.Add(TaskTime.TimeOfDay);


        [DataMember]
        public DateTime RemindDate
        {
            get => _remindDate;
            set
            {
                _remindDate = value;
                OnPropertyChanged("RemindDate");
            }
        }

        private DateTime _remindDate;

        [DataMember]
        public DateTime RemindTime
        {
            get => _remindTime;
            set
            {
                _remindTime = value;
                OnPropertyChanged("RemindTime");
            }
        }

        private DateTime _remindTime;

        [DataMember]
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _id;

        [DataMember]
        public bool IsDone
        {
            get => _isDone;
            set
            {
                
                _isDone = value;
                OnPropertyChanged("IsDone");
            }
        }
        private bool _isDone;

        [DataMember]
        public int TaskType { get; set; }

        [DataMember]
        public bool IsTrash
        {
            get => _isTrash;
            set
            {
                _isTrash = value;
                OnPropertyChanged("IsTrash");
            }
        }
        private bool _isTrash;

        public Repeater WhenRepeat
        {
            get => RepeatersInfo[RepeaterId];
            set
            {
                if (value == null) return;
                for (int i = 0; i < RepeatersInfo.Count; ++i)
                {
                    if (RepeatersInfo[i] == value)
                    {
                        RepeaterId = i;
                        return;
                    }
                }
                OnPropertyChanged("WhenRepeat");
            }
        }
        public Important WhatImportant => ImportantsInfo[ImportantId];

        [DataMember] 
        public int RepeaterId
        {
            get => _repeaterId;
            set
            {
                _repeaterId = value;
                OnPropertyChanged("RepeaterId");
            }
        }

        private int _repeaterId;

        [DataMember]
        public int ImportantId
        {
            get => _importantId;
            set
            {
                _importantId = value;
                OnPropertyChanged("WhatImportant");
                OnPropertyChanged("ImportantId");
            }
        }
        private int _importantId;
        private WeekDays _remindedWeekDays;

        public string RepeatsType => (RepeaterId == 0) ? "Однократно": RepeatersInfo[RepeaterId].Name;

        public static bool CheckTask(Task task)
        {
            if (task is BirthTask birthTask)
            {
                if (birthTask.Name is null && birthTask.Surname is null && birthTask.Farthername is null)
                    return false;

                return true;
            }

            if (task?.TaskInfo == null)
                return false;

            return true;
        }

        public override string ToString()
        {
            return TaskInfo;
        }

        public void MoveToTrash()
        {
            IsTrash = true;
        }

        public virtual void UpdateTime()
        {
            CorrectTime();

            if (NeedToAddInHistory && IsDone)
            {
                NeedToAddInHistory = false;
                TaskControl.AddToHistoryList(this);
            }


            if (RepeaterId < 1 || RepeaterId >= RepeatersInfo.Count || IsTrash || !IsDone || TaskDate.Date == DateTime.Now.Date && SettingProvider.GetSetting("repeat_task") != bool.TrueString)
                return;

            TaskDate = RemindDate;
            TaskTime = RemindTime;

            while (TaskDate.Date.Add(TaskTime.TimeOfDay) < DateTime.Now)
            {
                if (RepeaterId == 1)
                    TaskDate = TaskDate.AddDays(1);

                if (RepeaterId == 2)
                    TaskDate = TaskDate.AddDays(7);

                if (RepeaterId == 3)
                    TaskDate = TaskDate.AddMonths(1);

                if (RepeaterId == 4)
                    TaskDate = TaskDate.AddMonths(3);

                if (RepeaterId == 5)
                    TaskDate = TaskDate.AddYears(1);

                if (RepeaterId == 6)
                {
                    if(RemindedWeekDays == 0)
                        return;
                    while (!(RemindingDayOfWeek() && RemindingWeekNumber()) || TaskDate.Date.Add(TaskTime.TimeOfDay) < DateTime.Now)
                        TaskDate = TaskDate.AddDays(1);
                }
                    

                if (IsDone)
                    IsDone = false;       
            }
        }

        public bool RemindingDayOfWeek()
        {
            return RemindingDayOfWeek(TaskDate);
        }

        public bool RemindingDayOfWeek(DateTime dateTime)
        {
            var day = (int) Math.Pow(2, (dateTime.DayOfWeek == 0 ? 6 : (int)dateTime.DayOfWeek - 1));
            return day == (day & (int) RemindedWeekDays);
        }

        private bool RemindingWeekNumber()
        {
            return RemindingWeekNumber(TaskDate);
        }

        private bool RemindingWeekNumber(DateTime dateTime)
        {
            var weekNumber = (int)Math.Pow(2, (int)((dateTime.Day - 1) / 7));

            return RemindedWeekNumber == default || (weekNumber & (int) RemindedWeekNumber) == weekNumber;
        }

        public bool NeedToAddInHistory { get; set;}

        public void CorrectTime()
        {
            if(IsDone || IsTrash)
                return;

            while (RepeaterId == 1 && TaskDate.Add(TaskTime.TimeOfDay).AddDays(1).Subtract(new TimeSpan(0,0,0,TaskTime.Second)) <= DateTime.Now )
                TaskDate = TaskDate.AddDays(1);

            while (RepeaterId == 2 && TaskDate.Add(TaskTime.TimeOfDay).AddDays(7).Subtract(new TimeSpan(0, 0, 0, TaskTime.Second)) <= DateTime.Now)
                TaskDate = TaskDate.AddDays(7);

            while (RepeaterId == 3 && TaskDate.Add(TaskTime.TimeOfDay).AddMonths(1).Subtract(new TimeSpan(0, 0, 0, TaskTime.Second)) <= DateTime.Now)
                TaskDate = TaskDate.AddMonths(1);

            while (RepeaterId == 4 && TaskDate.Add(TaskTime.TimeOfDay).AddMonths(3).Subtract(new TimeSpan(0, 0, 0, TaskTime.Second)) <= DateTime.Now)
                TaskDate = TaskDate.AddMonths(3);

            while (RepeaterId == 5 && TaskDate.Add(TaskTime.TimeOfDay).AddYears(1).Subtract(new TimeSpan(0, 0, 0, TaskTime.Second)) <= DateTime.Now)
                TaskDate = TaskDate.AddYears(1);

            if (RepeaterId == 6 && RemindedWeekDays != 0)
            {
                while (NextDayOfWeekRemind().Add(TaskTime.TimeOfDay).Subtract(new TimeSpan(0, 0, 0, TaskTime.Second)) <=
                       DateTime.Now)
                    TaskDate = NextDayOfWeekRemind();
            }
        }

        private DateTime NextDayOfWeekRemind()
        {
            var result = TaskDate.AddDays(1);
            while (!(RemindingDayOfWeek(result) && RemindingWeekNumber(result))) result = result.AddDays(1);

            return result;
        }

    }
}
