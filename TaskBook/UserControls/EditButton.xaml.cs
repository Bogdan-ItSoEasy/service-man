using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TaskBook.Data;

namespace TaskBook.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EditButton.xaml
    /// </summary>
    public partial class EditButton : INotifyPropertyChanged
    {
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(int), typeof(EditButton), new PropertyMetadata(3));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(object), typeof(EditButton), new PropertyMetadata(default(object)));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(RemindedTask), typeof(EditButton), new PropertyMetadata(default(RemindedTask)));

        public EditButton()
        {
            InitializeComponent();

            //Hour = 3;
            Binding myBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath("HourString"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(Button, ContentProperty, myBinding);

            Binding commandBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Command"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(Button, ButtonBase.CommandProperty, commandBinding);

            Binding commandParamBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath("CommandParameter"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(Button, ButtonBase.CommandParameterProperty, commandParamBinding);


            Binding toolTipBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ToolTipText"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(Button, ToolTipProperty, toolTipBinding);
        }

        public string ToolTipText { get => "Перенести на " + HourString; }

        public int Hour
        {
            get => (HourProperty != null) ? (int)GetValue(HourProperty):0;

            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                SetValue(HourProperty, value);
                OnPropertyChanged($"Hour");
                OnPropertyChanged($"HourString");
                OnPropertyChanged($"ToolTipText");
            }
        }

        public string HourString
        {
            get => Hour + GetHourName();
        }

        public object Command
        {
            get => GetValue(CommandProperty);
            set { SetValue(CommandProperty, value); }
        }

        internal RemindedTask CommandParameter

        {
            get { return (RemindedTask) GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        private string GetHourName()
        {
            
            if (Hour > 9 && Hour < 20)
                return " часов";

            if (Hour%10 == 1)
                return " час";

            if (Hour % 10 > 4)
                return " часов";
            else
                return " часа";

        }

        private void Down_But_Click(object sender, RoutedEventArgs e)
        {
            if (Hour == 1)
                return;
            --Hour;
        }

        private void Up_But_Click(object sender, RoutedEventArgs e)
        {
            ++Hour;
  
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
