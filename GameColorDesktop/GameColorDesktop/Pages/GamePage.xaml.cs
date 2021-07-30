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
using ColorGameCore;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;

namespace GameColorDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public static List<SolidColorBrush> colors = new List<SolidColorBrush>() {
            (SolidColorBrush) new BrushConverter().ConvertFromString("Red"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Green"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Blue"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Yellow"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("Black"),
            (SolidColorBrush) new BrushConverter().ConvertFromString("White"), };
        public static GameMode Mode;
        Stopwatch time;
        public List<SolidColorBrush> chosedColors;
        public MatchGame<SolidColorBrush> game;
        public List<Response> answers;
        DispatcherTimer timer;
        public void ResetGame(bool askMode = true)
        {
            if (askMode)
                Mode = MessageBox.Show("Усложнить игру", "Начало игры", MessageBoxButton.YesNo) == MessageBoxResult.Yes ? GameMode.MatchingColors : GameMode.SingleColors;
            game = new MatchGame<SolidColorBrush>(colors, Mode, 10, 4);
            chosedColors = new List<SolidColorBrush>();
            answers = new List<Response>();
            LocalTryLW.ItemsSource = null;
            ColorsSP.Children.Clear();

        }
        public GamePage()
        {
            InitializeComponent();
            ResetGame(false);
            var recs = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records);
            if (recs != null)
            {
                RecordsDGEasy.ItemsSource = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records).Where(i => i.Mode == GameMode.SingleColors);
                RecordsDGHard.ItemsSource = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records).Where(i => i.Mode == GameMode.MatchingColors);
            }
            time = new Stopwatch();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += UpdateTime;
            timer.IsEnabled = true;
            timer.Start();
            time.Start();

        }


        private void UpdateTime(object sender, EventArgs e)
        {

            TimeTB.Text = time.Elapsed.ToString().Substring(0, 8);
        }

        private void AddColor(object sender, RoutedEventArgs e)
        {
            var local = LocalTryLW.ItemsSource as List<Rectangle> == null ? new List<Rectangle>() : LocalTryLW.ItemsSource as List<Rectangle>;
            if (LocalTryLW.SelectedIndex == -1)
            {
                if (local.Count >= 4) return;
                local.Add(new Rectangle() { Fill = (sender as Button).Background, Width = 70, Height = 70, Margin = new Thickness(5) });
                LocalTryLW.ItemsSource = null;
                LocalTryLW.ItemsSource = local;
            }
            else
            {
                local[LocalTryLW.SelectedIndex].Fill = (sender as Button).Background;
                LocalTryLW.ItemsSource = null;
                LocalTryLW.ItemsSource = local;
            }
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
                        wrapPanel.Children.Add(new Rectangle() { Fill = colors[4], Width = 25, Height = 25, Margin = new Thickness(5) });
                    }
                    for (int i = 0; i < answers[answerIndex].JustColorMatching; i++)
                    {
                        wrapPanel.Children.Add(new Rectangle() { Fill = colors[5], Width = 25, Height = 25, Margin = new Thickness(5) });
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

        private void OnContinue(object sender, RoutedEventArgs e)
        {

            PauseBtn.Visibility = Visibility.Visible;
            MenuGrid.Visibility = Visibility.Collapsed;
            time.Start();
            timer.Start();
        }

        private void OnRestart(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            var complexity = MessageBox.Show("Усложнить игру", "Начало игры", MessageBoxButton.YesNo);
            ResetGame();
            RecordsDGEasy.ItemsSource = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records).Where(i => i.Mode == GameMode.SingleColors);
            RecordsDGHard.ItemsSource = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records).Where(i => i.Mode == GameMode.MatchingColors);
            time = new Stopwatch();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += UpdateTime;
            timer.IsEnabled = true;
            timer.Start();
            time.Start();
            UpdateView();
        }

        private void OnPause(object sender, RoutedEventArgs e)
        {
            PauseBtn.Visibility = Visibility.Collapsed;
            MenuGrid.Visibility = Visibility.Visible;
            time.Stop();
            timer.Stop();
        }

        private void OnGoBack(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Вы истратили {game.Attemps} ходов за {time.Elapsed}");
            NavigationService.GoBack();
        }

        private void OnGetRespone(object sender, RoutedEventArgs e)
        {
            chosedColors.AddRange((LocalTryLW.ItemsSource as List<Rectangle>).ToList().Select(i => (SolidColorBrush)i.Fill).ToList());
            var a = game.GetResponse(chosedColors.Skip(chosedColors.Count - 4).Take(4).ToList());

            if (a.ColorPositionMatching == 4)
            {
                MessageBox.Show($"Вы выйграли за {game.Attemps} ходов. Вреля : {time.Elapsed.ToString().Substring(0, 8)}");
                List<Record> records = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records);
                if (records == null)
                    records = new List<Record>();
                records.Add(new Record() { Attemps = game.Attemps, Time = time.Elapsed, Mode = Mode });
                Properties.Settings.Default.Records = JsonConvert.SerializeObject(records);
                Properties.Settings.Default.Save();
                timer.Stop();
                ResetGame();
                RecordsDGEasy.ItemsSource = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records).Where(i => i.Mode == GameMode.SingleColors);
                RecordsDGHard.ItemsSource = JsonConvert.DeserializeObject<List<Record>>(Properties.Settings.Default.Records).Where(i => i.Mode == GameMode.MatchingColors);
                timer.Start();
                return;
            }
            else
                answers.Add(a);
            UpdateView();
            if (a.JustColorMatching == -1 || game.Attemps == 10)
            {
                MessageBox.Show($"Попытки закончились. Вы истратили 10 ходов за {time.Elapsed}");
                ResetGame();
                return;
            }
        }
    }
}
