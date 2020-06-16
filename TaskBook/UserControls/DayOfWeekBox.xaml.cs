using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TaskBook.Data;

namespace TaskBook.UserControls
{
    /// <summary>
    /// Interaction logic for DayOfWeekBox.xaml
    /// </summary>
    public partial class DayOfWeekBox
    {
        public DayOfWeekBox()
        {
            InitializeComponent();
        }

        public void SetBoxes()
        {
            var num = (int)WeekDaysContent;
            WeekDaysContent = 0;
            int i = 1;
            while (num != 0)
            {
                (FindName("Box" + i++) as CheckBox).IsChecked = num % 2 == 1;
                num /= 2;
            }
        }


        public WeekDays WeekDaysContent
        {
            get => (WeekDays)GetValue(WeekDaysContentProperty);
            set => SetValue(WeekDaysContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for WeekDaysContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WeekDaysContentProperty =
            DependencyProperty.Register("WeekDaysContent", typeof(WeekDays), typeof(DayOfWeekBox), new PropertyMetadata((WeekDays)default));
        
        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if(!(sender is CheckBox box))
                return;
            var num = int.Parse(box.Name.Substring(3), new CultureInfo("ru-Ru"));

            var day = (int)Math.Pow(2, num-1);
            WeekDaysContent ^= (WeekDays)day;
        }

        private void DayOfWeekBox_OnInitialized(object sender, EventArgs e)
        {
            SetBoxes();
        }
    }
}
