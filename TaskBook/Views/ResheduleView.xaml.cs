using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TaskBook.ViewModels;

namespace TaskBook.Views
{
    /// <summary>
    /// Логика взаимодействия для ResheduleView.xaml
    /// </summary>
    public partial class ResheduleView : UserControl
    {
        public ResheduleView()
        {
            InitializeComponent();
        }

        public ResheduleView(Dictionary<string, object> data)
        {
            InitializeComponent();

            var context = DataContext as ResheduleViewModel;
            if (context == null) return;

            context.Data = data;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this);
            if (wnd != null)
            {
                wnd.DialogResult = true;
                wnd.Close();
            }
            
        }
    }
}
