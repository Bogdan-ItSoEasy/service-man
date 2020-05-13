using System;
using TaskBook.Tools;

namespace TaskBook.Data
{
    public class HistoryTask :Task
    {
        public DateTime DoneTime { get; set; }

        public static HistoryTask FromHistoryTask(string history)
        {
            
            var split = history is null? new string[]{}: history.Split(';');
            if (split.Length < 3)
                return null;
            try
            {
                
                var doneTime = Convert.ToDateTime(split[0], default);
                var dateTime = Convert.ToDateTime(split[1], default);

                return new HistoryTask()
                {
                    TaskInfo = Base64Converter.FromBase64(split[2]), TaskDate = dateTime.Date, TaskTime = DateTime.MinValue.Add(dateTime.TimeOfDay),
                    DoneTime = doneTime
                };
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
