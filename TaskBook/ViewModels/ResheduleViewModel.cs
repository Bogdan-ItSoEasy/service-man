
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
    public class ResheduleViewModel : INotifyPropertyChanged
    {
        public ResheduleViewModel()
        {
            ResheduleCommand = new RelayCommand(OnResheduleCommand);
        }

        private void OnResheduleCommand()
        {
            _data["hour"] = Hour;
            _data["min"] = Min;
            _data["date"] = Date;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ResheduleCommand { get; set; }
        protected void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
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
            get { return _data; }
            set
            {
                _data = value;
                Hour = (int)value[key: "hour"];
                Min = (int)value[key: "min"];
                Date = (DateTime)value[key: "date"];
            }
        }
        Dictionary<string, object> _data;
    }
}
