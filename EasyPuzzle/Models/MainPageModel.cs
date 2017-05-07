using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPuzzle.Models
{
    struct Players
    {
        string name;
        string time;
    }

    class MainPageModel
    {

        private Players[] _TopPlayers;
        public Players[] TopPlayers
        {
            get { return _TopPlayers; }
            set
            {
                _TopPlayers = value;
            }
        }

        public MainPageModel()
        {

        }
    }
}
