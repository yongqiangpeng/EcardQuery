﻿using System;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace EcardQuery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RealtimeInquiryPage : Page
    {
        public RealtimeInquiryPage()
        {
            this.InitializeComponent();

            accountPicker.DataContext = App.websiteHelper.HistoryAccountIds;
            accountPicker.SelectedIndex = 0;
        }

        public static List<TranscationData> dataList;
        bool isShowingData = false;

        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            try
            {
                dataList = await App.websiteHelper.RealtimeInquire((string)accountPicker.SelectedItem);
                displayList.DataContext = dataList;
                isShowingData = true;
                displayList.Visibility = Visibility.Visible;
                if (ActualWidth < 600)
                {
                    controlPanel.Visibility = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                statusBlock.Text = ex.Message;
            }
            progressRing.IsActive = false;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (isShowingData && controlPanel.Visibility == Visibility.Collapsed && e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
                controlPanel.Visibility = Visibility.Visible;
                displayList.Visibility = Visibility.Collapsed;
                isShowingData = false;
                return;
            }
            base.OnNavigatingFrom(e);
        }

        private void VisualStateGroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState == wideState)
            {
                controlPanel.Visibility = Visibility.Visible;
                displayList.Visibility = Visibility.Visible;
            }
            else
            {
                if (isShowingData)
                    controlPanel.Visibility = Visibility.Collapsed;
                else
                    displayList.Visibility = Visibility.Collapsed;
            }
        }
    }
}