using Xamarin.Forms;

[assembly: ExportFont("SEGOEUI.ttf", Alias = "RegularFont")]
[assembly: ExportFont("SEGOEUIL.ttf", Alias = "LightFont")]
[assembly: ExportFont("SEGUISB.ttf", Alias = "MediumFont")]
namespace AlarmApp
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
