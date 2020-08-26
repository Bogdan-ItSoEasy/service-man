using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Xml;

namespace TaskBook.Data
{
    public class Serializer : ISaver
    {
        public static void CreateDirIfNotExist(string dirName)
        {
            string dirPath = Path.Combine(dirName);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }

        public static string GetDirName()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var fileName = appSettings["dir"] ?? "";
            if (string.IsNullOrEmpty(fileName) || !Directory.Exists(Path.GetFullPath(fileName)))
                fileName = GetDefaultPath();

            return fileName;
        }

        public static string GetDefaultPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),  "Слуга");
        }

        public static string GetHomePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public ObservableCollection<Task> Load()
        {
            var dirName = GetDirName();
            ObservableCollection<Task> loadData = null;
            CreateDirIfNotExist(dirName);
            string fileName = Path.Combine(dirName, "task.data");

            if (!File.Exists(fileName))
            {
                return new ObservableCollection<Task>();
            }

            FileStream fs = null;
            try
            {
                fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                LoadXml(fs, out loadData);
            }
            catch (FileNotFoundException)
            {
                fs?.Close();
                MessageBox.Show("Не удалось найти файл с задачами");
            }
            catch (SerializationException)
            {
                fs?.Close();
                int i = 0;
                while (File.Exists(fileName + i))
                    ++i;
                var newFileName = fileName + i;
                File.Copy(fileName, newFileName);
                
                MessageBox.Show($"Не удалось загрузить файл задач. Содержимое файла сохранено в файле: {newFileName}.");
                return new ObservableCollection<Task>();
            }

            fs?.Close();
            return loadData ?? new ObservableCollection<Task>();
        }


        public static void LoadXml(Stream fs, out ObservableCollection<Task> loadData)
        {
            
            using (XmlDictionaryReader xdr =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(ObservableCollection<Task>),
                    new[] { typeof(CommonTask), typeof(BirthTask), typeof(WeekDays) });

                loadData = (ObservableCollection<Task>)dcs.ReadObject(xdr);
            }
        }

        public bool Save(ObservableCollection<Task> saveData)
        {
            var dirName = GetDirName();
            CreateDirIfNotExist(dirName);
            FileStream fs = null;
            try
            {
                string fileName = Path.Combine(dirName, "task.data");
                fs = File.Open(fileName, FileMode.Create, FileAccess.Write);
                SaveFile(fs, saveData);
                
            }
            catch (IOException)
            {
                fs?.Close();
            }
            catch(SerializationException)
            {
                fs?.Close();
            }

            return true;
        }

        public static void SaveFile(FileStream fs, ObservableCollection<Task> saveData)
        {
            using (XmlDictionaryWriter xdw =
                XmlDictionaryWriter.CreateTextWriter(fs, Encoding.UTF8))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(ObservableCollection<Task>), new[] { typeof(CommonTask), typeof(BirthTask), typeof(WeekDays)});
                dcs.WriteObject(xdw, saveData);
            }
        }
    }
}