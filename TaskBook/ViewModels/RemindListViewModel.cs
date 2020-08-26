using System;
using System.Windows.Forms;
using System.Windows.Input;



using System.Diagnostics;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;
using TaskBook.Data;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{
    public class RemindListViewModel : ViewModelBase, IDisposable
    {
        public FontSetter FontSetter { get; } = new FontSetter(FontName.RemindWindowFontName);

        public RemindListViewModel()
        {

            TC = TaskControl.GetInstance();
            Reminder.GetInstance().UpdateCollection();
            RemindedCollection = TC.RemindedCollection;

            DoneCommand = new RelayCommand<string>(OnDoneCommand);

            ToBackGridCommand = new RelayCommand<RemindedTask>(OnToBackGridCommand);
            ToReshelduleGridCommand = new RelayCommand<RemindedTask>(OnToReshelduleGridCommand);
            ToOtherGridCommand= new RelayCommand<RemindedTask>(OnToOtherGridCommand);

            ResheduleCostCommand = new RelayCommand<RemindedTask>(OnCostReshedule);
            Reshedule5Command = new RelayCommand<string>(OnReshedule5Command);
            Reshedule15Command = new RelayCommand<string>(OnReshedule15Command);
            Reshedule30Command = new RelayCommand<string>(OnReshedule30Command);
            Reshedule60Command = new RelayCommand<string>(OnReshedule60Command);
            Reshedule180Command = new RelayCommand<RemindedTask>(OnReshedule180Command);
            Reshedule1DateCommand = new RelayCommand<string>(OnReshedule1DateCommand);


            MoveToTrashCommand = new RelayCommand<string>(OnMoveToTrashCommand);
            SelectedTask = TC.RemindedCollection.FirstOrDefault();
            FontSize = FontController.GetFontSize(FontName.RemindWindowFontName);
            SettingProvider.SettingsUpdate += SettingProviderOnSettingsUpdate;
        }

        private void SettingProviderOnSettingsUpdate(object sender, EventArgs e)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings["remind_window_font"] != null && Int32.Parse(appSettings["remind_window_font"], new CultureInfo("ru-Ru")) != FontSize)
                    FontSize = Int32.Parse(appSettings["remind_window_font"], new CultureInfo("ru-RU"));
            }
            catch (FormatException)
            {

            }
        }


        private void CostRemindedCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add &&
                notifyCollectionChangedEventArgs.NewItems.Count != 0)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => SelectedTask = TC.RemindedCollection.First()));
            }
                
        }

        private void OnToOtherGridCommand(RemindedTask obj)
        {
            obj.Visible = 2;
        }

        private void OnToBackGridCommand(RemindedTask obj)
        {
            obj.Visible = 0;
            
        }

        private void OnToReshelduleGridCommand(RemindedTask obj)
        {
            obj.Visible = 1;
        }

        private void OnDelCommand(string id)
        {
            TC.DelCostTaskById(id);
        }

        private void OnMoveToTrashCommand(string id)
        {
            var task = TC.GetTaskById(id);
            task.MoveToTrash();
            UpdateTask(task);
            OnDelCommand(id);
            TC.SaveCurrentTask();
        }

        private void OnReshedule1DateCommand(string id)
        {
            var task = TC.GetTaskById(id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;

            task.TaskDate = task.TaskDate.Add(new TimeSpan(1, 0, 0, 0));
            task.TaskTime = task.RemindTime;
            UpdateTask(task);
            OnDelCommand(id);
        }
        private void OnReshedule60Command(string id)
        {
            var task = TC.GetTaskById(id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;
             
            task.TaskTime = new DateTime(1,1,1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            task.TaskTime = task.TaskTime.Add(new TimeSpan(0, 60, 0));
            UpdateTask(task);
            OnDelCommand(id);
        }


        private void OnReshedule180Command(RemindedTask temp)
        {
            string id = temp.Task.Id;
            var task = TC.GetTaskById(id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;
            task.TaskTime = new DateTime(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            task.TaskTime = task.TaskTime.Add(new TimeSpan(0, 0, temp.Hour * 60, 0));
            UpdateTask(task);
            OnDelCommand(id);
        }

        private void OnReshedule30Command(string id)
        {
            var task = TC.GetTaskById(id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;
            task.TaskTime = new DateTime(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            task.TaskTime = task.TaskTime.Add(new TimeSpan(0, 0, 30, 0));
            UpdateTask(task);
            OnDelCommand(id);
        }
        private void OnReshedule15Command(string id)
        {
            var task = TC.GetTaskById(id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;
            task.TaskTime = new DateTime(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            task.TaskTime = task.TaskTime.Add(new TimeSpan(0, 0, 15, 0));
            UpdateTask(task);
            OnDelCommand(id);
        }

        private void OnReshedule5Command(string id)
        {
            var task = TC.GetTaskById(id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;
            task.TaskTime = new DateTime(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            task.TaskTime = task.TaskTime.Add(new TimeSpan(0, 0, 5, 0));
            UpdateTask(task);
            OnDelCommand(id);
        }

        private void OnCostReshedule(RemindedTask costTask)
        {
            var task = TC.GetTaskById(costTask.Task.Id);

            if (task.TaskDate.Date != DateTime.Now.Date)
                task.TaskDate = DateTime.Now.Date;
            task.TaskTime = task.TaskTime.Add(costTask.RemindTime.TimeOfDay);
            task.TaskDate = costTask.RemindDate;
            UpdateTask(task);
            OnDelCommand(costTask.Task.Id);
            return;
        }


        internal void ResheduleOther(Dictionary<string, object> data)
        {
            var task  = (SelectedTask as RemindedTask)?.Task;

            if (task == null) return;

            task.TaskTime = new DateTime(1, 1, 1, (int)data["hour"], (int)data["min"], 0);
            task.TaskDate = (DateTime)data["date"];
            UpdateTask(task);
            OnDelCommand(task.Id);
        }

        public TaskControl TC { get; set; }
        public ObservableCollection<RemindedTask> RemindedCollection { get; set; }

        public ICommand DoneCommand { get; private set; }
        public ICommand ResheduleCommand { get; private set; }
        public ICommand MoveToTrashCommand { get; private set; }

        public ICommand ToBackGridCommand { get; private set; }
        public ICommand ToReshelduleGridCommand { get; private set; }
        public ICommand ToOtherGridCommand { get; private set; }

        public ICommand ResheduleCostCommand { get; private set; }
        public ICommand Reshedule5Command { get; private set; }
        public ICommand Reshedule15Command { get; private set; }

        public ICommand Reshedule30Command { get; private set; }
        public ICommand Reshedule60Command { get; private set; }
        public ICommand Reshedule180Command { get; private set; }
        public ICommand Reshedule1DateCommand { get; private set; }
      

        private void OnDoneCommand(string id)
        {
            var task = TC.GetTaskById(id);
            task.IsDone = true;
            TaskControl.AddToHistoryList(task);
            UpdateTask(task);
            OnDelCommand(id);
            TC.SaveCurrentTask();

        }

        private void UpdateTask(Task task)
        {
            TaskControl tsControl = TaskControl.GetInstance();
            tsControl.RemoveTaskById(task.Id);
            tsControl.AddTask(task);
            TC.SaveCurrentTask();
        }

        private RemindedTask _selectedTask;

        public RemindedTask SelectedTask
        {
            get => _selectedTask;
            set => SetValue(ref _selectedTask, value);
        }

        private int _selectedIndex;
        private int _fontSize;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set => SetValue(ref _selectedIndex, value);
        }

        public int FontSize
        {
            get => _fontSize;
            set => SetValue(ref _fontSize, value);
        }

        public void Dispose()
        {
            RemindedCollection = null;
        }
    }
}