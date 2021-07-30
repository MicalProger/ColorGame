using ColorGameCore;
using ColorGameMobile.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ColorGameMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                GamePage.Mode = DisplayAlert("Новая игра", "Выберите режим", "Сложный", "Простой").Result ? GameMode.MatchingColors : GameMode.SingleColors;
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new GamePage()));
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new RecordsPage()));
        }
    }
}
