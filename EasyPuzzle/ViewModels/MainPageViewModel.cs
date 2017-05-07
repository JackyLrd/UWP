using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPuzzle.ViewModels
{
    class MainPageViewModel
    {
        private Models.MainPageModel _MainpageModel = default(Models.MainPageModel);
        public Models.MainPageModel MainpageModel
        {
            get { return _MainpageModel; }
            set
            {
                _MainpageModel = value;
            }
        }

        public MainPageViewModel()
        {

        }

        public Models.Players[] getTopPlayers()
        {
            return MainpageModel.TopPlayers;
        }
    }
}