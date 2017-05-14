using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace EasyPuzzle.Models
{
    class PuzzleFragment : INotifyPropertyChanged
    {
        private int index;
        private int curIndex;
        private Image img;
        private ImageSource src;

        public ImageSource Src
        {
            get
            {
                return src;
            }
            set
            {
                src = value;
                OnPropertyChanged();
            }
        }

        public Image Img
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                OnPropertyChanged();
            }
        }

        public int CurIndex
        {
            get
            {
                return curIndex;
            }
            set
            {
                curIndex = value;
                OnPropertyChanged();
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PuzzleFragment(PuzzleFragment x)
        {
            clonefrom(x);
        }

        public void clonefrom(PuzzleFragment x)
        {
            this.Img = x.Img;
            this.CurIndex = x.CurIndex;
            this.Src = x.Src;
            this.Index = x.Index;
        }

        public PuzzleFragment()
        {
        }
    }
}
