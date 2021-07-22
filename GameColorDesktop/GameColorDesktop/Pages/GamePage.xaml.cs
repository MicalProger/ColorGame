using ColorGameCore;
using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameColorDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public List<SolidColorBrush> chosedColors;
        public GameCore game;
        public List<Answer> answers;
        public GamePage()
        {
            InitializeComponent();
            game = new GameCore();
            chosedColors = new List<SolidColorBrush>();
            answers = new List<Answer>();
        }

        private void AddColor(object sender, RoutedEventArgs e)
        {
            chosedColors.Add((SolidColorBrush)(sender as Button).Background);
            if (chosedColors.Count % 4 == 0)
            {
                var a = game.UseTry(chosedColors.Skip(chosedColors.Count - 4).Take(4).ToList());
                if (a == null)
                {
                    MessageBox.Show("Попытки закончились.");
                    game = new GameCore();
                    chosedColors = new List<SolidColorBrush>();
                    answers = new List<Answer>();
                    return;
                }
                if (a.ColorPositionMatching == 4)
                {
                    MessageBox.Show($"Вы выйграли за {game.Attemps} ходов");
                    List<Record> records = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records);
                    if (records == null)
                        records = new List<Record>();
                    if (records.Any(i => i.Attemps >= game.Attemps))
                    {
                        records.Add(new Record() { Attemps = game.Attemps, Date = DateTime.Now });
                        Properties.Settings.Default.Records = JsonConvert.SerializeObject(records);
                        Properties.Settings.Default.Save();
                    }
                    game = new GameCore();
                    chosedColors = new List<SolidColorBrush>();
                    answers = new List<Answer>();
                    return;
                }

                else
                    answers.Add(a);
            }
            UpdateView();

        }

        public void UpdateView()
        {

            ColorsSP.Children.Clear();
            var localSP = new StackPanel() { Orientation = Orientation.Vertical, Width = 80, HorizontalAlignment = HorizontalAlignment.Left };
            int answerIndex = 0;
            foreach (var col in chosedColors)
            {
                localSP.Children.Add(new Rectangle() { Fill = col, Width = 70, Height = 70, Margin = new Thickness(5) });
                if (localSP.Children.Count == 4)
                {
                    WrapPanel wrapPanel = new WrapPanel() { Margin = new Thickness(5), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Width = 70, Height = 70, };
                    for (int i = 0; i < answers[answerIndex].ColorPositionMatching; i++)
                    {
                        wrapPanel.Children.Add(new Rectangle() { Fill = GameCore.colors[5], Width = 25, Height = 25, Margin = new Thickness(5) });
                    }
                    for (int i = 0; i < answers[answerIndex].ColorMatching; i++)
                    {
                        wrapPanel.Children.Add(new Rectangle() { Fill = GameCore.colors[4], Width = 25, Height = 25, Margin = new Thickness(5) });
                    }
                    localSP.Children.Add(wrapPanel);
                    ColorsSP.Children.Add(localSP);
                    localSP = new StackPanel() { Orientation = Orientation.Vertical, Width = 80, HorizontalAlignment = HorizontalAlignment.Left };
                    answerIndex++;
                }
            }
            if (localSP.Children.Count != 4)
                ColorsSP.Children.Add(localSP);
        }
    }
}
