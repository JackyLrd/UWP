using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EasyPuzzle.Views;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace EasyPuzzle
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            var startPopup = new MessagePopUpWindow("", "确认难度");
            startPopup.ShowWindow();
            int d = startPopup.getDifficulty();
            Frame.Navigate(typeof(Views.GameScene), d);
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Views.GameScene));
        }

        private void rulesButton_Click(object sender, RoutedEventArgs e)
        {
            string rulesText = "游戏规则：\n将拼图碎片一一放在正确的位置上，当拼图完成时，记录完成的时间并计入榜单。\n";
            var rulesPopup = new MessagePopUpWindow(rulesText, "我知道了");
            rulesPopup.ShowWindow();
        }

        private void rankingButton_Click(object sender, RoutedEventArgs e)
        {
            //todo: connect to the database
            string rankingText = "积分榜：\n1.xxx\n2.xxxx\n3.xxx\n4.xxx\n5.xxxx\n";
            var rankingPopup = new MessagePopUpWindow(rankingText, "点个赞");
            rankingPopup.ShowWindow();
        }
    }
}
