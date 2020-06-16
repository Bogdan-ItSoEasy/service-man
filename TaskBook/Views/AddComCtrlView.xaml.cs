using System.Windows.Controls;
using TaskBook.ViewModels;

namespace TaskBook.Views
{
    /// <summary>
    /// Interaction logic for AddView.xaml
    /// </summary>
    partial class AddComCtrlView : UserControl
    {
        internal AddComCtrlView()
        {
            InitializeComponent();


        }

        internal AddComCtrlView(AddComCtrlViewModel context)
        {
            InitializeComponent();

            DataContext = context;
      
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
