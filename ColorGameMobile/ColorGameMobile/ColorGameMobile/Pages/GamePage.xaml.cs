using ColorGameCore;
using GameColorDesktop;
using Java.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColorGameMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public static List<SolidColorBrush> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.White, Color.Black }.Select(i => new SolidColorBrush(i)).ToList();
        public List<SolidColorBrush> chosedColors;
        List<SolidColorBrush> currentColors;
        int currentColorIndex = -1;
        public MatchGame<SolidColorBrush> game;
        public List<Response> answers;
        public GamePage()
        {
            InitializeComponent();
            RestartGame();
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 500), UpdateTimer);
        }

        bool UpdateTimer()
        {
            TimerLabel.Text = game.GameTime.Elapsed.ToString().Substring(0, 8);
            return true;
        }

        public void RestartGame()
        {
            game = new MatchGame<SolidColorBrush>(colors, GameMode.SingleColors, 10, 4);
            chosedColors = new List<SolidColorBrush>();
            answers = new List<Response>();
            currentColors = new List<SolidColorBrush>();
        }
        private void AddColor(object sender, EventArgs e)
        {
            if (currentColorIndex == -1 && currentColors.Count != 4)
            {
                currentColors.Add((SolidColorBrush)(sender as Button).Background);

            }
            else if(currentColorIndex != -1)
            {
                currentColors[currentColorIndex] = (SolidColorBrush)(sender as Button).Background;
                currentColorIndex = -1;
            }
            CurrentTryLV.ItemsSource = null;
            CurrentTryLV.ItemsSource = currentColors;
        }

        private async void OnGetRespone(object sender, EventArgs e)
        {
            if (currentColors.Count != 4)
                return;
            chosedColors.AddRange(currentColors);
            var a = game.GetResponse(currentColors);
            currentColors.Clear();
            CurrentTryLV.ItemsSource = null;
            CurrentTryLV.ItemsSource = currentColors;
            if (a == null)
            {
                _ = DisplayAlert("Проигрыш!", "Попытки закончились.", "Ок");
                game = new MatchGame<SolidColorBrush>(colors, GameMode.SingleColors, 10, 4);
                chosedColors = new List<SolidColorBrush>();
                answers = new List<Response>();
                return;
            }
            if (a.ColorPositionMatching == 4)
            {
                if (Record.Records == null)
                    Record.Records = new List<Record>();
                if (!Record.Records.Any(i => i.Attemps <= game.Attemps))
                {
                    Record.Records.Add(new Record() { Attemps = game.Attemps, Time = new TimeSpan(0, 2, 0) });
                    Record.SaveRecords();
                }
                if (await DisplayAlert("Победа!", $"Вы выйграли за {game.Attemps} ходов", "На главный экран", "Начать заного"))
                {
                    await Navigation.PopAsync();
                    return;
                }
                else
                    RestartGame();

            }

            else
                answers.Add(a);
            UpdateView();

        }

        void UpdateView()
        {
            ResponesSL.ScrollToAsync(0, 600, false);
            ColorsCollector.Children.Clear();
            StackLayout layout = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(5) };
            int answerIndex = 0;
            var x = new List<View>();
            foreach (var item in chosedColors)
            {
                layout.Children.Add(new BoxView() { HeightRequest = 60, WidthRequest = 60, Background = item, Margin = new Thickness(5) });
                if (layout.Children.Count == 4)
                {
                    x = layout.Children.Reverse().ToList();
                    layout.Children.Clear();
                    x.ForEach(i => layout.Children.Add(i));
                    StackLayout answerSL = new StackLayout() { Orientation = StackOrientation.Vertical, Margin = 5 };
                    for (int i = 0; i < answers[answerIndex].ColorPositionMatching; i++)
                    {
                        answerSL.Children.Add(new BoxView() { HeightRequest = 11, WidthRequest = 60, Background = new SolidColorBrush(Color.Black), Margin = new Thickness(2) });
                    }
                    for (int i = 0; i < answers[answerIndex].JustColorMatching; i++)
                    {
                        answerSL.Children.Add(new BoxView() { HeightRequest = 11, WidthRequest = 60, Background = new SolidColorBrush(Color.White), Margin = new Thickness(2) });
                    }
                    layout.Children.Add(answerSL);
                    ColorsCollector.Children.Add(layout);
                    layout = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(5) };
                    answerIndex++;
                }
            }
            x = ColorsCollector.Children.Reverse().ToList();
            ColorsCollector.Children.Clear();
            x.ForEach(i => ColorsCollector.Children.Add(i));

        }

        private void CurrentTryLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            currentColorIndex = e.SelectedItemIndex;
        }
    }
}