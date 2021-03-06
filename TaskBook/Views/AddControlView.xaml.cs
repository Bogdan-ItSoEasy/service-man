﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TaskBook.Data;
using TaskBook.ViewModels;

namespace TaskBook.Views
{


    /// <summary>
    /// Логика взаимодействия для AddControlView.xaml
    /// </summary>
    partial class AddControlView : UserControl
    {
        

        public AddControlView()
        {

            buttonContent = "Добавить";
            CreateGridList(null);
            InitializeComponent();


            SwitchGrid(0);
            ComboBox.SelectedIndex = 0;
        }

      /*  public AddControlView(object task)
        {
            buttonContent = "Изменить";
            CreateGridList(task);

            InitializeComponent();

            var dataContext = DataContext as AddControlViewModel;
            dataContext.SetEditingTask(task);

            if (task is BirthTask)
            {
                SwitchGrid(1);
                ComboBox.SelectedIndex = 1;
            }
            else
            {
                SwitchGrid(0);
                ComboBox.SelectedIndex = 0;
            }
        }*/

        void CreateGridList(object task)
        {
            var window = Window.GetWindow(this);

            var comUc = new AddComCtrlView(task as CommonTask, new AddComCtrlViewModel() { RepeateId = 0, DataDate = DateTime.Today, DataTime = new DateTime(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0), Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute }) { ButtonContent = buttonContent };
            var birthUc = new AddBirthCtrlView(task as BirthTask) { ButtonContent = buttonContent };

            (comUc.DataContext as AddComCtrlViewModel).RequestClose += (s, e) => RequestClose.Invoke(this, EventArgs.Empty);
            (birthUc.DataContext as AddBirthCtrlViewModel).RequestClose += (s, e) => RequestClose.Invoke(this, EventArgs.Empty);

            Grids = new List<UserControl>()
            {
                comUc, birthUc
            };

            

        }
        public event EventHandler RequestClose;

        public void ChangeGridView(UserControl uc)
        {
            if (AddGrid == null)
                return;

            AddGrid.Children.RemoveRange(0, AddGrid.Children.Count);
            AddGrid.Children.Add(uc);
        }

        string buttonContent;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            if (comboBox == null)
                return;
            SwitchGrid(comboBox.SelectedIndex);
        }
        
        private void SwitchGrid(int gridNum)
        {
            switch (gridNum)
            {
                case 0:
                    ChangeGridView(Grids[0]);
                    break;
                case 1:
                    ChangeGridView(Grids[1]);
                    break;
            }
        }

        List<UserControl> Grids;
    }
}

