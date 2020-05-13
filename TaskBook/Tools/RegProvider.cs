using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TaskBook.Tools
{
    static class RegProvider
    {
        public static void SetAutoRunProperty()
        {
            var assembly = Assembly.GetAssembly(typeof(App));
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            string exePath = info.FileName;
            string key = info.ProductName;

            var reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\");

            var appSettings = ConfigurationManager.AppSettings;
            var isAutoRun = appSettings["autorun"] == null || bool.FalseString != appSettings["autorun"];

            if (isAutoRun)
            {
                reg?.SetValue(key, exePath);
            }
            else
            {
                if (reg?.GetValueNames().Contains(key) is true)
                {
                    reg.DeleteValue(key);
                }
            }


        }
    }
}
