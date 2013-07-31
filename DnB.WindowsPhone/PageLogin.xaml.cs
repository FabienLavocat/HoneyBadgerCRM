using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DnB.WindowsPhone.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace DnB.WindowsPhone
{
    public partial class PageLogin
    {
        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageLogin), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        public PageLogin()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void LoginTwitterClick(object sender, RoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.Twitter);
        }

        private async void LoginFacebookClick(object sender, RoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
        }

        private async void LoginMicrosoftClick(object sender, RoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        private async void LoginGoogleClick(object sender, RoutedEventArgs e)
        {
            await AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
        }

        private async Task AuthenticateAsync(MobileServiceAuthenticationProvider service)
        {
            LoadingVisibility = Visibility.Visible;

            try
            {
                MobileServiceUser user = await App.MobileService.LoginAsync(service);

                IMobileServiceTable<User> userTable = App.MobileService.GetTable<User>();
                await userTable.InsertAsync(new User(user.UserId));

                var collection = await userTable.Where(u => u.UserId == user.UserId).Take(1).ToCollectionAsync();
                App.Authenticate(collection.FirstOrDefault());

                LoadingVisibility = Visibility.Collapsed;

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            catch (InvalidOperationException)
            {
                LoadingVisibility = Visibility.Collapsed;

                MessageBox.Show("An error happened, please try again.");
            }
        }
    }
}