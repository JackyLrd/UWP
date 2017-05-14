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
using SQLitePCL;
using System.Text;

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

        private string str1;
        private string str2;
        private string str3;
        private string str4;

        ViewModels.GameSceneViewModel ViewModel { get; set; }
        public GameScene()
        {
            this.InitializeComponent();

            str1 = "ms-appx:///Assets/p1.jpg";
            str2 = "ms-appx:///Assets/p2.jpg";
            str3 = "ms-appx:///Assets/p3.jpg";
            img1.Source = new BitmapImage(new Uri(str1));
            img2.Source = new BitmapImage(new Uri(str2));
            img3.Source = new BitmapImage(new Uri(str3));

            this.SelectedFragment = null;
            this.img = new selectedImage();
            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatchTimer.Tick += dispatchTimer_Tick;
            t = 0;
            nameBlock.Visibility = name.Visibility = submit.Visibility = Visibility.Collapsed;
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
                    str4 = file.Path.ToString();
                    int position1 = str4.IndexOf('A');
                    int position2 = str4.IndexOf('.');
                    //string path = img.SRC.ToString();
                    string path = "ms-appx:///" + str4.Substring(position1, position2 - position1) + "-";
                    cutImage(path, ".jpg");//src.Substring(position));
                    /*var msgDialog = new Windows.UI.Popups.MessageDialog(path) { Title = "提示标题" };
                    await msgDialog.ShowAsync();*/
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                // If this is a new navigation, this is a fresh launch so we can
                // discard any saved state
                ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
            }
            else
            {
                // Try to restore state if any, in case we were terminated
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;

                    /*title.Text = (string)composite["title"];
                    details.Text = (string)composite["details"];
                    date.Date = (DateTimeOffset)composite["date"];*/

                    // We're done with it, so remove it
                    ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
                }
            }

            this.ViewModel = new ViewModels.GameSceneViewModel();
            /*var startPopup = new MessagePopUpWindow("", "确认难度");
            startPopup.ShowWindow();
            int d = startPopup.getDifficulty();*/

            this.dimension = (int)e.Parameter;
            this.initialOrder = new int[dimension * dimension];
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).IsSuspending;
            if (suspending)
            {
                // Save volatile state in case we get terminated later on, then
                // we can restore as if we'd never been gone :)
                var composite = new ApplicationDataCompositeValue();
                /*composite["title"] = title.Text;
                composite["details"] = details.Text;
                composite["date"] = date.Date;*/

                ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
            }
        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            if (this.SelectedFragment == null)
            {
                this.SelectedFragment = (Models.PuzzleFragment)(e.ClickedItem);
                //textBlock.Text = SelectedFragment.Index.ToString() + " " + SelectedFragment.CurIndex.ToString();
            }
            else
            {
                PuzzleFragment SecondFragment = (Models.PuzzleFragment)(e.ClickedItem);
                ViewModel.swap(this.SelectedFragment, SecondFragment);
                bool x = ViewModel.checkfinished();
                if (x == true)
                {
                    dispatchTimer.Stop();
                    nameBlock.Visibility = name.Visibility = submit.Visibility = Visibility.Visible;
                }
                //textBlock.Text = SelectedFragment.Index.ToString() + " " + SelectedFragment.CurIndex.ToString() + " " + ViewModel.correct.ToString() + " " + ViewModel.count.ToString();
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
            string src = "";
            switch (index)
            {
                case "one":
                    img.SRC = img1.Source;
                    src = str1;
                    break;
                case "two":
                    img.SRC = img2.Source;
                    src = str2;
                    break;
                case "three":
                    img.SRC = img3.Source;
                    src = str3;
                    break;
            }
            int position = src.IndexOf('.');
            //string path = img.SRC.ToString();
            string path = src.Substring(0, position) + "-";
            cutImage(path, ".jpg");//src.Substring(position));
        }

        private void cutImage(string path, string postfix)
        {
            if(dimension == 3)
            {
                gridView.Height = 400;
                gridView.Width = 400;
            }
            else
            {
                gridView.Height = 500;
                gridView.Width = 500;
            }
            while (ViewModel.Imgs.Count != 0)
            {
                ViewModel.removeFragment(0);
            }
            for (int i = 0; i < dimension * dimension; i++)
            {
                initialOrder[i] = dimension * dimension - 1 - i;
            }
            /*for (int i = 0; i < 50; i++)
            {
                Random random = new Random();
                int a = random.Next() % (dimension * dimension);
                int b = random.Next() % (dimension * dimension);
                int temp = initialOrder[a];
                initialOrder[a] = initialOrder[b];
                initialOrder[b] = temp;
            }*/
            for (int i = 0; i < dimension * dimension; i++)
            {
                string str = path + dimension.ToString() + "/" + initialOrder[i].ToString() + postfix;//碎片的path
                var a = new Models.PuzzleFragment();
                var img = new Windows.UI.Xaml.Controls.Image();         
                img.Source = new BitmapImage(new Uri(str));
                img.Width = img.Height = 100;
                a.CurIndex = i;
                a.Index = initialOrder[i];
                a.Img = img;
                a.Src = img.Source;
                this.ViewModel.addFragment(a);
            }
            t = 0;
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
            var db = App.conn;
            using (var statement = db.Prepare(@"INSERT INTO Record (Name,FinishTime) VALUES(?, ?)"))
            {
                StringBuilder result = new StringBuilder();
                if(name.Text == "")
                {
                    var msgDialog = new Windows.UI.Popups.MessageDialog("名字不能为空") { Title = "提示标题" };
                    msgDialog.ShowAsync();
                    return;
                }
                statement.Bind(1, name.Text);
                statement.Bind(2, t.ToString());
                statement.Step();
            }
            while (ViewModel.Imgs.Count != 0)
            {
                ViewModel.removeFragment(0);
            }
            nameBlock.Visibility = name.Visibility = submit.Visibility = Visibility.Collapsed;
            name.Text = "";
        }
    }

    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int? x = value as int?;
            x = x % 2;
            return x == 1 ? 3 : 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    //"{Binding Imgs.Count, Mode=OneWay, Converter={StaticResource ResourceKey=converter}}"
}
