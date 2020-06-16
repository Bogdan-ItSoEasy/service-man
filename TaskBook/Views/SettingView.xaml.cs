using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskBook.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
            SetEnable(CheckBox1.IsChecked, Box1, Button1);      
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window?.Close();
        }

        private void SetEnable(bool? isEnable, TextBox box, Button button)
        {
            if (isEnable == true)
            {
                box.IsEnabled = false;
                button.IsEnabled = false;
            }
            else
            {
                box.IsEnabled = true;
                button.IsEnabled = true;
            }
        }


        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
            SetEnable(CheckBox1.IsChecked, Box1, Button1);
        }
        

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

  

    }
}
