using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using DevExpress.Xpf.Core;
using TaskBook.Views;
using MessageBox = System.Windows.MessageBox;
using System.Threading;

using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using DevExpress.Xpf.Core.Native;
using Microsoft.Win32;
using TaskBook.Data;
using TaskBook.Properties;
using TaskBook.Tools;
using Application = System.Windows.Application;

namespace TaskBook
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {

        public MainWindow()
        {
            InitializeComponent();
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(MainWindow)).Location);
            Title = $"{info.ProductName} {info.ProductMajorPart}.{info.ProductMinorPart}";

            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            
            Loaded += MainWindow_load;

            reminder = Reminder.GetInstance();
            reminder.BeginRemind += Reminder_BeginRemind;
            reminder.EndRemind += Reminder_EndRemind;

            UpdateTheme(this, null);
            SettingProvider.SettingsUpdate += UpdateTheme; 

            Content = new PresentView();

            RestorePresentWindow();
            
            SetResize(Application.Current.MainWindow);
            
            SaveWindow();
            CreateTrayIcon();
        }

        void SetResize(Window window)
        {
            window.SizeChanged += (sender, args) => SavePresentWindow();
            window.LocationChanged += (sender, args) => SavePresentWindow();
        }

        private void UpdateTheme(object sender, EventArgs e)
        {
            SetColor("Base");
            SetColor("Normal");
            SetColor("Important");
            SetColor("VeryImportant");
            SetColor("BirthDay");

            foreach (var task in TaskControl.GetInstance().AllTasks)
            {
                task.OnPropertyChanged("ImportantId");
            }
        }

        private void SetColor(string colorName)
        {
            if (!Resources.Contains(colorName))
                return;

            var backUp = Resources[colorName];
            try
            {
                if (string.IsNullOrEmpty(SettingProvider.GetSetting(colorName+"Color")))
                    Resources[colorName] =
                        System.Windows.Media.ColorConverter.ConvertFromString(SettingProvider.GetSetting(colorName + "Color"));
            }
            catch (FormatException)
            {
                Resources[colorName] = backUp;
            }
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch(e.Mode)
            {
                case PowerModes.Resume:
                    TaskControl.GetInstance().UpdateTime();
                    TaskControl.GetInstance().SetMissedRemindedCollection();
                    break;

            }
        }

        private void Reminder_BeginRemind(object sender, EventArgs e)
        {
            if (rw == null) 
                CreateRemindWindow();
        }

        private void Reminder_EndRemind(object sender, EventArgs e)
        {
            CloseRemindWindow();
        }

        Reminder reminder;
        Mutex mutex = new Mutex(false, "TaskBook");

        void MainWindow_load(object sender, RoutedEventArgs e)
        {
            if(!mutex.WaitOne(500,false))
            {
                MessageBox.Show("Данное приложение уже запущено.");
                System.Windows.Application.Current.Shutdown();
            }


            var appSettings = ConfigurationManager.AppSettings;
            if (bool.TrueString == appSettings["minimize"])
                Visibility = Visibility.Hidden;
        }

        private RemindWindow rw;
        private Window sw;

        public void GetSettingWindow()
        {
            if (sw == null)
            {
                int height = 500;
                int width = 450;
                sw = new DXWindow() { Content = new SettingView(), Title = "Настройки", Height = height, Width = width, ResizeMode = ResizeMode.NoResize };
                SetResize(sw);
                sw.SizeChanged += (s, en) =>
                {
                    Properties.Settings.Default.SettingWindowPosition = sw.RestoreBounds;
                    Properties.Settings.Default.Save();
                    sw = null;
                };
                
                var rect = Properties.Settings.Default.SettingWindowPosition;
                PresentView.RestoreWindow(rect, sw, height, width, true);
                sw.ResizeMode = ResizeMode.NoResize;
                sw.Show();
            }
            else
            {
                MessageBox.Show("Данное окно уже открыто!");
                sw.Activate();
            }
        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            GetSettingWindow();
        }


        private void CreateRemindWindow()
        {
            rw = new RemindWindow();
            SetResize(rw);

            Rect bounds = Properties.Settings.Default.RemindWindow;

            if(bounds.Equals(new Rect(0,0,0,0)))
            {
                rw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }
           

            rw.Top = bounds.Top;
            rw.Left = bounds.Left;

            if (rw.SizeToContent == SizeToContent.Manual)
            {
                rw.Width = bounds.Width;
                rw.Height = bounds.Height;
            }

            rw.Show();
        }

     
        protected override void OnStateChanged(EventArgs eventArgs)
        {
            base.OnStateChanged(eventArgs);

            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                Hide();
            }
            else
            {
                CurrentWindowState = WindowState;
            }
        }

        private NotifyIcon TrayIcon = null;
        private System.Windows.Controls.ContextMenu TrayMenu = null;


        private void CreateTrayIcon()
        {
            
            if (TrayIcon == null)
            {
                TrayIcon = new NotifyIcon
                {
                    Icon = TaskBook.Properties.Resources.icon2,
                    Text = TaskBook.Properties.Resources.ResourceManager.GetString("Description", CultureInfo.CurrentCulture),
                };
                TrayMenu = Resources["TrayMenu"] as System.Windows.Controls.ContextMenu;
                TrayIcon.DoubleClick+= delegate
                {
                    ShowWindow();
                };
                TrayIcon.Click += delegate(object sender, EventArgs args)
                {
                    var mouseEventArgs = args as System.Windows.Forms.MouseEventArgs;
                    if (mouseEventArgs != null && mouseEventArgs.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        ShowHideMainWindow();
                    }
                    else if(mouseEventArgs != null && mouseEventArgs.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        TrayMenu.IsOpen = true;
                        Activate();
                    }
                };
                
            }
            TrayIcon.Visible = true;

        }

        
        
        private void ShowWindow()
        {
            Show();
            WindowState = CurrentWindowState;
            Activate();
        }

        public void ShowHideMainWindow()
        {
            TrayMenu.IsOpen = false;
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                ShowWindow();
            }
        }

        public WindowState CurrentWindowState { get; set; } = WindowState.Normal;

        private void RestorePresentWindow()
        {
            Rect bounds = Properties.Settings.Default.PresentWindowsPosition;
            var curBounds = SystemInformation.VirtualScreen;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            if (bounds.Equals(new Rect(0, 0, 0, 0)) || curBounds.Top < bounds.Top || curBounds.Left < bounds.Left)
            {
                this.Height = 500;
                this.Width = 500;
                return;
            }

            this.Top = bounds.Top;
            this.Left = bounds.Left;

            if (this.SizeToContent == SizeToContent.Manual)
            {
                this.Width = bounds.Width;
                this.Height = bounds.Height;
            }
        }
        private void SaveWindow()
        {
            Properties.Settings.Default.WindowsPosition = this.RestoreBounds;
            Properties.Settings.Default.Save();
        }

        private void SavePresentWindow()
        {
            Properties.Settings.Default.PresentWindowsPosition = this.RestoreBounds;
            Properties.Settings.Default.Save();
        }

        private void SaveRemindWindow()
        {
            if (rw != null)
            {
                Properties.Settings.Default.RemindWindow = rw.RestoreBounds;
                Properties.Settings.Default.Save();
            }
        }


        private bool _fCanClose = false;

        public bool CanClose
        {
            get { return _fCanClose; }
            set { _fCanClose = value; }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            base.OnClosing(e);
            if (!CanClose)
            {
                e.Cancel = true;
                CurrentWindowState = this.WindowState;
                Hide();
            }
            else
            {
                TrayIcon.Visible = false;
                
            }
        }


        private void CloseSettingWindow()
        {
            if(sw != null)
            {
                sw.Close();
                sw = null;
            }
        }

        private void CloseRemindWindow()
        {
            if (rw != null)
            {
                SaveRemindWindow();
                rw.CanClose = true;
                rw.Close();
                rw.Dispose();
                rw = null;
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            Close();
        }

        private void DXWindow_Initialized(object sender, EventArgs e)
        {
            TaskControl.GetInstance().SetMissedRemindedCollection();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }


        private void About_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new DXWindow(){Content = new AboutView(), Height = 400, Width = 400, ResizeMode = ResizeMode.NoResize, Title = "О программе", WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen};
            wnd.ShowDialog();
        }

        private void DXWindow_Closed(object sender, EventArgs e)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
                if (!Equals(window, System.Windows.Application.Current.MainWindow))
                { 
                    if (window is RemindWindow remindWindow)
                        remindWindow.CanClose = true;
                    window.Close();
                }
        }

        private void DXWindow_Closing(object sender, EventArgs eventArgs)
        {
            SavePresentWindow();
            SaveRemindWindow();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                mutex?.Dispose();
                TrayIcon?.Dispose();
            }
        }
    }
}
