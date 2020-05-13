using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using TaskBook.Data;
using Task = System.Threading.Tasks.Task;

namespace TaskBook.Tools
{
    public class Reminder
    {

        public event EventHandler BeginRemind;
        public event EventHandler EndRemind;

        public TaskControl TC;


        private static Reminder _reminder;
        public static Reminder GetInstance()
        {
            return _reminder; //?? (_reminder = new Reminder());
        }

        private DateTime _currentDate;

        static Reminder()
        {
            _reminder = new Reminder();
        }

        public event EventHandler NextDay;
        protected virtual void OnNextDay()
        {
            NextDay?.Invoke(this, EventArgs.Empty);
        }

        private Reminder()
        {
            TC = TaskControl.GetInstance();
            TC.CostRemindedCollection.CollectionChanged += RemindedCollectionOnCollectionChanged;
            _secondRemind = 0;
            _currentDate = DateTime.Today;

            var t = new DispatcherTimer(DispatcherPriority.Background) { Interval = new TimeSpan(0,0,1,0)};

            t.Tick += (sender, args) =>
            {
                CheckDay();
                IsRemind();
            };
            t.Start();
            
        }

        private void CheckDay()
        {
            if(Equals(_currentDate, DateTime.Today))
                return;

            _currentDate = DateTime.Today;
            OnNextDay();
        }

        private void RemindedCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (TaskControl.GetInstance().CostRemindedCollection.Count != 0)
            {
                BeginRemind?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                EndRemind?.Invoke(this, EventArgs.Empty);
                _secondRemind = 0;                
                TaskControl.GetInstance().CostRemindedCollection = new ObservableCollection<RemindedTask>();
                TC.CostRemindedCollection.CollectionChanged += RemindedCollectionOnCollectionChanged;
            }
        }



        public bool IsRemind()
        {
            var reminder = TC.GetRemider();

            foreach (var task in reminder)
            {
                if(!TC.CostRemindedCollection.Select(x => x.Task).ToList().Contains(task))
                {
                    if(task is BirthTask)
                        BirthDayRing();
                    else
                        Ring();
                    
                    //Dispatcher.CurrentDispatcher.BeginInvoke((Action) (() =>
                        TC.CostRemindedCollection.Insert(0, new RemindedTask(task));
                }
            }
            
            DoRingRepeation();

            return true;
        }

        private void DoRingRepeation()
        {
            if (TC.CostRemindedCollection.Count > 0)
            {
                ++_secondRemind;
            }

            var appSettings = ConfigurationManager.AppSettings;
            int repeateRingTime;
            try
            {
                repeateRingTime = appSettings["repeate"] != null ? Int32.Parse(appSettings["repeate"]) : 5;
            }
            catch (Exception)
            {
                repeateRingTime = 5;
                
            }

            if (_secondRemind > repeateRingTime)
            {
                RepeateRing();
                _secondRemind = 0;
            }

        }


        private void Ring()
        {
            string path = ConfigurationManager.AppSettings["ring"];
            Ring(path);   
        }

        private void RepeateRing()
        {
            string path = ConfigurationManager.AppSettings["ring2"];
            Ring(path);
        }

        private void BirthDayRing()
        {
            string path = ConfigurationManager.AppSettings["ring3"];
            Ring(path);
        }

        public Uri GetDefaultUri()
        {
            var path = "ring.mp3";
            return new Uri(Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + '\\' + path),
                UriKind.RelativeOrAbsolute);
        }

        public void Ring(string path)
        {
            _player = new MediaPlayer();
            var uri = string.IsNullOrEmpty(path) || !File.Exists(Path.GetFullPath(path)) ?
                GetDefaultUri() :
                new Uri(Path.GetFullPath(path), UriKind.RelativeOrAbsolute);

            _player.Open(uri);

            _player.Play();
        }

        public MediaPlayer _player;

        private int _secondRemind;

    }
}
