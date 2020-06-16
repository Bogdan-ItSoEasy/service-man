using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using TaskBook.Tools;

namespace TaskBook.ViewModels
{


    public class FontSettingsViewModel: ViewModelBase
    {
        public FontSettingsViewModel()
        {
            FontSizes = new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            Fonts = TaskBook.Tools.FontsHandler.Fonts.All;

            CurrentMain = new FontSetter(FontName.MainWindowFontName, false);
            MainWindowFontSize = CurrentMain.FontSize;
            MainWindowFont = CurrentMain.FontFamily.Clone().ToString();

            CurrentAdd = new FontSetter(FontName.AddWindowFondName, false);
            AddWindowFontSize = CurrentAdd.FontSize;
            AddWindowFont = CurrentAdd.FontFamily;

            CurrentRemind = new FontSetter(FontName.RemindWindowFontName, false);
            RemindWindowFontSize = CurrentRemind.FontSize;
            RemindWindowFont = CurrentRemind.FontFamily;

            SaveCommand = new RelayCommand(OnSaveCommand);
        }


        private FontSetter CurrentMain { get; set; }
        private FontSetter CurrentAdd { get; set; }
        private FontSetter CurrentRemind { get; set; }
        
        private void OnSaveCommand()
        {
            FontController.SetFontSize(FontName.MainWindowFontName, MainWindowFontSize);
            FontController.SetFontSize(FontName.AddWindowFondName, AddWindowFontSize);
            FontController.SetFontSize(FontName.RemindWindowFontName, RemindWindowFontSize);

            FontController.SetFontFamily(FontName.MainWindowFontName, MainWindowFont);
            FontController.SetFontFamily(FontName.AddWindowFondName, AddWindowFont);
            FontController.SetFontFamily(FontName.RemindWindowFontName, RemindWindowFont);

            CurrentAdd.FontFamily = AddWindowFont;
            CurrentAdd.FontSize = AddWindowFontSize;
            CurrentMain.FontFamily = MainWindowFont;
            CurrentMain.FontSize = MainWindowFontSize;
            CurrentRemind.FontFamily = RemindWindowFont;
            CurrentRemind.FontSize = RemindWindowFontSize;
        }

        public void RestoreFonts()
        {
            FontController.SetFontSize(FontName.MainWindowFontName, CurrentMain.FontSize);
            FontController.SetFontSize(FontName.AddWindowFondName, CurrentAdd.FontSize);
            FontController.SetFontSize(FontName.RemindWindowFontName, CurrentRemind.FontSize);

            FontController.SetFontFamily(FontName.MainWindowFontName, CurrentMain.FontFamily);
            FontController.SetFontFamily(FontName.AddWindowFondName, CurrentAdd.FontFamily);
            FontController.SetFontFamily(FontName.RemindWindowFontName, CurrentRemind.FontFamily);
        }

        public List<int> FontSizes { get; }

        public List<string> Fonts { get; }

        public int RemindWindowFontSize
        {
            get => _remindWindowFontSize;
            set
            {
                SetValue(ref _remindWindowFontSize, value);
                FontController.SetFontSize(FontName.RemindWindowFontName, RemindWindowFontSize);
            }
        }

        public int MainWindowFontSize
        {
            get => _mainWindowFontSize;
            set
            {
                SetValue(ref _mainWindowFontSize, value);
                FontController.SetFontSize(FontName.MainWindowFontName, MainWindowFontSize);
            }
        }

        public int AddWindowFontSize
        {
            get => _addWindowFontSize;
            set
            {
                SetValue(ref _addWindowFontSize, value);
                FontController.SetFontSize(FontName.AddWindowFondName, AddWindowFontSize);
            }
        }

        public string RemindWindowFont
        {
            get => _remindWindowFont;
            set
            {
                SetValue(ref _remindWindowFont, Fonts.FirstOrDefault(x => x == value));
                FontController.SetFontFamily(FontName.RemindWindowFontName, RemindWindowFont);
            }
        }

        public string MainWindowFont
        {
            get => _mainWindowFont;
            set
            {
                SetValue(ref _mainWindowFont, Fonts.FirstOrDefault(x => x == value));
                FontController.SetFontFamily(FontName.MainWindowFontName, MainWindowFont);
            }
        }

        public string AddWindowFont
        {
            get => _addWindowFont;
            set
            {
                SetValue(ref _addWindowFont, Fonts.FirstOrDefault(x => x == value));
                FontController.SetFontFamily(FontName.AddWindowFondName, AddWindowFont);
            }
        }

        public ICommand SaveCommand { get; private set; }
        
        private int _remindWindowFontSize;
        private int _mainWindowFontSize;
        private int _addWindowFontSize;
        private string _remindWindowFont;
        private string _mainWindowFont;
        private string _addWindowFont;
    }
}
