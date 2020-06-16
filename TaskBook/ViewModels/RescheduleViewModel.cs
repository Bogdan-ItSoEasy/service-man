
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{
    public class RescheduleViewModel : INotifyPropertyChanged
    {
        public RescheduleViewModel()
        {
            RescheduleCommand = new RelayCommand(OnRescheduleCommand);
        }

        private void OnRescheduleCommand()
        {
            _data["hour"] = Hour;
            _data["min"] = Min;
            _data["date"] = Date;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RescheduleCommand { get; set; }
        protected void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public int Hour
        {
            get => _hour;
            set
            {
                _hour = value;

                OnPropertyChanged("Hour");
            }
        }
        int _hour;

        public int Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;

                OnPropertyChanged("Min");
            }
        }
        int _min;

        DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }

        }

        public Dictionary<string, object> Data
        {
            get => _data;
            set
            {
                _data = value;
                if (value != null)
                {
                    Hour = (int) value[key: "hour"];
                    Min = (int) value[key: "min"];
                    Date = (DateTime) value[key: "date"];
                }
            }
        }
        Dictionary<string, object> _data;
    }
}
