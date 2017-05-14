﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPuzzle.Models;
using Windows.UI.Xaml.Media.Imaging;

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
        private ObservableCollection<Models.PuzzleFragment> imgs = new ObservableCollection<Models.PuzzleFragment>();
        private int count;

        public ObservableCollection<Models.PuzzleFragment> Imgs { get { return this.imgs; } }

        public GameSceneViewModel(int para)
        {
            count = 0;
            for (int i = 0; i < para * para; i++)
            {
                var a = new Models.PuzzleFragment();
                var img = new Windows.UI.Xaml.Controls.Image();
                img.Source = new BitmapImage(new Uri("ms-appx:///Assets/abc.jpg"));
                img.Width = img.Height = 100;
                a.Index = i;
                a.Img = img;
                a.Src = img.Source;
                addFragment(a);
            }
            addFragment(new PuzzleFragment());
        }

        public void addFragment(PuzzleFragment x)
        {
            imgs.Add(x);
            count++;
        }

        public void removeFragment(int index)
        {
            imgs.RemoveAt(index);
            count--;
        }

        internal void swap(PuzzleFragment selectedFragment, PuzzleFragment secondFragment)
        {
            var temp = imgs.IndexOf(selectedFragment);
            PuzzleFragment tempFragment = new PuzzleFragment(imgs[temp]);
            var temp2 = imgs.IndexOf(secondFragment);
            imgs[temp].clonefrom(imgs[temp2]);
            imgs[temp2].clonefrom(tempFragment);
            imgs[temp].CurIndex = temp;
            imgs[temp2].CurIndex = temp2;
        }

        internal bool checkfinished()
        {
            int correct = 0;
            for (int i = 0; i < count; i++)
            {
                if (imgs[i].Index == imgs[i].CurIndex)
                {
                    correct++;
                }
            }
            return correct == count;
        }
    }
}
