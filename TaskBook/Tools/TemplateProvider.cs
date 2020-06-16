using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Core;
using TaskBook.Data;

namespace TaskBook.Tools
{
    class TemplateProvider
    {

        public TemplateProvider()
        {
            try
            {
                var masterDir = Serializer.GetDirName() + @"\Шаблоны";
                Serializer.CreateDirIfNotExist(masterDir);
                _masterDir = masterDir;
            }
            catch (Exception)
            {
            }
            
        }

        public static string TemplateNotChoiced = "Шаблон не выбран";

        public Dictionary<string, string> LoadTemplates()
        { 
            try
            {                  
              
                var files = Directory.GetFiles(_masterDir);
                var result = new Dictionary<string, string> { {TemplateNotChoiced, ""}};

                foreach (var file in files)
                    if(file.EndsWith(ex, StringComparison.Ordinal))
                        try
                        {
                            var template= File.ReadAllText(file);
                            var fileName = file.Split('\\').Last();
                            if (string.IsNullOrEmpty(template))
                                continue;
                            result.Add(fileName.Substring(0, fileName.Length - ex.Length), template);
                        }
                        catch (Exception)
                        {
                        }
                    

                return result;
            }
            catch (Exception )
            {
               return new Dictionary<string, string>();
            }
        }

        public void SaveTemplateDialog(string data)
        {
            var addTemplateWindow = new AddTemplateWindow(data);
            addTemplateWindow.ShowDialog();

            AddNewTemplate(addTemplateWindow.TemplateKey, addTemplateWindow.TemplateData);
        }

        private void AddNewTemplate(string templateKey, string template)
        {
            var fs = File.CreateText($@"{_masterDir}/{templateKey}{ex}");
            fs.Write(template);
            fs.Close();
        }

        private readonly string _masterDir = "";
        private readonly  string ex = ".tem";
    }
}
