using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DevExpress.Xpf.Bars;
using TaskBook.Data;
using TaskBook.Tools;
using TaskBook.Views;
using TaskBook.Views.AdditionalWindow;
using Application = System.Windows.Application;
using FontSettings = TaskBook.Views.AdditionalWindow.FontSettings;
using Window = System.Windows.Window;

namespace TaskBook.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public ICommand SetFileNameCommand { get; private set; }
        public ICommand SetRingFileNameCommand { get; private set; }
        public ICommand SetSaveDirNameCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }
        public ICommand ImportCommand { get; private set; }
        public ICommand EnterCommand { get; private set; }
        public ICommand PlayRingCommand { get; private set; }
        public ICommand FontSettingsCommand { get; private set; }
        public ICommand RingSettingsCommand { get; private set; }
        public ICommand ColorSettingsCommand { get; private set; }

        public TaskControl TC;

        string _fileName;

        public string FileName
        {
            get => _fileName;
            set => SetValue(ref _fileName, value);
        }


        private int _repeateRingTime;

        public int RepeateRingTime
        {
            get => _repeateRingTime;
            set
            {
                if (value < 0)
                {
                    SetValue(ref _repeateRingTime, 0);
                    return;
                }

                if (value > 180)
                {
                    SetValue(ref _repeateRingTime, 180);
                    return;
                }

                SetValue(ref _repeateRingTime, value);
            }
        }

        string _ringfileName;

        public string RingFileName
        {
            get => _ringfileName;
            set => SetValue(ref _ringfileName, value);
        }

        public string RingShortFileName => _ringfileName.Split('\\').Last();

        string _savefileName;

        public string SaveDirName
        {
            get => _savefileName;
            set => SetValue(ref _savefileName, value);
        }

        bool _isRingDefault;

        public bool IsRingDefault
        {
            get => _isRingDefault;
            set => SetValue(ref _isRingDefault, value);
        }

        bool _isDirDefault;
        

        public bool IsDirDefault
        {
            get => _isDirDefault;
            set => SetValue(ref _isDirDefault, value);
        }

        public bool IsAutoRun
        {
            get => _isAutoRun;
            set => SetValue(ref _isAutoRun, value);
        }
        private bool _isAutoRun;

        public bool IsMinimize
        {
            get => _isMinimize;
            set => SetValue(ref _isMinimize, value);
        }
        private bool _isMinimize;

        public bool IsRepeat
        {
            get => _isRepeat;
            set => SetValue(ref _isRepeat, value);
        }
        private bool _isRepeat;

        public SettingViewModel()
        {
            TC = TaskControl.GetInstance();

            var appSettings = ConfigurationManager.AppSettings;

            RingFileName = appSettings["ring"] != null && !IsRingDefault ? appSettings["ring"] : "";
            SaveDirName = appSettings["dir"] != null && !IsDirDefault ? appSettings["dir"] : "";

            try
            {
                RepeateRingTime = appSettings["repeate"] != null ? Int32.Parse(appSettings["repeate"], new CultureInfo("ru-Ru")) : 5;
            }
            catch (Exception)
            {
                RepeateRingTime = 5;
            }
            
            IsAutoRun = appSettings["autorun"] == null || bool.FalseString != appSettings["autorun"];
            IsMinimize = bool.TrueString == appSettings["minimize"];
            IsRepeat = bool.TrueString == appSettings["repeat_task"];

            IsDirDefault = string.IsNullOrEmpty(SaveDirName);
            IsRingDefault = string.IsNullOrEmpty(RingFileName);

            ExportCommand = new RelayCommand(OnExportCommand);
            EnterCommand = new RelayCommand(OnEnterCommand);
            SetRingFileNameCommand = new RelayCommand(OnSetRingFileNameCommand);
            SetSaveDirNameCommand = new RelayCommand(OnSetSaveDirNameCommand);
            SetFileNameCommand = new RelayCommand(OnSetFileNameCommand);
            ImportCommand = new RelayCommand(OnImportCommand);
            PlayRingCommand = new RelayCommand(OnPlayRingCommand);
            FontSettingsCommand = new RelayCommand(OnFontSettingCommand);
            RingSettingsCommand = new RelayCommand(OnRingSettingCommand);
            ColorSettingsCommand = new RelayCommand(OnColorSettingCommand);


        }

        private void OnExportCommand()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = @"XML File|*.xml|Excel File|*.xlsx",
                Title = @"Выберете файл экспорта",
                InitialDirectory = Serializer.GetDirName()
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                saveFileDialog.Dispose();
                return;
            }

            var exportFileName = saveFileDialog.FileName;
            var splits = exportFileName.Split('.');
            var extention = splits[splits.Length - 1];

            if (extention == "xlsx" || extention == "xls")
                ExportImportProvider.ExportXls(exportFileName);
            else if (extention == "xml")
                ExportImportProvider.ExportXml(exportFileName);
            else
                MessageBox.Show(@"Файлы данного расширения не поддерживаются");

            saveFileDialog.Dispose();
        }

        public static string DefaultDirPath => Serializer.GetHomePath();

        private void OnEnterCommand()
        {
            SettingProvider.SetSetting("dir", IsDirDefault ? "" : SaveDirName);

            SettingProvider.SetSetting("repeat_task", _isRepeat.ToString(new CultureInfo("ru-Ru")));

            SettingProvider.SetSetting("repeate", RepeateRingTime.ToString(new CultureInfo("ru-Ru")));

            SettingProvider.SetSetting("minimize", _isMinimize.ToString(new CultureInfo("ru-Ru")));

            SettingProvider.SetAutoRun("autorun", _isAutoRun.ToString(new CultureInfo("ru-Ru")));

            SettingProvider.UpdateAutorun();
        }

        private void OnImportCommand()
        {
            //var listener = new TextWriterTraceListener("ImportLog.txt", "import");
            //Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;

            if (!File.Exists(FileName))
            {
                MessageBox.Show(@"Файл импорта не существует");
                return;
            }
            var splits = FileName.Split('.');
            var extention = splits[splits.Length - 1];

            if (extention == "xlsx" || extention == "xls")
                ExportImportProvider.ImportFromXls(FileName);
            else if (extention == "xml")
                ExportImportProvider.ImportFromXml(FileName);
            else
                MessageBox.Show(@"Файлы данного расширения не поддерживаются");
        }


        private void OnSetFileNameCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = @"Excel File|*.xlsx|XML File|*.xml",
                Title = @"Выберете файл экспорта",
                InitialDirectory = Serializer.GetDefaultPath(),
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                FileName = openFileDialog.FileName;

            openFileDialog.Dispose();
        }

        private void OnSetRingFileNameCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                RingFileName = openFileDialog.FileName;

            openFileDialog.Dispose();
        }

        private void OnSetSaveDirNameCommand()
        {
            var folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = Serializer.GetHomePath()
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                SaveDirName = folderBrowserDialog.SelectedPath;

            folderBrowserDialog.Dispose();
        }

        private void OnFontSettingCommand()
        {
            var window = new FontSettings();
            foreach (Window win in Application.Current.Windows)
            {
                if (win.Content is SettingView)
                {
                    window.Left = win.Left;
                    window.Top = win.Top;
                    break;
                }
            }
            window.Show();

        }

        private void OnColorSettingCommand()
        {
            var window = new ColorSettings();
            foreach (Window win in Application.Current.Windows)
            {
                if (win.Content is SettingView)
                {
                    window.Left = win.Left;
                    window.Top = win.Top;
                    break;
                }
            }
            window.Show();
        }

        private void OnRingSettingCommand()
        {
            var window = new RingSettings();
            foreach (Window win in Application.Current.Windows)
            {
                if (win.Content is SettingView)
                {
                    window.Left = win.Left;
                    window.Top = win.Top;
                    break;
                }
            }
            window.Show();
        }

        private void OnPlayRingCommand()
        {
            Reminder.GetInstance().Ring(IsRingDefault? null:RingFileName);
        }
    }
}