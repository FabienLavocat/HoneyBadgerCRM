using System;

namespace DnB.WindowsPhone
{
    public partial class PageSearch
    {
        public PageSearch()
        {
            InitializeComponent();
            LstStates.ItemsSource = new[]
                {
                    "CA",
                    "CO",
                    "FL",
                    "NV",
                    "NY"
                };
        }

        private void ButtonSearchClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageResults.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}