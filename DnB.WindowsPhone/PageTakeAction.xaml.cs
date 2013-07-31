using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DnB.WindowsPhone.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace DnB.WindowsPhone
{
    public partial class PageTakeAction
    {
        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageTakeAction), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region TargetDate

        public DateTime TargetDate
        {
            get { return (DateTime)GetValue(TargetDateProperty); }
            set { SetValue(TargetDateProperty, value); }
        }

        public static readonly DependencyProperty TargetDateProperty =
            DependencyProperty.Register("TargetDate", typeof(DateTime), typeof(PageTakeAction), new PropertyMetadata(DateTime.Now.AddMonths(1)));

        #endregion

        #region TargetGain

        public int TargetGain
        {
            get { return (int)GetValue(TargetGainProperty); }
            set { SetValue(TargetGainProperty, value); }
        }

        public static readonly DependencyProperty TargetGainProperty =
            DependencyProperty.Register("TargetGain", typeof(int), typeof(PageTakeAction), new PropertyMetadata(0));

        #endregion

        #region Notes

        public string Notes
        {
            get { return (string)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }

        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register("Notes", typeof(string), typeof(PageTakeAction), new PropertyMetadata(""));

        #endregion

        public PageTakeAction()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _companyId;
        private string _companyName;

        private static IEnumerable<CampaignType> _campaignTypes;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _companyId = NavigationContext.QueryString["id"];
            _companyName = HttpUtility.UrlDecode(NavigationContext.QueryString["name"]);

            LoadingVisibility = Visibility.Visible;

            try
            {
                if (_campaignTypes == null)
                {
                    IMobileServiceTable<CampaignType> campaignTypeTable = App.MobileService.GetTable<CampaignType>();
                    _campaignTypes = await campaignTypeTable.ToCollectionAsync();
                }

                LstCampaignTypes.ItemsSource = _campaignTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LoadingVisibility = Visibility.Collapsed;

            base.OnNavigatedTo(e);
        }

        private async void SendButtonClick(object sender, EventArgs e)
        {
            TxtTargetGain.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            TxtNotes.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (TargetDate < DateTime.Now) return;

            CampaignType campaign = LstCampaignTypes.SelectedItem as CampaignType;
            if (campaign == null) return;

            if (campaign.Cost > App.MobileServicesUser.Points)
            {
                MessageBox.Show("You don't have enough points to start this campaign!");
                return;
            }

            Campaign c = new Campaign
                {
                    User = App.MobileServicesUser.UserId,
                    Name = campaign.Name,
                    CompanyId = _companyId,
                    CompanyName = _companyName,
                    Cost = campaign.Cost,
                    TargetGain = TargetGain,
                    TargetDate = TargetDate,
                    IsClosed = false,
                    Notes = Notes
                };

            LoadingVisibility = Visibility.Visible;

            try
            {
                IMobileServiceTable<Campaign> campaignTable = App.MobileService.GetTable<Campaign>();
                await campaignTable.InsertAsync(c);

                App.MobileServicesUser.Points -= campaign.Cost;

                string url = string.Format("/PageCompany.xaml?id={0}&name={1}",
                                           _companyId,
                                           HttpUtility.UrlEncode(_companyName));
                NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
            }
            catch (Exception)
            {
                MessageBox.Show("An error happened. Please try again.");
            }
            finally
            {
                LoadingVisibility = Visibility.Collapsed;
            }
        }
    }
}