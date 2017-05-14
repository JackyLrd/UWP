using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using EasyPuzzle.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace EasyPuzzle.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GameScene : Page
    {
        private int dimension;
        private int[] initialOrder;
        private PuzzleFragment SelectedFragment;
        DispatcherTimer dispatchTimer;
        int t;

        private selectedImage img;

        ViewModels.GameSceneViewModel ViewModel { get; set; }
        public GameScene()
        {
            this.InitializeComponent();
            this.SelectedFragment = null;
            this.img = new selectedImage();
            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatchTimer.Tick += dispatchTimer_Tick;
            t = 0;
            nameBlock.Visibility = name.Visibility = recordBlock.Visibility = record.Visibility = submit.Visibility = Visibility.Collapsed;
        }

        void dispatchTimer_Tick(object sender, object e)
        {
            t++;
            time.Text = "时间: " + t;
        }

        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 600; //match the target Image.Width, not shown
                    await bitmapImage.SetSourceAsync(fileStream);
                    this.img.SRC = bitmapImage;
                    //设置图片路径（小的图片的path）
                    string src = this.img.SRC.ToString();
                    int position = src.IndexOf('.');
                    string path = src.Substring(0, position) + "-" + dimension * dimension + "-";
                    cutImage(path, src.Substring(position));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.dimension = (int)e.Parameter;
            this.ViewModel = new ViewModels.GameSceneViewModel(this.dimension);
            this.initialOrder = new int[dimension * dimension];
        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.SelectedFragment == null)
            {
                this.SelectedFragment = (Models.PuzzleFragment)(e.ClickedItem);
            }
            else
            {
                PuzzleFragment SecondFragment = (Models.PuzzleFragment)(e.ClickedItem);
                ViewModel.swap(this.SelectedFragment, SecondFragment);
                if (ViewModel.checkfinished() == true)
                {
                    dispatchTimer.Stop();
                    nameBlock.Visibility = name.Visibility = recordBlock.Visibility = record.Visibility = submit.Visibility = Visibility.Visible;
                }
                this.SelectedFragment = null;
            }
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            if (dispatchTimer.IsEnabled)
            {
                dispatchTimer.Stop();
                t = 0;
                time.Text = "时间: 0";
            }
            string index = ((Button)e.OriginalSource).Name;
            switch (index)
            {
                case "one":
                    img.SRC = img1.Source;
                    break;
                case "two":
                    img.SRC = img2.Source;
                    break;
                case "three":
                    img.SRC = img3.Source;
                    break;
            }
            //
            //string src = img.SRC.ToString();
            //int position = src.IndexOf('.');
            string path = img.SRC.ToString();//src.Substring(0, position) + "-" + dimension * dimension + "-";
            cutImage(path, "");//src.Substring(position));
        }

        private void cutImage(string path, string postfix)
        {
            for (int i = 0; i < dimension * dimension; i++)
            {
                initialOrder[i] = i;
            }
            for (int i = 0; i < 50; i++)
            {
                Random random = new Random();
                int a = random.Next() % (dimension * dimension);
                int b = random.Next() % (dimension * dimension);
                int temp = initialOrder[a];
                initialOrder[a] = initialOrder[b];
                initialOrder[b] = temp;
            }

            for (int i = 0; i < dimension * dimension; i++)
            {
                string str = "ms:appx//" + path;// + initialOrder[i] + postfix;//碎片的path
                var a = new Models.PuzzleFragment();
                var img = new Windows.UI.Xaml.Controls.Image();
                img.Source = new BitmapImage(new Uri(str));
                img.Width = img.Height = 100;
                a.Index = initialOrder[i];
                a.Img = img;
                a.Src = img.Source;
                this.ViewModel.addFragment(a);
            }
            dispatchTimer.Start();
        }

        public class selectedImage : INotifyPropertyChanged
        {
            private ImageSource src;

            public selectedImage()
            {
                src = null;
            }

            public ImageSource SRC
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

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged([CallerMemberName]string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
