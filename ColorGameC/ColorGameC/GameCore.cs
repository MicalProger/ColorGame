﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorGameC
{
    public class Answer
    {
        public int ColorMatching;
        public int ColorPositionMatching;

        public Answer(int colorMatching, int positionMatching)
        {
            ColorMatching = colorMatching;
            ColorPositionMatching = positionMatching;
        }
    }

    public class GameCore
    {
        public static List<SolidColorBrush> colors = new List<SolidColorBrush>() { new SolidColorBrush(Color.FromRgb(128, 0, 0)),
            new SolidColorBrush(Color.FromRgb(0, 255, 0)),
            new SolidColorBrush(Color.FromRgb(0, 0, 255)),
            new SolidColorBrush(Color.FromRgb(255, 255, 0)),
            new SolidColorBrush(Color.FromRgb(0, 0, 0)),
            new SolidColorBrush(Color.FromRgb(255, 255, 255)) };
        List<SolidColorBrush> row;
        int attempts;
        public GameCore()
        {
            Random rnd = new Random();
            row = new List<SolidColorBrush>();
            for (int i = 0; i < 4; i++)
            {
                var r = rnd.Next(0, 5);
                while (row.IndexOf(colors[r]) != -1)
                    r = rnd.Next(0, 5);
                row.Add(colors[r]);
            }
        }

        public Answer UseTry(List<SolidColorBrush> colcors)
        {
            if (attempts < 9)
                attempts++;
            else
                return null;
            Answer ans = new Answer(0, 0);
            ans.ColorMatching = row.Where(i => colcors.Contains(i) && colcors.IndexOf(i) != row.IndexOf(i)).Count();
            ans.ColorPositionMatching = row.Where(i => colcors.Contains(i) && colcors.IndexOf(i) == row.IndexOf(i)).Count();
            return ans;
        }
    }
}
