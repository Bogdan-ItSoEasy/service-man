using System.ComponentModel;
using System.Windows;

namespace TaskBook.UserControls.Clock
{
    /// <summary>
    /// Логика взаимодействия для ClockPoint.xaml
    /// </summary>
    public partial class ClockPoint : INotifyPropertyChanged
    {
        public ClockPoint()
        {
            InitializeComponent();
        }


        public object Hour
        {
            get { return Label.Content; }
            set
            {
                Label.Content = value;
            }
        }

        public double Length
        {
            get { return Grid.ActualHeight; }
            set
            {
                Grid.Height = value;
                Grid.Width = value;
            }
        }

        public double Radius
        {

            set
            {
                Grid.RenderTransformOrigin = new Point(0.5, value);
            }
        }

        public double Angle
        {
            get { return Rotate.Angle; }
            set
            {
                Rotate.Angle = value;
                LabelRotate.Angle = -value;
            }

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
    }
}
