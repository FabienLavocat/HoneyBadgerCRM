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
    public partial class PageStart
    {
        #region LoadingVisibility

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PageStart), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region SelectedItem

        public Category SelectedItem
        {
            get { return (Category)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Category), typeof(PageStart), new PropertyMetadata(null));

        #endregion

        #region City

        public string City
        {
            get { return (string)GetValue(CityProperty); }
            set { SetValue(CityProperty, value); }
        }

        public static readonly DependencyProperty CityProperty =
            DependencyProperty.Register("City", typeof(string), typeof(PageStart), new PropertyMetadata(""));

        #endregion

        public PageStart()
        {
            InitializeComponent();
            DataContext = this;
        }

        private static IEnumerable<Category> _categories;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadingVisibility = Visibility.Visible;

            LstStates.ItemsSource = Core.State.GetStates();

            try
            {
                if (_categories == null)
                {
                    IMobileServiceTable<Category> categoryTable = App.MobileService.GetTable<Category>();
                    _categories = await categoryTable.ToCollectionAsync();
                }
                
                LstCategory.ItemsSource = _categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LoadingVisibility = Visibility.Collapsed;

            base.OnNavigatedTo(e);
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            var binding = TxtCity.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            var category = LstCategory.SelectedItem as Category;
            if (category == null) return;

            var state = LstStates.SelectedItem as State;
            if (state == null) return;

            string url = string.Format("/PageResults.xaml?state={0}&city={1}&category={2}",
                                       state.Short,
                                       HttpUtility.UrlEncode(City.ToUpperInvariant()),
                                       category.Id);
            NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
        }
    }
}