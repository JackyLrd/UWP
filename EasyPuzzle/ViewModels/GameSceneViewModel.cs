using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPuzzle.ViewModels
{
    class GameSceneViewModel
    {
        private Models.GameSceneModel _SelecetedPic = default(Models.GameSceneModel);
        public Models.GameSceneModel SelectedPic
        {
            get { return _SelecetedPic; }
            set
            {
                _SelecetedPic = value;
            }
        }

        public GameSceneViewModel()
        {

        }
    }
}
