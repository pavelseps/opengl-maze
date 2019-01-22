using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{
    class App
    {
        private Form1 _game;
        private Form2 _settings;
        private DateTime _startTime;
        private DateTime _endTime;

        public Form1 Game { get => _game; set => _game = value; }
        public Form2 Settings { get => _settings; set => _settings = value; }

        public void Start(string mazeUrl)
        {
            Game.Start(mazeUrl);
            Game.Show();
            Settings.Hide();
            _startTime = DateTime.Now;
        }

        public void Finished()
        {
            _endTime = DateTime.Now;
            Settings.Finished(_endTime.Subtract(_startTime));
            Game.Hide();
            Game.End();
            Settings.Show();
        }
    }
}
