using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskBook.Data;
using TaskBook.Tools;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace TaskBook.UserControls
{
    /// <summary>
    /// Interaction logic for RingSetting.xaml
    /// </summary>
    public partial class RingSetting : UserControl
    {
        public RingSetting()
        {
            InitializeComponent();
        }

        public string RingName
        {
            get => (string)GetValue(RingNameProperty);
            set => SetValue(RingNameProperty, value);
        }

        public string RingFileName
        {
            get => (string) GetValue(RingFileNameProperty);
            set => SetValue(RingFileNameProperty, value);
        }

        public bool IsRingDefault
        {
            get => (bool)GetValue(IsRingDefaultProperty);
            set => SetValue(IsRingDefaultProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsRingDefault.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRingDefaultProperty =
            DependencyProperty.Register("IsRingDefault", typeof(bool), typeof(RingSetting), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for RingName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingNameProperty =
            DependencyProperty.Register("RingName", typeof(string), typeof(RingSetting), new PropertyMetadata("Звук"));

        public static readonly DependencyProperty RingFileNameProperty = DependencyProperty.Register("RingFileName", typeof(string), 
            typeof(RingSetting), new PropertyMetadata(default(string)));

        private void View_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                RingFileName = openFileDialog.FileName;

            openFileDialog.Dispose();
        }

        private void Play_OnClick(object sender, RoutedEventArgs e)
        {
            Reminder.GetInstance().Ring(IsRingDefault ? null : RingFileName);
        }
    }
}
