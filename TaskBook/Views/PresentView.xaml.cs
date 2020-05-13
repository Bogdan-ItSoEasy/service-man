using System;
using System.IO;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskBook.Data;
using TaskBook.Tools;
using TaskBook.ViewModels;

namespace TaskBook.Views
{
    /// <summary>
    /// Interaction logic for PresentView.xaml
    /// </summary>
    public partial class PresentView : UserControl
    {
        public PresentView()
        {
            InitializeComponent();
            }

        private void SettingView_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;
            window?.GetSettingWindow();
            
        }

        private void TrashView_Click(object sender, RoutedEventArgs e)
        {
            if (tw == null)
            {
                tw = new DXWindow() { Content = new TrashView(), Title = "Удаленные задачи", MinHeight = 400, MinWidth = 430 };
                tw.Closing += (s, en) =>
                {
                    Properties.Settings.Default.TrashWindowPosition = tw.RestoreBounds;
                    Properties.Settings.Default.Save();
                    tw = null;
                };
                RestoreWindow(Properties.Settings.Default.TrashWindowPosition, tw,500,500);
                tw.Show();
            }
            else
            {
                MessageBox.Show("Данное окно уже открыто!");
                tw?.Activate();
            }
        }
        private void DoneListView_Click(object sender, RoutedEventArgs e)
        {
            if (dlw == null)
            {
                dlw = new DXWindow() { Content = new HistoryView(), Title = "Список выполненных задач", MinHeight = 400, MinWidth = 430 };
                dlw.Closing += (s, en) =>
                {
                    Properties.Settings.Default.DoneWindowPosition = dlw.RestoreBounds;
                    Properties.Settings.Default.Save();
                    dlw = null;
                };
                RestoreWindow(Properties.Settings.Default.HistoryListWindowPosition, dlw, 500, 500);
                dlw.Show();
            }
            else
            {
                MessageBox.Show("Данное окно уже открыто!");
            }
        }

        private void DoneView_Click(object sender, RoutedEventArgs e)
        {
            if(dw == null)
            {
                dw = new DXWindow() { Content = new DoneView(), Title = "Выполненные задачи", MinHeight = 400, MinWidth = 430};
                dw.Closing += (s, en) =>
                {
                    Properties.Settings.Default.DoneWindowPosition = dw.RestoreBounds;
                    Properties.Settings.Default.Save();
                    dw = null;
                };
                RestoreWindow(Properties.Settings.Default.DoneWindowPosition, dw, 500, 500);
                dw.Show();
            }
            else
            {
                MessageBox.Show("Данное окно уже открыто!");
                dw?.Activate();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (aw == null)
            {
                aw = new DXWindow() { Content = new AddControlView(), Title = "Добавление", MinHeight = 600, MaxWidth = 500, MinWidth= 500 };
 

                (aw.Content as AddControlView).RequestClose += (s, en) =>
                {       
                    aw.Close();
                };
                aw.Closing += (s, en) =>
                {
                    Properties.Settings.Default.AddWindowPosition = aw.RestoreBounds;
                    Properties.Settings.Default.Save();
                    aw = null;
                };
                RestoreWindow(Properties.Settings.Default.AddWindowPosition, aw, 500);
                aw.Show();
            }
            else
            {
                MessageBox.Show("Данное окно уже открыто!");
                aw?.Activate();
            }

        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
             
                if (aw == null)
                {
                    var obj = (sender as StackPanel)?.DataContext;

                    if (obj is BirthTask)
                    {
                        aw = new DXWindow() { Content = new AddBirthCtrlView(obj) { ButtonContent = "Изменение" }, Title = "Изменение", MinHeight = 600, MaxWidth = 500, MinWidth = 500 };
                        ((aw.Content as AddBirthCtrlView).DataContext as AddBirthCtrlViewModel).RequestClose += (s, en) => aw.Close();
                        RestoreWindow(Properties.Settings.Default.AddWindowPosition, aw,500);                      
                    }
                    else
                    {
                        CommonTask task = obj as CommonTask;
                        if (task == null)
                            return;

                        var context = new AddComCtrlViewModel()
                        {
                            Data = task.TaskInfo,
                            DataTime = task.TaskTime,
                            DataDate = task.TaskDate,
                            RepeateId = task.RepeaterId,
                            ImportantId = task.ImportantId,
                            EditingTask = task,
                            Hour = task.TaskTime.Hour,
                            Min = task.TaskTime.Minute,
                            RemindedWeekDays = task.RemindedWeekDays
                        };

                        aw = new DXWindow() { Content = new AddComCtrlView() { ButtonContent = "Изменение", DataContext = context }, Title = "Изменение", MinHeight = 600, MaxWidth = 500, MinWidth = 500 };
                        ((aw.Content as AddComCtrlView).DataContext as AddComCtrlViewModel).RequestClose += (s, en) => aw.Close();
                        RestoreWindow(Properties.Settings.Default.AddWindowPosition, aw,500);              
                    }


                    aw.Closing += (s, en) =>
                    {
                        Properties.Settings.Default.AddWindowPosition = aw.RestoreBounds;
                        Properties.Settings.Default.Save();
                        (DataContext as PresentViewModel)?.Refresh();
                        aw = null;
                    };

                    aw.Show();
                    Window.GetWindow(this).Activated += PresentView_Activated;
                }
                else
                {
                    MessageBox.Show("Данное окно уже открыто!");
                    aw?.Activate();
                    Window.GetWindow(this).Activated += PresentView_Activated;
                }
            }
        }

        private void PresentView_Activated(object sender, System.EventArgs e)
        {
            Window.GetWindow(this).Activated -= PresentView_Activated;
            aw?.Activate();
        }

        public static void RestoreWindow(System.Windows.Rect setting, Window window, int stdHeight=0, int stdWidth=0, bool useStd=false)
        {
   
            try
            {
                Rect bounds = setting;

                if(bounds.Equals(new Rect(0,0,0,0)) || useStd)
                {
                    window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    window.Height = (stdHeight != 0) ? stdHeight : window.Height;
                    window.Width = (stdWidth != 0) ? stdWidth : window.Width;

                    return;
                }

                window.Top = bounds.Top;
                window.Left = bounds.Left;

                if (window.SizeToContent == SizeToContent.Manual)
                {
                    window.Width = bounds.Width;
                    window.Height = bounds.Height;
                }
            }
            catch
            { 
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                window.Height = (stdHeight != 0) ? stdHeight : window.Height;
                window.Width = (stdWidth != 0) ? stdWidth : window.Width;
            }

        }

        Window aw;
        Window dw;
        Window tw;
        Window sw;
        Window dlw;

        private void CheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
