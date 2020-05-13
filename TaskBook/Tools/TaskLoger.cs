using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.Logging;
using Task = TaskBook.Data.Task;

namespace TaskBook.Tools
{
    static class TaskLoger
    {
        static TaskLoger()
        {
            
        }

        public static void LogTask(Task task, DateTime now)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application appExcel = new Microsoft.Office.Interop.Excel.Application();

                var workbook = !File.Exists(LogFileName) ? appExcel.Workbooks.Add() : appExcel.Workbooks.Open(LogFileName);
                Worksheet worksheet = workbook.Sheets.Count == 0 ? workbook.Sheets.Add() : workbook.Sheets[0];

                var newRow = worksheet.UsedRange.Row + 1;
 
                worksheet.Cells[newRow, "A"] = now;
                worksheet.Cells[newRow, "B"] = task.TaskInfo;
                
                appExcel.Workbooks.Close();
                appExcel.Quit();

                int[] arr = new int[]{0,1,2};

            }
            catch (Exception)
            {

            }
        }

        private static string LogFileName => "Выполненные задачи";
    }
}
