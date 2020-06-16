using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskBook.Data
{
    public class TaskControl 
    {
        private TaskControl()
        {
            AllTasks = new ObservableCollection<Task>();
            Saver = new Serializer();

            RemindedCollection = new ObservableCollection<RemindedTask>();
            
            LoadTask();
            UpdateTime();
            
        }

        ~TaskControl()
        {
            SaveCurrentTask();
            
        }

        public event EventHandler CollectionUpdate;

        protected virtual void OnCollectionUpdate()
        {
            CollectionUpdate?.Invoke(this, EventArgs.Empty);
        }

        public void SetMissedRemindedCollection()
        {
            SetRemindedCollection(GetMissedReminder());
        }

        public bool IsNewRemindTask(Task task)
        {
            foreach (var cost in RemindedCollection)
                if (cost.Task.Equals(task))
                    return false;

            return true;
        }

        public void SetRemindedCollection(List<Task> list)
        {
            if (list != null)
                foreach (var task in list)
                    if (IsNewRemindTask(task))
                        RemindedCollection.Insert(0, new RemindedTask(task));
        }

        private static TaskControl _tc;

        public static TaskControl GetInstance()
        {
            return _tc ?? (_tc = new TaskControl());
        }

        internal void RefreshTask(Task obj)
        {
            if(AllTasks.FirstOrDefault(i => i.Equals(obj)) != null)
            {
                AllTasks.Remove(obj);
                AllTasks.Add(obj);
            }

        }

        public bool SaveCurrentTask()
        {
            Saver.Save(AllTasks);
            return true;
        }

        public bool LoadTask()
        {
            foreach (var task in Saver.Load())
            {
                AddTask(task);
            }
            
            return true;
        }

        public ObservableCollection<Task> AllTasks { get; }

        private ISaver Saver { get; }

        public Task GetTaskById(string id)
        {
            foreach (var task in AllTasks)
            {
                if (task.Id == id) return task;
            }
            return new Task();
        }

        public bool RemoveTaskById(string id)
        {
            return AllTasks.Remove(GetTaskById(id));
        }

        public bool AddTask(Task newTask)
        {

            if (!Task.CheckTask(newTask))
                return false;

            if (newTask != null && newTask.RemindDate == DateTime.MinValue)
                newTask.RemindDate = newTask is BirthTask task? task.BirthDay: newTask.TaskDate;

            if (newTask != null && newTask.RemindTime == DateTime.MinValue)
                newTask.RemindTime = newTask.TaskTime;

            AllTasks.Add(newTask);

            return true;
        }

        public List<Task> GetReminder()
        {
            UpdateTime();

            List<Task> result = new List<Task>();
            foreach (var task in AllTasks)
                if(!task.IsDone && !task.IsTrash)
                    if (task.TaskDate.Year == DateTime.Now.Year && task.TaskDate.Month == DateTime.Now.Month && task.TaskDate.Day == DateTime.Now.Day)
                        if (task.TaskTime.Hour == DateTime.Now.Hour && task.TaskTime.Minute == DateTime.Now.Minute)
                            result.Add(task);

            return result;
        }

        public List<Task> GetMissedReminder()
        {
            List<Task> res = new List<Task>();

            foreach (var task in AllTasks)
            {
                task.CorrectTime();
                if (task.IsMissRemind())
    
                    res.Add(task);
            }
            res.Sort((task, task1) => task.TaskDate.Add(task.TaskTime.TimeOfDay).CompareTo(task1.TaskDate.Add(task1.TaskTime.TimeOfDay)));
            return res;
        }

        public void UpdateTime()
        {
            foreach (var task in AllTasks)
            {
                task.UpdateTime();
            }
            OnCollectionUpdate();
        }

        public ObservableCollection<RemindedTask> RemindedCollection { get; }

        public void DelCostTaskById(string id)
        {
            foreach(RemindedTask coTask in RemindedCollection)
            {
                if (id == coTask.Task.Id)
                {
                    RemindedCollection.Remove(coTask);
                    
                    return;
                }
            }
        }
        
        internal bool IsNewBirthTask(BirthTask birthTask)
        {
            foreach (var task in AllTasks)
            {
                if(!(task is BirthTask bTask))
                    continue;

                if (bTask.TaskDate.Date == birthTask.TaskDate.Date && bTask.Surname == birthTask.Surname && bTask.Name == birthTask.Name && bTask.Farthername == birthTask.Farthername)
                    return false;
            }

            return true;
        }

        public bool IsNewCommonTask(CommonTask task)
        {
            foreach (var curTask in AllTasks)
            {
                if (!(curTask is CommonTask cTask))
                    continue;

                if (task != null && (cTask.TaskDate.Date == task.TaskDate.Date && cTask.TaskInfo == task.TaskInfo && cTask.TaskTime == task.TaskTime))
                    return false;
            }

            return true;
        }

        public static void AddToHistoryList(Task task)
        {
            if (task != null) HistoryListProvider.AddToHistoryList(task);
        }

        public static List<HistoryTask> GetHistoryList()
        {
            return HistoryListProvider.GetHistoryList();
        }
    }
}
