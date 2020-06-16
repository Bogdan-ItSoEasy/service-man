using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.Tools.FontsHandler;
using TaskBook.ViewModels;

namespace TaskBook.Tools
{

    public class FontName
    {
        protected FontName(string name, int standartSize, string standartFamily=FontsHandler.Fonts.SegoeUI)
        {
            Name = name;
            
            StandartSize = standartSize;
            StandartFamily = standartFamily;
        }

        public string Name { get; private set; }
        public int StandartSize { get; private set; }
        public string StandartFamily { get; private set; }

        public string SizeName => Name + "_size";
        public string FamilyName => Name + "_family";

        public static FontName RemindWindowFontName { get; } = new FontName("remind_window_font", 24);

        public static FontName AddWindowFondName { get; } = new FontName("add_window_font", 12);

        public static FontName MainWindowFontName { get; } = new FontName("main_window_font", 12);
    }
    

    static class FontController
    {
        public static void SetFontSize(FontName fontName, int value)
        {
            if(!SettingProvider.GetSetting(fontName.SizeName).Equals(value.ToString(new CultureInfo("ru-Ru")), StringComparison.Ordinal))
                SettingProvider.SetSetting(fontName.SizeName, value.ToString(new CultureInfo("ru-Ru")));
        }
        public static void SetFontFamily(FontName fontName, string family)
        {
            if (!SettingProvider.GetSetting(fontName.FamilyName).Equals(family, StringComparison.Ordinal))
                SettingProvider.SetSetting(fontName.FamilyName, family);
        }

        public static int GetFontSize(FontName fontName, bool isReturnStdWhenFailed=true)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[fontName.SizeName] != null ? Int32.Parse(appSettings[fontName.SizeName], new CultureInfo("ru-Ru")) : fontName.StandartSize;
            }
            catch (FormatException)
            {
                return isReturnStdWhenFailed? fontName.StandartSize : 0;
            }
        }


        public static string GetFontFamily(FontName fontName, bool isReturnStdWhenFailed = true)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[fontName.FamilyName] != null ? appSettings[fontName.FamilyName] : fontName.StandartFamily;
            }
            catch (FormatException)
            {
                return isReturnStdWhenFailed ? fontName.StandartFamily : "";
            }
        }

        public static void UpdateSize(FontName fontName, ref int oldFont)
        {
            var updatedFontSize = FontController.GetFontSize(fontName, false);
            if (updatedFontSize != default(int) && updatedFontSize != oldFont)
                oldFont = updatedFontSize;
        }


    }
}
