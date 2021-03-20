namespace AlarmApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new AlarmApp.App());
        }
    }
}
