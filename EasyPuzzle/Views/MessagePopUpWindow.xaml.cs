    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace EasyPuzzle.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MessagePopUpWindow : UserControl
    {
        private Popup myPopup;
        private string myTextContent;
        private int difficulty;
        private MessagePopUpWindow()
        {
            this.InitializeComponent();

            myPopup = new Popup();
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            myPopup.Child = this;
            this.Loaded += MessagePopUpWindow_Loaded;
            this.Unloaded += MessagePopUpWindow_UnLoaded;
        }
        public MessagePopUpWindow(string showMsg, string btnText) : this()
        {
            this.myTextContent = showMsg;
            Btn.Content = btnText;
            if (btnText == "确认难度")
                DifficultyChoose.Visibility = Visibility.Visible;
            else
                DifficultyChoose.Visibility = Visibility.Collapsed;
        }
        private void MessagePopUpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbContent.Text = myTextContent;
            Window.Current.SizeChanged += MessagePopUpWindow_SizeChanged;
        }
        private void MessagePopUpWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }

        private void MessagePopUpWindow_UnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= MessagePopUpWindow_SizeChanged;
        }

        public void ShowWindow()
        {
            myPopup.IsOpen = true;
        }

        public void DismissWindow()
        {
            myPopup.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string btnText = (string)Btn.Content;
            DismissWindow();
        }


        private void difficulty3m3_Checked(object sender, RoutedEventArgs e)
        {
            if (difficulty3m3.IsChecked == false)
            {
                difficulty3m3.IsChecked = true;
                difficulty4m4.IsChecked = false;
                difficulty = 3;
            }
        }

        private void difficulty4m4_Checked(object sender, RoutedEventArgs e)
        {
            if (difficulty4m4.IsChecked == false)
            {
                difficulty4m4.IsChecked = true;
                difficulty3m3.IsChecked = false;
                difficulty = 4;
            }
        }

        public int getDifficulty()
        {
            return difficulty;
        }

        public bool isEnd()
        {
            return myPopup.IsOpen;
        }
    }
}
