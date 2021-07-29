using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorGameCore
{
    public enum GameMode
    {
        SingleColors = 1,
        MatchingColors = 2
    }

    public class Response
    {
        public int JustColorMatching { get; set; }
        public int ColorPositionMatching { get; set; }

        public Response(int justColorMatching, int colorPositionMatching)
        {
            JustColorMatching = justColorMatching;
            ColorPositionMatching = colorPositionMatching;
        }

        public override string ToString()
        {
            return $"JustColorMatching : {JustColorMatching}, ColorPositionMatching : {ColorPositionMatching}";
        }

        
    }

    public class MatchGame<T>
    {
        public Stopwatch GameTime;
        public int Attemps;
        private List<T> _hiddenValues;
        private List<T> _totalValues;
        private GameMode _mode;
        private int _maxAttempt;

        public MatchGame(List<T> values, GameMode mode, int maxAttempt, int lenght)
        {
            GameTime = new Stopwatch();
            this._totalValues = values;
            this._mode = mode;
            this._maxAttempt = maxAttempt;
            _hiddenValues = new List<T>();
            Random random = new Random();
            switch (mode)
            {
                case GameMode.SingleColors:
                    while (_hiddenValues.Count != lenght)
                    {
                        _hiddenValues.Add(values[random.Next(0, values.Count)]);
                        _hiddenValues = _hiddenValues.Distinct().ToList();
                    }

                    break;
                case GameMode.MatchingColors:
                    for (int i = 0; i < lenght; i++)
                    {
                        _hiddenValues.Add(values[random.Next(0, values.Count - 1)]);
                    }

                    break;
            }
            GameTime.Start();
        }

        public List<T> GetHidden()
        {
            return _hiddenValues;
        }

        public Response GetResponse(List<T> values)
        {
            Response answer = new Response(0, 0);
            if (Attemps < _maxAttempt)
            {
                Attemps++;
            }
            else
                return new Response(-1, -1);

            // answer.JustColorMatching =
            //     _totalValues.Count(i => values.Contains(i) && _hiddenValues.Contains(i) && _hiddenValues.IndexOf(i) != values.IndexOf(i));
            // answer.ColorPositionMatching =
            //     _totalValues.Count(i => values.Contains(i) && _hiddenValues.Contains(i) && _hiddenValues.IndexOf(i) == values.IndexOf(i));
            foreach (var value in _totalValues)
            {
                if (_hiddenValues.Contains(value) && values.Contains(value))
                {

                    if (values.Any(i => values[_hiddenValues.IndexOf(value)].Equals(value)))
                        answer.ColorPositionMatching++;
                    else
                        answer.JustColorMatching++;
                }
            }
           return answer;
        }
    }
}
