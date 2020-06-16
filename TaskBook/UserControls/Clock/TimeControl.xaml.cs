using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace TaskBook.UserControls.Clock
{
    /// <summary>
    /// Логика взаимодействия для TimeControl.xaml
    /// </summary>
    public partial class TimeControl : INotifyPropertyChanged
    {
        public TimeControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
        }

        public int HourControl
        {
            get
            {
                return (int)GetValue(HourControlProperty);
            }
            set
            {
                SetValue(HourControlProperty, value);
                SetTime();
                OnPropertyChanged("HourControl");
            }
        }

        public void UpdateTime()
        {
            HourControl += 0;
            MinControl += 0;
        }

        public static readonly DependencyProperty HourControlProperty = DependencyProperty.Register("HourControl", typeof(int), typeof(TimeControl), new FrameworkPropertyMetadata(0, PropChangeCallback)
        { BindsTwoWayByDefault = true });

        private static void PropChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeControl = d as TimeControl;
            timeControl?.SetTime();
        }

        public int MinControl
        {
            get
            {
                return (int)GetValue(MinControlProperty);
            }
            set
            {
                SetValue(MinControlProperty, value);
                SetTime();
                OnPropertyChanged("MinControl");
            }
        }

        public static readonly DependencyProperty MinControlProperty = DependencyProperty.Register("MinControl", typeof(int), typeof(TimeControl), new PropertyMetadata(0, PropChangeCallback));

        private static string itoc2(int value)
        {
            return (value < 10) ? "0" + value : value.ToString(new CultureInfo("ru-Ru"));
        }
        
        private void SetTime()
        {
            TimeBlock.Text = String.Format(new CultureInfo("ru-Ru"), "{0,2}:{1,2}", itoc2(HourControl), itoc2(MinControl));
        }

        private void Up_But_Click(object sender, RoutedEventArgs e)
        {
            if (MinControl == 59)
            {
                MinControl = 0;
                HourControl = (HourControl == 23) ? 0 : HourControl + 1;
            }
            else
                ++MinControl;
        }

        private void Down_But_Click(object sender, RoutedEventArgs e)
        {
            
            if (MinControl == 0)
            { 
                MinControl = 59;
                HourControl = (HourControl == 0) ? 23 : HourControl - 1;
            }
            else
                --MinControl;
        }

        private void Up_But_Hour_Click(object sender, RoutedEventArgs e)
        {
            if (HourControl == 23)
            {
                HourControl = 0;
            }
            else
                ++HourControl;
        }

        private void Down_But_Hour_Click(object sender, RoutedEventArgs e)
        {

            if (HourControl == 0)
            {
                HourControl = 23;
            }
            else
                --HourControl;
        }
    }
}
