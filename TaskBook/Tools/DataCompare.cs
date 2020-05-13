using System.Collections;
using Task = TaskBook.Data.Task;

namespace TaskBook.Tools
{
    class DataCompare : IComparer
    {
        private int _coef;

        public DataCompare(int coef = 1)
        {
            _coef = coef;
        }
        public int Compare(object x, object y)
        {
            Task xTask = x as Task;
            Task yTask = y as Task;

            if (xTask != null && yTask != null)
            {
                int result = _coef * xTask.TaskDate.CompareTo(yTask.TaskDate);
                if(result == 0)
                    return _coef * xTask.TaskTime.CompareTo(yTask.TaskTime);
                return result;
            }

            return 0;
        }
    }
}
