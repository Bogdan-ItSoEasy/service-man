using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TaskBook.Data;

namespace TaskBook.UserControls
{
    /// <summary>
    /// Interaction logic for WeekNumbersBox.xaml
    /// </summary>
    public partial class WeekNumbersBox
    {
        public WeekNumbersBox()
        {
            InitializeComponent();
        }

        public void SetBoxes()
        {
            var num = (int)WeekNumbersContent;
            WeekNumbersContent = 0;
            int i = 1;
            while (num != 0)
            {
                (FindName("Box" + i++) as CheckBox).IsChecked = num % 2 == 1;
                num /= 2;
            }
        }


        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox box))
                return;
            var num = int.Parse(box.Name.Substring(3), new CultureInfo("ru-Ru"));

            var day = (int)Math.Pow(2, num - 1);
            WeekNumbersContent ^= (WeekNumbers)day;
        }


        public WeekNumbers WeekNumbersContent
        {
            get => (WeekNumbers)GetValue(WeekNumbersContentProperty);
            set => SetValue(WeekNumbersContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for WeekNumbersContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WeekNumbersContentProperty =
            DependencyProperty.Register("WeekNumbersContent", typeof(WeekNumbers), typeof(WeekNumbersBox), new PropertyMetadata(default));

        private void DayOfWeekBox_OnInitialized(object sender, EventArgs e)
        {
            SetBoxes();
        }
    }
}
