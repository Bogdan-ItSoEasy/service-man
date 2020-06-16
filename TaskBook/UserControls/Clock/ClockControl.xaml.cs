using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using TaskBook.UserControls.Clock;

namespace TaskBook.UserControls.Clock
{
    /// <summary>
    /// Логика взаимодействия для ClockControl.xaml
    /// </summary>
    public partial class ClockControl : INotifyPropertyChanged
    {
        public ClockControl()
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

        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(int), typeof(ClockControl), new PropertyMetadata(0, 
            PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if(!(dependencyObject is ClockControl clock))
                return;

            var value = (int)dependencyPropertyChangedEventArgs.NewValue;

            foreach (var chil in clock.GridMin.Children)
            {
                if (chil is ClockPoint curMin && curMin.Label?.Content != null)
                {
                    if(Int32.Parse(curMin.Label.Content.ToString(), new CultureInfo("ru-Ru")) == value - value % 5) 
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

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(int), typeof(ClockControl), new PropertyMetadata(0,
            HourPropertyChangedCallback));

        private static void HourPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is ClockControl clock))
                return;

            var value = (int)dependencyPropertyChangedEventArgs.NewValue;

            foreach (var chil in clock.Grid.Children)
            {
                if (chil is ClockPoint curMin && curMin.Label?.Content != null)
                {
                    if (Int32.Parse(curMin.Label.Content.ToString(), new CultureInfo("ru-Ru")) == value)
                        curMin.Back.Fill = Brushes.Aqua;
                    else

                        curMin.Back.Fill = null;
                }
            }

            foreach (var chil in clock.GridLow.Children)
            {
                if (chil is ClockPoint curMin && curMin.Label?.Content != null)
                {
                    var hour = Int32.Parse(curMin.Label.Content.ToString(), new CultureInfo("ru-Ru"));
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

        public double HoursLength
        {
            get => Grid.Children.Count != 0 ? (Grid.Children[0] as ClockPoint)?.Length ?? 0 : 0;
            set
            {
                foreach (var children in Grid.Children)
                {
                    if (children is ClockPoint hour)
                        hour.Length = value;
                }

                foreach (var children in GridLow.Children)
                {
                    if (children is ClockPoint hour)
                        hour.Length = value;
                }

                foreach (var children in GridMin.Children)
                {
                    if (children is ClockPoint hour)
                        hour.Length = value;
                }
            }
        }



        private void ClockFace_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var children in Grid.Children)
            {
                if (children is ClockPoint hour)
                    hour.Radius = Grid.ActualHeight / (2 * hour.Grid.ActualHeight);
            }

            foreach (var children in GridLow.Children)
            {
                if (children is ClockPoint hour)
                    hour.Radius = GridLow.ActualHeight / (2 * hour.Grid.ActualHeight);
            }

            foreach (var children in GridMin.Children)
            {
                if (children is ClockPoint hour)
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
                    Min = Int32.Parse(hour.Label.Content.ToString(), new CultureInfo("ru-Ru"));
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
                    var newValue = Int32.Parse(hour.Label.Content.ToString(), new CultureInfo("ru-Ru"));
                    Hour = newValue != 24? newValue: 0;
                }
            }
        }
    }
}
