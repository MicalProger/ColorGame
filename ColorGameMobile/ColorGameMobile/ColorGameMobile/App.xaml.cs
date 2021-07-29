using GameColorDesktop;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColorGameMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            Record.SaveRecords();
        }

        protected override void OnResume()
        {
        }
    }
}
