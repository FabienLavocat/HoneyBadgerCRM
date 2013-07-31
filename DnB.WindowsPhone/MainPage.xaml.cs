using System;
using System.Windows;
using System.Windows.Navigation;

namespace DnB.WindowsPhone
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BtLogin.Visibility = App.IsAuthenticated ? Visibility.Collapsed : Visibility.Visible;
            BtMyDeals.Visibility = App.IsAuthenticated ? Visibility.Visible : Visibility.Collapsed;
            TxtPoints.Text = App.IsAuthenticated ? string.Concat(App.MobileServicesUser.Points, " points available.") : string.Empty;

            base.OnNavigatedTo(e);
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageLogin.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MyDealsButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageCompanies.xaml", UriKind.RelativeOrAbsolute));
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageStart.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}