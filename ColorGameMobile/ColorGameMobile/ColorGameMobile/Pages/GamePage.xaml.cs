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
        public static List<SolidColorBrush> colors = new List<SolidColorBrush>() {
            (SolidColorBrush) new BrushConverter().ConvertFromString("Red"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Green"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Blue"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Yellow"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Black"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("White"), };
        List<SolidColorBrush> colors;
        public List<SolidColorBrush> chosedColors;
        public MatchGame<SolidColorBrush> game;
        public List<Response> answers;
        public GamePage()
        {
            InitializeComponent();
            colors = (List<SolidColorBrush>)ButtonsSL.Children.Select(i => (i as Button).Background);
            game = new MatchGame<SolidColorBrush>(colors, GameMode.SingleColors, 10, 4);
            chosedColors = new List<SolidColorBrush>();
            answers = List<Response>();
        }
        private void AddColor(object sender, EventArgs e)
        {
            chosedColors.Add((SolidColorBrush)(sender as Button).Background);
            if (chosedColors.Count % 4 == 0)
            {
                var a = game.GetResponse(chosedColors.Skip(chosedColors.Count - 4).Take(4).ToList());
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
                    DisplayAlert("Победа!", $"Вы выйграли за {game.Attemps} ходов" , "");
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
            }
        }
    }
}