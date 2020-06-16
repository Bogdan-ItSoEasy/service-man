using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.Data;
using TaskBook.ViewModels;

namespace TaskBook.Tools
{
    public class FontSetter: ViewModelBase
    {
        private int _fontSize;
        private readonly FontName _fontName;
        private string _fontFamily;

        public FontSetter(FontName fontName, bool isUpdateSetting = true)
        {
            _fontName = fontName;
            if(isUpdateSetting)
                SettingProvider.SettingsUpdate += SettingProvider_SettingsUpdate;
            if (_fontName != null)
            {
                FontSize = FontController.GetFontSize(_fontName, false);
                FontFamily = FontController.GetFontFamily(_fontName, false);
            }
        }
        private void SettingProvider_SettingsUpdate(object sender, EventArgs e)
        {
            FontSize = FontController.GetFontSize(_fontName, false);
            FontFamily = FontController.GetFontFamily(_fontName, false);
            if(_fontName == FontName.MainWindowFontName)
                foreach (var allTask in TaskControl.GetInstance().AllTasks)
                {
                    allTask.OnPropertyChanged("DateWidth");
                    allTask.OnPropertyChanged("DayWidth");
                }
        }

        public int FontSize
        {
            get => _fontSize;
            set => SetValue(ref _fontSize, value);
        }

        public string FontFamily
        {
            get => _fontFamily;
            set => SetValue(ref _fontFamily, value);
        }
    }
}
