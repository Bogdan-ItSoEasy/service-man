namespace TaskBook
{
    /// <summary>
    /// Interaction logic for RemindWindow.xaml
    /// </summary>
    public partial class RemindWindow
    {
        public RemindWindow()
        {
            InitializeComponent();
            Closing += RemindWindow_Closing;
            CanClose = false;
        }

        public bool CanClose { get; set; }

        private void RemindWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!CanClose)
                e.Cancel = true;

        }
    }
}
