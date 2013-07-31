using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using DnB.WindowsPhone.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace DnB.WindowsPhone
{
    public partial class PageCompanies
    {
        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageCompanies), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Campaigns

        public IEnumerable<Campaign> Campaigns
        {
            get { return (IEnumerable<Campaign>)GetValue(CampaignsProperty); }
            set { SetValue(CampaignsProperty, value); }
        }

        public static readonly DependencyProperty CampaignsProperty =
            DependencyProperty.Register("Campaigns", typeof(IEnumerable<Campaign>), typeof(PageCompanies), new PropertyMetadata(null));

        #endregion

        public PageCompanies()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
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

            var campaign = frameworkElement.DataContext as Campaign;
            if (campaign == null) return;

            string url = string.Format("/PageCompany.xaml?id={0}&name={1}",
                                       campaign.CompanyId,
                                       HttpUtility.UrlEncode(campaign.CompanyName));
            NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
        }
    }
}