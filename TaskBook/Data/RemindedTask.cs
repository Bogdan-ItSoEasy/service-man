using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.ViewModels;

namespace TaskBook.Data
{
    public class RemindedTask : ViewModelBase
    {
        public RemindedTask(Task task)
        {
            Task = task;
            if (task != null) RemindDate = task.TaskDate;
            Visible = 0;
            Hour = 3;
        }


        public Task Task { get; set; }

        public DateTime RemindTime
        {
            get => _remindTime;
            set => SetValue(
                ref _remindTime, value);
        }
        DateTime _remindTime;

        public DateTime RemindDate { get; set; }
        public int Visible
        {
            get => _visible;
            set => SetValue(ref _visible, value);
        }
        int _visible;

        public int Hour
        {
            get => _hour;
            set => SetValue(ref _hour, value);
        }

        private int _hour;

        public override string ToString()
        {
            return Task.TaskInfo;
        }
    }
}
