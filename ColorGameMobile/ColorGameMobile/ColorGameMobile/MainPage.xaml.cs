﻿using ColorGameMobile.Pages;
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
            Application.Current.UserAppTheme = OSAppTheme.Light;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new GamePage();
        }
    }
}
