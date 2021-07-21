using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorGameC
{
    class Program
    {
        static void Main(string[] args)
        {
            GameCore game = new GameCore();
            var ans = game.UseTry(new List<System.Windows.Media.SolidColorBrush>() { GameCore.colors[0], GameCore.colors[1], GameCore.colors[2], GameCore.colors[3], });
        }
    }
}
