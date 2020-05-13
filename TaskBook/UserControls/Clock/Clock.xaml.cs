using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using TaskBook.UserControls.Clock;

namespace TaskBook.UserControls.Clock
{
    /// <summary>
    /// Логика взаимодействия для Clock.xaml
    /// </summary>
    public partial class Clock : INotifyPropertyChanged
    {
        public Clock()
        {
            InitializeComponent();

            SizeChanged += ClockFace_SizeChanged;

        }

        public int Min
        {
            get
            {
                return (int)GetValue(MinProperty);
            }
            set
            {
                SetValue(MinProperty, value);
                OnPropertyChanged("Min");
            }
        }

        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(int), typeof(Clock), new PropertyMetadata(0, 
            PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if(!(dependencyObject is Clock clock))
                return;

            var value = (int)dependencyPropertyChangedEventArgs.NewValue;

            foreach (var chil in clock.GridMin.Children)
            {
                if (chil is ClockPoint curMin && curMin.Label?.Content != null)
                {
                    if(Int32.Parse(curMin.Label.Content.ToString()) == value - value % 5) 
                        curMin.Back.Fill = Brushes.Orange;
                    else
                    
                        curMin.Back.Fill = null;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Hour
        {
            get
            {
                return (int)GetValue(HourProperty);
            }
            set
            {
                SetValue(HourProperty, value);
                OnPropertyChanged("Hour");
            }
        }

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(int), typeof(Clock), new PropertyMetadata(0,
            HourPropertyChangedCallback));

        private static void HourPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is Clock clock))
                return;

            var value = (int)dependencyPropertyChangedEventArgs.NewValue;

            foreach (var chil in clock.Grid.Children)
            {
                if (chil is ClockPoint curMin && curMin.Label?.Content != null)
                {
                    if (Int32.Parse(curMin.Label.Content.ToString()) == value)
                        curMin.Back.Fill = Brushes.Aqua;
                    else

                        curMin.Back.Fill = null;
                }
            }

            foreach (var chil in clock.GridLow.Children)
            {
                if (chil is ClockPoint curMin && curMin.Label?.Content != null)
                {
                    var hour = Int32.Parse(curMin.Label.Content.ToString());
                    if (hour == value || value == 0 && hour == 24)
                        curMin.Back.Fill = Brushes.Aqua;
                    else

                        curMin.Back.Fill = null;
                }
            }
        }

        protected void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public double HoursLenght
        {
            set
            {
                foreach (var chil in Grid.Children)
                {
                    if (chil is ClockPoint hour)
                        hour.Length = value;
                }

                foreach (var chil in GridLow.Children)
                {
                    if (chil is ClockPoint hour)
                        hour.Length = value;
                }

                foreach (var chil in GridMin.Children)
                {
                    if (chil is ClockPoint hour)
                        hour.Length = value;
                }
            }
        }



        private void ClockFace_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var chil in Grid.Children)
            {
                if (chil is ClockPoint hour)
                    hour.Radius = Grid.ActualHeight / (2 * hour.Grid.ActualHeight);
            }

            foreach (var chil in GridLow.Children)
            {
                if (chil is ClockPoint hour)
                    hour.Radius = GridLow.ActualHeight / (2 * hour.Grid.ActualHeight);
            }

            foreach (var chil in GridMin.Children)
            {
                if (chil is ClockPoint hour)
                    hour.Radius = GridMin.ActualHeight / (2 * hour.Grid.ActualHeight);
            }

        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Source is ClockPoint hour)
            {
                if (GridMin.Children.Contains(hour))
                {
                    foreach (var chil in GridMin.Children)
                    {
                        if (chil is ClockPoint curMin)
                            curMin.Back.Fill = null;
                    }
                    hour.Back.Fill = Brushes.Orange;
                    Min = Int32.Parse(hour.Label.Content.ToString());
                }
                else
                {
                    foreach (var chil in Grid.Children)
                        if (chil is ClockPoint curHour)
                            curHour.Back.Fill = null;

                    foreach (var chil in GridLow.Children)
                        if (chil is ClockPoint curHour)
                           curHour.Back.Fill = null;

                    hour.Back.Fill = Brushes.Aqua;
                    var newValue = Int32.Parse(hour.Label.Content.ToString());
                    Hour = newValue != 24? newValue: 0;
                }
            }
        }
    }
}
