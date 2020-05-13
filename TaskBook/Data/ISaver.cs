using System.Collections.ObjectModel;

namespace TaskBook.Data
{
    public interface ISaver
    {
        ObservableCollection<Task> Load();
        bool Save(ObservableCollection<Task> saveData);
    }
}
