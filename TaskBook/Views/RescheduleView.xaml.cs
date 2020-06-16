using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TaskBook.ViewModels;

namespace TaskBook.Views
{
    /// <summary>
    /// Логика взаимодействия для RescheduleView.xaml
    /// </summary>
    public partial class RescheduleView : UserControl
    {
        public RescheduleView()
        {
            InitializeComponent();
        }

        public RescheduleView(Dictionary<string, object> data)
        {
            InitializeComponent();

            if (!(DataContext is RescheduleViewModel context)) return;

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
