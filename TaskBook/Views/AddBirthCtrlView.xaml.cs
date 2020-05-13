using System.Windows.Controls;
using TaskBook.Data;
using TaskBook.ViewModels;

namespace TaskBook.Views
{
    /// <summary>
    /// Interaction logic for AddBirthView.xaml
    /// </summary>
    public partial class AddBirthCtrlView : UserControl
    {
        public AddBirthCtrlView()
        {
            InitializeComponent();
        }

        public AddBirthCtrlView(object task)
        {
            InitializeComponent();

            var birthTask = task as BirthTask;

            if (birthTask == null)
                return;

            var dataContext = (DataContext as AddBirthCtrlViewModel);

            if (dataContext!=null)
            { 
                dataContext.EditingTask = birthTask;
                dataContext.Surname = birthTask.Surname;
                dataContext.Name = birthTask.Name;
                dataContext.Farthername = birthTask.Farthername;
                dataContext.Day = birthTask.RemindDate.Day;
                dataContext.Mounth = birthTask.RemindDate.Month - 1;
                dataContext.Year = birthTask.RemindDate.Year;
                dataContext.Min = birthTask.TaskTime.Minute;
                dataContext.Hour = birthTask.TaskTime.Hour;
                dataContext.Comment = birthTask.Comment;
            }
        }

        public string ButtonContent
        {
            get
            {
                return (string)Button.Content; 
            }
            set
            {
                Button.Content = value;
            }
        }


    }
}
