using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskBook.Data
{
    static class HistoryListProvider
    {
        public static List<HistoryTask> GetHistoryList()
        {
            string dirName = Serializer.GetDirName();
            string fileName = Path.Combine(dirName, "task.history");

            try
            {
                if(!File.Exists(fileName))
                    return new List<HistoryTask>();

                var lines = File.ReadAllLines(fileName);

                return lines.Length != 0? lines.Select(HistoryTask.FromHistoryTask).Where(x => x != null).ToList(): new List<HistoryTask>();
            }
            catch (IOException)
            {
                return new List<HistoryTask>();
            }
            
        }

        public static void AddToHistoryList(Task task)
        {
            string dirName = Serializer.GetDirName();
            string fileName = Path.Combine(dirName, "task.history");

            File.AppendAllLines(fileName, new[] {task.ToHistoryString()});
        }

    }
}
