using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using DnB.WindowsPhone.Core;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DnB.WindowsPhone
{
    public partial class PageResults
    {
        #region ItemDisplayVisibility

        public Visibility ItemDisplayVisibility
        {
            get { return (Visibility)GetValue(ItemDisplayVisibilityProperty); }
            set { SetValue(ItemDisplayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ItemDisplayVisibilityProperty =
            DependencyProperty.Register("ItemDisplayVisibility", typeof(Visibility), typeof(PageResults), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageResults), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region SelectedItem

        public CompanyResult SelectedItem
        {
            get { return (CompanyResult)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(CompanyResult), typeof(PageResults), new PropertyMetadata(null));

        #endregion

        #region IsAuthenticated

        public bool IsAuthenticated
        {
            get { return (bool)GetValue(IsAuthenticatedProperty); }
            set { SetValue(IsAuthenticatedProperty, value); }
        }

        public static readonly DependencyProperty IsAuthenticatedProperty =
            DependencyProperty.Register("IsAuthenticated", typeof (bool), typeof (PageResults), new PropertyMetadata(false));

        #endregion

        public PageResults()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadingVisibility = Visibility.Visible;

            var state = NavigationContext.QueryString["state"];
            var city = NavigationContext.QueryString["city"];
            var categoryId = NavigationContext.QueryString["category"];

            IsAuthenticated = App.IsAuthenticated;
            BackKeyPress += PageResultsBackKeyPress;
            base.OnNavigatedTo(e);

            try
            {
                int catId = int.Parse(categoryId);
                await SearchAsync(state, city, catId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            finally
            {
                LoadingVisibility = Visibility.Collapsed;
            }
        }

        private void PageResultsBackKeyPress(object sender, CancelEventArgs e)
        {
            if (ItemDisplayVisibility == Visibility.Collapsed)
                return;

            e.Cancel = true;
            ItemDisplayVisibility = Visibility.Collapsed;
        }

        private const string URL_BASE = "/api/firmographics";

        private static async Task<T> DownloadResultAsync<T>(string url)
        {
            var client = new WebClient { Credentials = new NetworkCredential("accountKey", "LWUJDdwrKgcinhYZ4keiti99TikByzb+32tibFieZ+Q=") };
            var result = await client.DownloadStringTaskAsync(new Uri(url));

            return JsonConvert.DeserializeObject<T>(result);
        }

        private static async Task<Category> GetCategoryAsync(int id)
        {
            IMobileServiceTable<Category> categoryTable = App.MobileService.GetTable<Category>();
            var collection = await categoryTable.Where(c => c.Id == id).Take(1).ToCollectionAsync();
            return collection.FirstOrDefault();
        }

        private async Task SearchAsync(string state, string city, int categoryId)
        {
            var category = await GetCategoryAsync(categoryId);

            string filter = string.Format("?StateAbbrv={0}&IndustryCode={1}", state, category.Code);

            if (!string.IsNullOrWhiteSpace(city))
            {
                filter += "&City=" + HttpUtility.UrlEncode(city);
            }

            string url = string.Concat(App.MOBILE_SERVICE_URL, URL_BASE, filter);
            var results = await DownloadResultAsync<CompanyResult[]>(url);

            if (!results.Any())
                throw new Exception("We cannot find any result. Please try with a different Category / City / State.");

            var children = MapExtensions.GetChildren(Map);
            bool isFirst = true;

            foreach (var item in results)
            {
                var pushpin = new Pushpin
                    {
                        GeoCoordinate = new System.Device.Location.GeoCoordinate(item.Latitude, item.Longitude),
                        DataContext = item
                    };

                if (isFirst)
                {
                    Map.Center = pushpin.GeoCoordinate;
                    isFirst = false;
                }

                pushpin.Tap += PushpinTap;

                children.Add(pushpin);
            }
        }

        private void PushpinTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;

            ItemDisplayVisibility = Visibility.Visible;
            SelectedItem = frameworkElement.DataContext as CompanyResult;
        }

        private void ActionButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) return;

            string url = string.Format("/PageTakeAction.xaml?id={0}&name={1}", SelectedItem.DUNSNumber, HttpUtility.UrlEncode(SelectedItem.Company));
            NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        private void CallButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) return;

            PhoneCallTask phoneCallTask = new PhoneCallTask
                {
                    PhoneNumber = string.Format("+{0} {1}", SelectedItem.InternationalDialingCode, SelectedItem.Phone),
                    DisplayName = SelectedItem.Company
                };

            phoneCallTask.Show();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageLogin.xaml", UriKind.RelativeOrAbsolute));
        }
    }

    [DataContract]
    public class CompanyResult
    {
        [DataMember]
        // ReSharper disable InconsistentNaming
        public string DUNSNumber { get; set; }
        // ReSharper restore InconsistentNaming
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string StateAbbrv { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public int InternationalDialingCode { get; set; }

        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public string AccuracyCode { get; set; }
    }
}