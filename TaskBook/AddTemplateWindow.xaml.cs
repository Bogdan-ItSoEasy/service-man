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
using System.Windows.Shapes;
using DevExpress.Data.XtraReports.ServiceModel.DataContracts;
using TaskBook.Views;

namespace TaskBook
{
    /// <summary>
    /// Логика взаимодействия для AddTemplateWindow.xaml
    /// </summary>
    public partial class AddTemplateWindow
    {
        public AddTemplateWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.Windows.Cast<Window>().First(x => x.Content.GetType() == typeof(AddControlView) || 
                                                                         x.Content.GetType() == typeof(AddComCtrlView));
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
        }

        public AddTemplateWindow(string tmpTemplate)
        {
            InitializeComponent();

            TemplateBox.Text = tmpTemplate;
            Loaded += OnLoaded;
        }

        public string TemplateKey{ get; set; }
        
        public string TemplateData { get; set; }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            TemplateKey = KeyBox.Text;
            TemplateData = TemplateBox.Text;

            Close();
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
