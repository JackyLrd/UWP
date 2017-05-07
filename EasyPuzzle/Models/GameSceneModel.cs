using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace EasyPuzzle.Models
{
    class GameSceneModel
    {
        private bool _isCompleted;
        public bool isCompleted
        {
            get { return _isCompleted;  }
            set
            {
                _isCompleted = value;
            }
        }

        private BitmapImage _SelectedImageSource;
        public BitmapImage SelectedImageSource
        {
            get { return _SelectedImageSource; }
            set
            {
                _SelectedImageSource = value;
            }
        }

        public GameSceneModel()
        {

        }
    }
}
