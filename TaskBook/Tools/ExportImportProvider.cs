using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using TaskBook.Data;
using Task = TaskBook.Data.Task;

namespace TaskBook.Tools
{
    static class ExportImportProvider
    {
        public static void ExportXls(string exportFileName)
        {
            try
            {
                var TC = TaskControl.GetInstance();
                Microsoft.Office.Interop.Excel.Application appExcel =
                    new Microsoft.Office.Interop.Excel.Application {ScreenUpdating = false};
                var workbook = appExcel.Workbooks.Add();
                Worksheet worksheet = workbook.Sheets.Add();
                WriteHeader(worksheet);
                for (int i = 0; i < TC.AllTasks.Count; ++i)
                {
                    if (TC.AllTasks[i] is BirthTask birthTask)
                        ExportBirthTask(worksheet, i + 2, birthTask);
                    else
                        ExportCommonTask(worksheet, i + 2, TC.AllTasks[i]);
                }

                workbook.SaveAs(exportFileName);
                appExcel.Workbooks.Close();
                appExcel.Quit();
                MessageBox.Show(@"Экспорт выполнен");
            }
            catch (Exception)
            {
                MessageBox.Show(
                    $@"Не удалось экспортировать данные в файл {
                            exportFileName
                        }. Возможно Microsoft Excel не установлен на данном компьютере или установлен некорректно. Воспользуйтель форматом .xml.");
            }
        }

        private static void WriteHeader(Worksheet worksheet)
        {
            worksheet.Cells.Item[1, 1] = @"Тип задачи";
            worksheet.Cells.Item[1, 2] = "Дата напоминания";
            worksheet.Cells.Item[1, 3] = "Время напоминания";
            worksheet.Cells.Item[1, 4] = "Обычная задача: текст задачи /\nДр: фамилия";
            worksheet.Cells.Item[1, 5] = "Обычная задача: пусто /\nДр: имя";
            worksheet.Cells.Item[1, 6] = "Обычная задача: пусто /\nДр: отчество";
            worksheet.Cells.Item[1, 7] = "Обычная задача: пусто /\nДр: комментарий";
            worksheet.Cells.Item[1, 8] = "Обычная задача: время повторения /\nДр: пусто";
            worksheet.Cells.Item[1, 9] = "Обычная задача: важность задачи /\nДр: пусто";
            worksheet.Cells.Item[1, 10] = "'+' - задача выполнена /\n '-' - задача не выполнена";
            worksheet.Cells.Item[1, 11] = "'+' - задача удалена /\n '-' - задача не удалена";
            /*worksheet.Cells.set_Item(1, 5, birthTask.Name);
            worksheet.Cells.set_Item(1, 6, birthTask.Farthername);
            worksheet.Cells.set_Item(1, 7, birthTask.Comment);
            worksheet.Cells.set_Item(1, 10, (birthTask.IsDone) ? "+" : "-");
            worksheet.Cells.set_Item(1, 11, (birthTask.IsTrash) ? "+" : "-");*/
        }


        public static void ExportXml(string exportFileName)
        {
            try
            {
                var saveData = TaskControl.GetInstance().AllTasks;
                var fs = File.Open(exportFileName, FileMode.Create, FileAccess.Write);
                Serializer.SaveFile(fs, saveData);
                MessageBox.Show(@"Экспорт выполнен");
            }
            catch (Exception)
            {
                MessageBox.Show($@"Не удалось экспортировать данные в файл {exportFileName}.");
            }
        }

        public static void ImportFromXls(string fileName)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application appExcel = new Microsoft.Office.Interop.Excel.Application();

