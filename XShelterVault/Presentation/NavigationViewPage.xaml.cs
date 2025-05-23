// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using ShelterVault.Models;

namespace XShelterVault.Presentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationViewPage : Page
    {
        public NavigationViewPage()
        {
            this.InitializeComponent();
            this.Loaded += NavigationViewPage_Loaded;
            this.CredentialsMenu.DataContextChanged += CredentialsMenu_DataContextChanged;
        }

        private void NavigationViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            AppContent.Navigate(typeof(HomePage));
        }

        private void CredentialsMenu_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null && args.NewValue is NavigationViewModel nvm)
            {
                if (nvm.Credentials.Any())
                {
                    this.CredentialsMenu.Visibility = Visibility.Visible;
                    foreach (CredentialsViewItem credentialsViewItem in nvm.Credentials)
                    {
                        NavigationViewItem navigationViewItem = new() { Content = credentialsViewItem.Title, Icon = new FontIcon() { Glyph = "\uE72E" }, Tag = credentialsViewItem };
                        navigationViewItem.SetValue(ToolTipService.ToolTipProperty, credentialsViewItem.Title);
                        //navigationViewItem.SetValue(PageLoaderBehavior.PageTypeProperty, typeof(CredentialsPage));
                        this.CredentialsMenu.MenuItems.Add(navigationViewItem);
                    }
                }
                else
                {
                    this.CredentialsMenu.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
