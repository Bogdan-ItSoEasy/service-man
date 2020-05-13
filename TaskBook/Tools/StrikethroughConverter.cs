using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TaskBook.Data;
using TaskBook.ViewModels;

namespace TaskBook.Tools
{
    public class StrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool) value)
            {
                return TextDecorations.Strikethrough;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && ((int)value + Int32.Parse((string)parameter ?? throw new InvalidOperationException())) % 3 == 0)
            {
                return Visibility.Visible;
            }
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            int important  = (int)value;
            return GetColorFromRes(important);
            
        }

        public static SolidColorBrush GetColorFromRes(int important)
        {
            var getResource = new Func<object, object>(x => Application.Current?.MainWindow?.FindResource(x));
            if (important == 0 && getResource("Base") is Color baseColor)
                return new SolidColorBrush(baseColor);
            if (important == 1 && getResource("Normal") is Color normalColor)
                return new SolidColorBrush(normalColor);
            if (important == 2 && getResource("Important") is Color importantColor)
                return new SolidColorBrush(importantColor);
            if (important == 3 && getResource("VeryImportant") is Color veryImportantColor)
                return new SolidColorBrush(veryImportantColor);
            if (important == 4 && getResource("BirthDay") is Color birthDayColor)
                return new SolidColorBrush(birthDayColor);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RemindColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            int important = (int)value;

            return ColorConverter.GetColorFromRes(important);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RemindUnselectColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            int important = (int)value;
            var color = ColorConverter.GetColorFromRes(important).Color;
            const double coef = 0.75; 
            return new SolidColorBrush(new Color(){A = (byte) (color.A*coef), R = (byte)(color.R * coef), G = (byte)(color.G * coef), B = (byte)(color.B * coef) });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RemindTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            int important = (int)value;

            if (important == 0)
                return new SolidColorBrush(Colors.Black);
            if (important == 1)
                return new SolidColorBrush(Colors.Black);
            if (important == 2)
                return new SolidColorBrush(Colors.Black);
            if (important == 3)
                return new SolidColorBrush(Colors.DarkOrange);
            if (important == 4)
                return new SolidColorBrush(Colors.Black);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RemindViewColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            int important = (int)value;

            return ColorConverter.GetColorFromRes(important);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SizeConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] - 110 * (double)FontController.GetFontSize(FontName.MainWindowFontName)/12-140 - (int)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RemindWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = Int32.Parse((string)(parameter ?? "0"));
            if (value is null)
                return width;
            var border = 10;
            var tabWidth = (double)value;
            var remainder = (int)tabWidth % (width + border);
            var count = (int)tabWidth / (width + border);
            var delta = (double)remainder / count;

            return width + delta;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}