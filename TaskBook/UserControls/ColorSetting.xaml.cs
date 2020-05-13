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

namespace TaskBook.UserControls
{
    /// <summary>
    /// Interaction logic for ColorSetting.xaml
    /// </summary>
    public partial class ColorSetting : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorSetting), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register("ColorName", typeof(string), typeof(ColorSetting), new PropertyMetadata(default(string)));

        public ColorSetting()
        {
            InitializeComponent();
        }

        public Color SelectedColor
        {
            get { return (Color) GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public string ColorName
        {
            get { return (string) GetValue(ColorNameProperty); }
            set { SetValue(ColorNameProperty, value); }
        }
    }
}