                var workbook = appExcel.Workbooks.Open(fileName);
                foreach (Worksheet worksheet in workbook.Sheets)
                    for (int i = 1; i <= worksheet.UsedRange.Rows.Count; ++i)
                    {
                        if (worksheet.Cells[i, "A"].Text == "др" && AddImportBirthTask(worksheet.Cells[i, "B"].Text,
                                worksheet.Cells[i, "C"].Text,
                                worksheet.Cells[i, "D"].Text, worksheet.Cells[i, "E"].Text, worksheet.Cells[i, "F"].Text,
                                worksheet.Cells[i, "G"].Text, worksheet.Cells[i, "J"].Text, worksheet.Cells[i, "K"].Text))
                        {
                        }

                        if (worksheet.Cells[i, "A"].Text == "з" && AddImportCommonTask(worksheet.Cells[i, "B"].Text,
                                worksheet.Cells[i, "C"].Text, worksheet.Cells[i, "D"].Text,
                                worksheet.Cells[i, "H"].Text, worksheet.Cells[i, "I"].Text,
                                worksheet.Cells[i, "J"].Text, worksheet.Cells[i, "K"].Text))
                        {
                        }
                    }
                appExcel.Workbooks.Close();
                appExcel.Quit();
                MessageBox.Show($@"Импорт успешно выполнен.");
                
            }
            catch (Exception )
            {
                MessageBox.Show($@"Не удалось импортировать данные из файла {fileName}. Возможно Microsoft Excel не установлен на данном компьютере или установлен некорректно.");
            }
        }
        //TODO: REWORK FUNCTION
        public static void ImportFromXml(string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(fileName, FileMode.Open);
                Serializer.LoadXml(fs, out var loadTask);

                var tc = TaskControl.GetInstance();
                var importCount = 0;
                foreach (var task in loadTask)
                {
                    if (task is CommonTask t && tc.IsNewCommonTask(t) && tc.AddTask(t))
                        ++importCount;

                    if (task is BirthTask b && tc.IsNewBirthTask(b) && tc.AddTask(b))
                        ++importCount;
                }

                MessageBox.Show($@"Импорт успешно выполнен.");
            }
            catch (Exception)
            {
                MessageBox.Show($@"Не удалось импортировать данные из файла {fileName}.");
                
                fs?.Close();
                return;
            }

            fs.Close();
        }
        //TODO: REWORK FUNCTIONw

        private static void ExportBirthTask(Worksheet worksheet, int row, BirthTask birthTask)
        {
            worksheet.Cells.Item[row, 1]= @"др";
            worksheet.Cells.Item[row, 2]= $"{Dtos(birthTask.RemindDate.Day)}.{Dtos(birthTask.RemindDate.Month)}.{birthTask.RemindDate.Year}";
            worksheet.Cells.Item[row, 3] = birthTask.TaskTime.ToString("t", new CultureInfo("ru-RU"));
            worksheet.Cells.set_Item(row, 4, birthTask.Surname);
            worksheet.Cells.set_Item(row, 5, birthTask.Name);
            worksheet.Cells.set_Item(row, 6, birthTask.Farthername);
            worksheet.Cells.set_Item(row, 7, birthTask.Comment);
            worksheet.Cells.set_Item(row, 10, (birthTask.IsDone) ? "+" : "-");
            worksheet.Cells.set_Item(row, 11, (birthTask.IsTrash) ? "+" : "-");
        }

        private static void ExportCommonTask(Worksheet worksheet, int row, Task task)
        {
            worksheet.Cells.set_Item(row, 1, @"з");
            worksheet.Cells.set_Item(row, 2,$"{Dtos(task.TaskDate.Day)}.{Dtos(task.TaskDate.Month)}.{task.TaskDate.Year}");
            worksheet.Cells.set_Item(row, 3, task.TaskTime.TimeOfDay.ToString());
            worksheet.Cells.set_Item(row, 4, task.TaskInfo);
            worksheet.Cells.set_Item(row, 8, task.WhenRepeat.Name == "По дням недели"? task.RemindedWeekDays.ToString() :task.WhenRepeat.Name);
            worksheet.Cells.set_Item(row, 9, task.WhatImportant.Name);
            worksheet.Cells.set_Item(row, 10, task.IsDone ? "+" : "-");
            worksheet.Cells.set_Item(row, 11, task.IsTrash ? "+" : "-");
        }

        private static string Dtos(int num)
        {
            return num < 10 ? "0" + num : num.ToString(new CultureInfo("ru-RU"));
        }
        //TODO: REWORK FUNCTION
        private static bool AddImportCommonTask(string sDate, string sTime, string taskInfo, string period, string important, string isDoneStr, string isTrashStr)
        {
            try
            {
                if (string.IsNullOrEmpty(taskInfo))
                    return false;

                var dateStr = sDate.Split('.');
                if (dateStr.Length != 3)
                    return false;

                var date = new DateTime(Int32.Parse(dateStr[2], new CultureInfo("ru-RU")), Int32.Parse(dateStr[1], new CultureInfo("ru-RU")), 
                    Int32.Parse(dateStr[0], new CultureInfo("ru-RU")));
  
                var timeStr = sTime.Split(':');

                var time = new DateTime(1, 1, 1, Int32.Parse(timeStr.Any() ? timeStr[0] : "9", new CultureInfo("ru-RU")), 
                    Int32.Parse((timeStr.Length > 1) ? timeStr[1] : "9", new CultureInfo("ru-RU")), 0);
                

                int repeaterId = -1;
                foreach (var repeater in Task.RepeatersInfo)
                {
                    if (RepeateKeys[repeater.Name].Contains(period))
                        repeaterId = repeater.Id;
                }

                WeekDays weekDays = default;
                if (repeaterId == -1)
                {
                    repeaterId = 6;
                    _ = Enum.TryParse(period, out weekDays);
                }
                
                


                int importantId = 0;
                foreach (var importantInfo in Task.ImportantsInfo)
                {
                    if (ImportantKeys[importantInfo.Name].Contains(important))
                        importantId = importantInfo.Id;
                }


                bool isDone = isDoneStr == "+";
                bool isTrash = isTrashStr == "+";

                var task = new CommonTask()
                {
                    TaskInfo = taskInfo,
                    TaskDate = date,
                    TaskTime = time,
                    RemindDate = date,
                    RemindTime = time,
                    RepeaterId = repeaterId,
                    ImportantId = importantId,
                    RemindedWeekDays = weekDays,
                    IsDone = repeaterId != 0 && date.Date < DateTime.Today || isDone,
                    IsTrash = isTrash
                };
                task.UpdateTime();

                var tc = TaskControl.GetInstance();
                if (tc.IsNewCommonTask(task))
                    return tc.AddTask(task);
                return false;


            }
            catch (Exception )
            {
                return false;

            }
        }

        //TODO: REWORK
        private static bool AddImportBirthTask(string date, string sTime, string surname, string name, string farthername, string comment,  string isDoneStr, string isTrashStr)
        {
            
            if (string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(farthername))
                return false;


            var dateStr = date.Split('.');
            if (dateStr.Length != 3)
                return false;
            DateTime dateTime;
            try
            {
                dateTime = new DateTime(Int32.Parse(dateStr[2], new CultureInfo("ru-RU")), int.Parse(dateStr[1], new CultureInfo("ru-RU")), 
                    Int32.Parse(dateStr[0], new CultureInfo("ru-RU")));
            }
            catch
            {
                return false;
            }

            DateTime taskTime;
            try
            {
                var timeStr = sTime.Split(':');

                taskTime = new DateTime(1, 1, 1, Int32.Parse(timeStr.Any() ? timeStr[0] : "9", new CultureInfo("ru-RU")),
                    Int32.Parse((timeStr.Length > 1) ? timeStr[1] : "9", new CultureInfo("ru-RU")), 0);
            }
            catch
            {
                taskTime = new DateTime(1, 1, 1, 9, 0, 0, DateTimeKind.Local);
            }
     

            bool isDone = isDoneStr == "+";
            bool isTrash = isTrashStr == "+";

            var birthTask = new BirthTask()
            {
                Surname = surname,
                Name = name,
                Farthername = farthername,
                TaskDate = dateTime,
                RemindDate = dateTime,
                TaskTime = taskTime,
                RemindTime = taskTime,
                IsDone = dateTime.Date.DayOfYear != DateTime.Today.DayOfYear || isDone,
                IsTrash = isTrash,
                Comment = comment,
            };

            birthTask.UpdateTime();
            var tc = TaskControl.GetInstance();
            if (tc.IsNewBirthTask(birthTask))
            {
                return tc.AddTask(birthTask);
            }
            return false;
        }


        private static readonly Dictionary<string, List<string>> RepeateKeys = new Dictionary<string, List<string>>
        {
            {"Не повторять", new List<string>() { "Не повторять", "не повторять"}},
            {"Ежедневно", new List<string>() { "Ежедневно", "ежедневно", "д", "Д", "d", "D", "день"}},
            {"Еженедельно", new List<string>() { "Еженедельно", "еженедельно", "нед", "7", "7д"}},
            {"Ежемесячно", new List<string>() { "Ежемесячно", "ежемесячно", "мес", "м", "m","М", "M"}},
            {"Ежеквартально", new List<string>() { "Ежеквартально", "ежеквартально", "3м", "3m", "3М", "3M", "кв", "кварт", "квартал"}},
            {"Ежегодно", new List<string>() { "Ежегодно", "ежегодно", "365", "y", "г", "Y", "Г", "год", "Год"}},
            {"По дням недели", new List<string>() {"По дням недели", "по дням недели", "пдн"} }
        };


        private static readonly Dictionary<string, List<string>> ImportantKeys = new Dictionary<string, List<string>>
        {
            {"Обычная", new List<string>() { "Обычная", "обычная"}},
            {"Важная", new List<string>() { "Важная", "важная", "в", "В"}},
            {"Очень важная", new List<string>() { "Очень важная", "очень важная", "оч", "Очень", "очень"}},
            {"Особо важная", new List<string>() { "Особо важная", "особо важная", "ос", "Особо", "особо"}}
        };

    }

}
