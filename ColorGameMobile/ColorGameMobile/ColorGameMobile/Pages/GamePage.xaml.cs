using ColorGameCore;
using GameColorDesktop;
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
            else
            {
                currentColors[currentColorIndex] = (SolidColorBrush)(sender as Button).Background;
                currentColorIndex = -1;
            }
            CurrentTryLV.ItemsSource = null;
            CurrentTryLV.ItemsSource = currentColors;
        }

        private void OnGetRespone(object sender, EventArgs e)
        {
            if (currentColors.Count != 4)
                return;
            chosedColors.AddRange(currentColors);
            var a = game.GetResponse(currentColors);
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
                DisplayAlert("Победа!", $"Вы выйграли за {game.Attemps} ходов", "Начать заного");
                //List<Record> records = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records);
                //if (records == null)
                //    records = new List<Record>();
                //if (records.Any(i => i.Attemps >= game.Attemps))
                //{
                //    records.Add(new Record() { Attemps = game.Attemps, Date = DateTime.Now });
                //    Properties.Settings.Default.Records = JsonConvert.SerializeObject(records);
                //    Properties.Settings.Default.Save();
                //}
                game = new MatchGame<SolidColorBrush>(colors, GameMode.SingleColors, 10, 4);
                chosedColors = new List<SolidColorBrush>();
                answers = new List<Response>();
                return;
            }

            else
                answers.Add(a);
            UpdateView();

        }

        void UpdateView()
        {
            ResponesSL.ScrollToAsync(600, 0, false);
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