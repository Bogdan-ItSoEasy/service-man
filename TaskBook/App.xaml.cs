using System;
using System.Configuration;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.Win32;
using TaskBook.Tools;

namespace TaskBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, System.IDisposable
    {
        private void OnAppStartup_UpdateThemeName(object sender, StartupEventArgs e)
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();

            if (!mutex.WaitOne(500, false))
            {
                MessageBox.Show("Данное приложение уже запущено.");
                System.Windows.Application.Current.Shutdown();
            }

            RegProvider.SetAutoRunProperty();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            File.AppendAllText("error.log", $"Message: {e.Exception.Message}\t\nSource: {e.Exception.Source}\nStack:\n{e.Exception.StackTrace}");
            
        }

        Mutex mutex = new Mutex(false, "TaskBook");


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                mutex.Dispose();
            }
        }
    }
}
