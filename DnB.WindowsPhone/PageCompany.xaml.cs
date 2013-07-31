using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using DnB.WindowsPhone.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace DnB.WindowsPhone
{
    public partial class PageCompany
    {
        #region CloseDealVisibility

        public Visibility CloseDealVisibility
        {
            get { return (Visibility)GetValue(CloseDealVisibilityProperty); }
            set { SetValue(CloseDealVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CloseDealVisibilityProperty =
            DependencyProperty.Register("CloseDealVisibility", typeof(Visibility), typeof(PageCompany), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageCompany), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Campaigns

        public IEnumerable<Campaign> Campaigns
        {
            get { return (IEnumerable<Campaign>)GetValue(CampaignsProperty); }
            set { SetValue(CampaignsProperty, value); }
        }

        public static readonly DependencyProperty CampaignsProperty =
            DependencyProperty.Register("Campaigns", typeof(IEnumerable<Campaign>), typeof(PageCompany), new PropertyMetadata(null));

        #endregion

        #region SelectedItem

        public Campaign SelectedItem
        {
            get { return (Campaign)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Campaign), typeof(PageCompany), new PropertyMetadata(null));

        #endregion

        #region CompanyName

        public string CompanyName
        {
            get { return (string)GetValue(CompanyNameProperty); }
            set { SetValue(CompanyNameProperty, value); }
        }

        public static readonly DependencyProperty CompanyNameProperty =
            DependencyProperty.Register("CompanyName", typeof(string), typeof(PageCompany), new PropertyMetadata(null));

        #endregion

        private string _companyId;

        public PageCompany()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CompanyName = NavigationContext.QueryString["name"];
            _companyId = NavigationContext.QueryString["id"];

            BackKeyPress += PageResultsBackKeyPress;

            Refresh();

            base.OnNavigatedTo(e);
        }

        private async void Refresh()
        {
            LoadingVisibility = Visibility.Visible;

            try
            {
                IMobileServiceTable<Campaign> campaignTable = App.MobileService.GetTable<Campaign>();
                Campaigns = await campaignTable.Where(c => c.CompanyId == _companyId).ToCollectionAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LoadingVisibility = Visibility.Collapsed;
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

                App.MobileServicesUser.Points += SelectedItem.TargetGain;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            CloseDealVisibility = Visibility.Collapsed;

            Refresh();
        }

        private void CampaignTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;

            SelectedItem = frameworkElement.DataContext as Campaign;
            CloseDealVisibility = Visibility.Visible;
        }

        private void BackCompaniesButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageCompanies.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}