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
            UpdateView();
            if(chosedColors.Count % 4 == 0)
            {
                var a = game.UseTry(chosedColors.Skip(chosedColors.Count - 4).Take(4).ToList());
                if (a.ColorPositionMatching == 4)
                    MessageBox.Show("ezwin");
                else
                    answers.Add(a);
            }
        }

        public void UpdateView()
        {
            ColorsSP.Children.Clear();
            var localSP = new StackPanel() { Orientation = Orientation.Vertical, Width = 80, HorizontalAlignment = HorizontalAlignment.Left };
            int answerIndex = 0;
            foreach(var col in chosedColors)
            {
                if(localSP.Children.Count == 4)
                {
                    ColorsSP.Children.Add(localSP);
                    localSP = new StackPanel() { Orientation = Orientation.Vertical, Width = 80, HorizontalAlignment = HorizontalAlignment.Left };
                    WrapPanel wrapPanel = new WrapPanel() { Margin = new Thickness(5), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Width = 70, Height = 70, };
                    for (int i = 0; i < answers[answerIndex].ColorPositionMatching; i++)
                    {
                        wrapPanel.Children.Add(new Rectangle() { Fill = GameCore.colors[5], Width = 25, Height = 25, Margin = new Thickness(5) });
                    }
                    for (int i = 0; i < answers[answerIndex].ColorMatching; i++)
                    {
                        wrapPanel.Children.Add(new Rectangle() { Fill = GameCore.colors[4], Width = 25, Height = 25, Margin = new Thickness(5) });
                    }
                }
                localSP.Children.Add(new Rectangle() { Fill = col, Width = 70, Height = 70,  Margin = new Thickness(5) });
            }
            ColorsSP.Children.Add(localSP);
        }
    }
}
