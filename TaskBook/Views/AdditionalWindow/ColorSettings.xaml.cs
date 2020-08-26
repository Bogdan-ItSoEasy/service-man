using System.Globalization;
using System.Windows;
using System.Windows.Media;
using TaskBook.Tools;


namespace TaskBook.Views.AdditionalWindow
{
    /// <summary>
    /// Interaction logic for ColorSettings.xaml
    /// </summary>
    public partial class ColorSettings
    {
        public ColorSettings()
        {
            InitializeComponent();
            
            BaseColor =
                System.Windows.Media.ColorConverter.ConvertFromString(GetColor("BaseColor")
             ) as Color? ?? Application.Current?.MainWindow?.Resources["Base"] as Color? ?? Colors.Black;

            NormalColor =
                System.Windows.Media.ColorConverter.ConvertFromString(GetColor("NormalColor")
                ) as Color? ?? Application.Current?.MainWindow?.Resources["Normal"] as Color? ?? Colors.Black;

            ImportantColor =
                System.Windows.Media.ColorConverter.ConvertFromString(GetColor("ImportantColor")
                ) as Color? ?? Application.Current?.MainWindow?.Resources["Important"] as Color? ?? Colors.Black;

            VeryImportantColor =
                System.Windows.Media.ColorConverter.ConvertFromString(GetColor("VeryImportantColor")
                ) as Color? ?? Application.Current?.MainWindow?.Resources["VeryImportant"] as Color? ?? Colors.Black;

            BirthDayColor =
                System.Windows.Media.ColorConverter.ConvertFromString(GetColor("BirthDayColor")
                ) as Color? ?? Application.Current?.MainWindow?.Resources["BirthDay"] as Color? ?? Colors.Black;
        } 

        public static string GetColor(string colorName)
        {
            var color = SettingProvider.GetSetting(colorName);

            return string.IsNullOrEmpty(colorName) || string.IsNullOrEmpty(color) ? null : color;
        }



        public Color BaseColor
        {
            get => (Color)GetValue(BaseColorProperty);
            set => SetValue(BaseColorProperty, value);
        }

        public Color NormalColor
        {
            get => (Color) GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }

        public Color ImportantColor
        {
            get => (Color) GetValue(ImportantColorProperty);
            set => SetValue(ImportantColorProperty, value);
        }

        public Color VeryImportantColor
        {
            get => (Color) GetValue(VeryImportantColorProperty);
            set => SetValue(VeryImportantColorProperty, value);
        }

        public Color BirthDayColor
        {
            get => (Color) GetValue(BirthDayColorProperty);
            set => SetValue(BirthDayColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for BaseColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseColorProperty =
            DependencyProperty.Register("BaseColor", typeof(Color), typeof(ColorSettings), new PropertyMetadata(default));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register("NormalColor", typeof(Color), typeof(ColorSettings), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty ImportantColorProperty = DependencyProperty.Register("ImportantColor", typeof(Color), typeof(ColorSettings), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty VeryImportantColorProperty = DependencyProperty.Register("VeryImportantColor", typeof(Color), typeof(ColorSettings), new PropertyMetadata(default(Color)));
        public static readonly DependencyProperty BirthDayColorProperty = DependencyProperty.Register("BirthDayColor", typeof(Color), typeof(ColorSettings), new PropertyMetadata(default(Color)));


        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            SettingProvider.SetSetting("BaseColor", BaseColor.ToString(new CultureInfo("ru-RU")));
            SettingProvider.SetSetting("NormalColor", NormalColor.ToString(new CultureInfo("ru-RU")));
            SettingProvider.SetSetting("ImportantColor", ImportantColor.ToString(new CultureInfo("ru-RU")));
            SettingProvider.SetSetting("VeryImportantColor", VeryImportantColor.ToString(new CultureInfo("ru-RU")));
            SettingProvider.SetSetting("BirthDayColor", BirthDayColor.ToString(new CultureInfo("ru-RU")));

            Close();
        }

        private void ToDefault_OnClick(object sender, RoutedEventArgs e)
        {
            BaseColor = Colors.AntiqueWhite;
            NormalColor = Colors.GreenYellow;
            ImportantColor = Colors.DarkOrange;
            VeryImportantColor = Colors.DarkRed;
            BirthDayColor = Colors.CornflowerBlue;
        }
    }
}
