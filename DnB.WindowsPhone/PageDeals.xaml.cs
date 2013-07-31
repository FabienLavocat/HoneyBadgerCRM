using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using DnB.WindowsPhone.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace DnB.WindowsPhone
{
    public partial class PageDeals
    {
        #region CloseDealVisibility

        public Visibility CloseDealVisibility
        {
            get { return (Visibility)GetValue(CloseDealVisibilityProperty); }
            set { SetValue(CloseDealVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CloseDealVisibilityProperty =
            DependencyProperty.Register("CloseDealVisibility", typeof(Visibility), typeof(PageDeals), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region SelectedItem

        public Campaign SelectedItem
        {
            get { return (Campaign)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Campaign), typeof(PageDeals), new PropertyMetadata(null));

        #endregion

        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageDeals), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Campaigns

        public IEnumerable<Campaign> Campaigns
        {
            get { return (IEnumerable<Campaign>)GetValue(CampaignsProperty); }
            set { SetValue(CampaignsProperty, value); }
        }

        public static readonly DependencyProperty CampaignsProperty =
            DependencyProperty.Register("Campaigns", typeof(IEnumerable<Campaign>), typeof(PageDeals), new PropertyMetadata(null));

        #endregion

        public PageDeals()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackKeyPress += PageResultsBackKeyPress;

            LoadingVisibility = Visibility.Visible;

            try
            {
                IMobileServiceTable<Campaign> campaignTable = App.MobileService.GetTable<Campaign>();
                Campaigns = await campaignTable.ToCollectionAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LoadingVisibility = Visibility.Collapsed;

            base.OnNavigatedTo(e);
        }

        private void CampaignTap(object sender, GestureEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;

            CloseDealVisibility = Visibility.Visible;
            SelectedItem = frameworkElement.DataContext as Campaign;
        }

        private void PageResultsBackKeyPress(object sender, CancelEventArgs e)
        {
            if (CloseDealVisibility == Visibility.Collapsed)
                return;

            e.Cancel = true;
            CloseDealVisibility = Visibility.Collapsed;
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            CloseDealVisibility = Visibility.Collapsed;
        }

        private async void YesButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) return;

            SelectedItem.IsClosed = true;

            try
            {
                IMobileServiceTable<Campaign> campaignTable = App.MobileService.GetTable<Campaign>();
                await campaignTable.UpdateAsync(SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            CloseDealVisibility = Visibility.Collapsed;
        }
    }
}