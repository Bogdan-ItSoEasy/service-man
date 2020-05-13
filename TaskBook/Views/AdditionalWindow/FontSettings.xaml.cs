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
using DevExpress.Xpf.Core;
using TaskBook.ViewModels;

namespace TaskBook.Views.AdditionalWindow
{
    /// <summary>
    /// Логика взаимодействия для FontSettings.xaml
    /// </summary>
    public partial class FontSettings : DXWindow
    {
        public FontSettings()
        {
            InitializeComponent();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FontSettings_OnClosed(object sender, EventArgs e)
        {
            (DataContext as FontSettingsViewModel)?.RestoreFonts();
        }
    }
}
