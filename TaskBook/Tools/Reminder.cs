﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
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

        public TaskControl Tc { get; set; }


        private static readonly Reminder _reminder = new Reminder();
        public static Reminder GetInstance()
        {
            return _reminder; //?? (_reminder = new Reminder());
        }

        private DateTime _currentDate;
        
        public event EventHandler NextDay;
        protected virtual void OnNextDay()
        {
            NextDay?.Invoke(this, EventArgs.Empty);
        }

        private Reminder()
        {
            Tc = TaskControl.GetInstance();
            Tc.RemindedCollection.CollectionChanged += RemindedCollectionOnCollectionChanged;
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


        public void UpdateCollection()
        {
            Tc.RemindedCollection.CollectionChanged -= RemindedCollectionOnCollectionChanged;
            Tc.RemindedCollection = new ObservableCollection<RemindedTask>(Tc.RemindedCollection);
            Tc.RemindedCollection.CollectionChanged += RemindedCollectionOnCollectionChanged;
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
            if (TaskControl.GetInstance().RemindedCollection.Count != 0)
            {
                BeginRemind?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                EndRemind?.Invoke(this, EventArgs.Empty);
                _secondRemind = 0;
                //Tc.RemindedCollection.CollectionChanged += RemindedCollectionOnCollectionChanged;
            }
        }

        public bool IsRemind()
        {
            var reminder = Tc.GetReminder();

            foreach (var task in reminder)
            {
                if(!Tc.RemindedCollection.Select(x => x.Task).ToList().Contains(task))
                {
                    if(task is BirthTask)
                        BirthDayRing();
                    else
                        Ring();

                    var rt = new RemindedTask(task);
                    Tc.RemindedCollection.Insert(0, rt);
                }
            }
            
            DoRingRepeation();

            return true;
        }

        private void DoRingRepeation()
        {
            if (Tc.RemindedCollection.Any())
            {
                ++_secondRemind;
            }

            var appSettings = ConfigurationManager.AppSettings;
            int repeateRingTime;
            try
            {
                repeateRingTime = appSettings["repeate"] != null ? Int32.Parse(appSettings["repeate"], new CultureInfo("ru-Ru")) : 5;
            }
            catch (FormatException)
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

        public static Uri GetDefaultUri()
        {
            var path = "ring.mp3";
            return new Uri(Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + '\\' + path),
                UriKind.RelativeOrAbsolute);
        }

        public void Ring(string path)
        {
            Player = new MediaPlayer();
            var uri = string.IsNullOrEmpty(path) || !File.Exists(Path.GetFullPath(path)) ?
                GetDefaultUri() :
                new Uri(Path.GetFullPath(path), UriKind.RelativeOrAbsolute);

            Player.Open(uri);

            Player.Play();
        }

        public MediaPlayer Player { get; set; }

        private int _secondRemind;

    }
}
