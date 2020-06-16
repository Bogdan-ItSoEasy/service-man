using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskBook.Tools;

namespace TaskBook.Views.AdditionalWindow
{
    /// <summary>
    /// Interaction logic for RingSettings.xaml
    /// </summary>
    public partial class RingSettings : Window
    {
        public static readonly DependencyProperty IsRingOneDefaultProperty = DependencyProperty.Register("IsRingOneDefault", typeof(bool), typeof(RingSettings), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsRingTwoDefaultProperty = DependencyProperty.Register("IsRingTwoDefault", typeof(bool), typeof(RingSettings), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsRingTreeDefaultProperty = DependencyProperty.Register("IsRingTreeDefault", typeof(bool), typeof(RingSettings), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty RingOneProperty = DependencyProperty.Register("RingOne", typeof(string), typeof(RingSettings), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty RingTwoProperty = DependencyProperty.Register("RingTwo", typeof(string), typeof(RingSettings), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty RingTreeProperty = DependencyProperty.Register("RingTree", typeof(string), typeof(RingSettings), new PropertyMetadata(default(string)));

        public RingSettings()
        {
            InitializeComponent();
            var appSettings = ConfigurationManager.AppSettings;
            RingOne = appSettings["ring"] != null && !IsRingOneDefault ? appSettings["ring"] : "";
            RingTwo = appSettings["ring2"] != null && !IsRingTwoDefault? appSettings["ring2"] : "";
            RingTree = appSettings["ring3"] != null && !IsRingTreeDefault ? appSettings["ring3"] : "";

            IsRingOneDefault = string.IsNullOrEmpty(RingOne);
            IsRingTwoDefault = string.IsNullOrEmpty(RingTwo);
            IsRingTreeDefault = string.IsNullOrEmpty(RingTree);
        }


        public bool IsRingOneDefault
        {
            get { return (bool) GetValue(IsRingOneDefaultProperty); }
            set { SetValue(IsRingOneDefaultProperty, value); }
        }

        public bool IsRingTwoDefault
        {
            get { return (bool) GetValue(IsRingTwoDefaultProperty); }
            set { SetValue(IsRingTwoDefaultProperty, value); }
        }

        public bool IsRingTreeDefault
        {
            get { return (bool) GetValue(IsRingTreeDefaultProperty); }
            set { SetValue(IsRingTreeDefaultProperty, value); }
        }

        public string RingOne
        {
            get { return (string) GetValue(RingOneProperty); }
            set { SetValue(RingOneProperty, value); }
        }

        public string RingTwo
        {
            get { return (string) GetValue(RingTwoProperty); }
            set { SetValue(RingTwoProperty, value); }
        }

        public string RingTree
        {
            get { return (string) GetValue(RingTreeProperty); }
            set { SetValue(RingTreeProperty, value); }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            SettingProvider.SetSetting("ring", RingOne);

            SettingProvider.SetSetting("ring2", RingTwo);

            SettingProvider.SetSetting("ring3", RingTree);

            Close();
        }
    }
}
