using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
            
            var assembly = Assembly.GetAssembly(typeof(AboutView));
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            Title.Content = $"{info.ProductName} {info.ProductMajorPart}.{info.ProductMinorPart}";

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
