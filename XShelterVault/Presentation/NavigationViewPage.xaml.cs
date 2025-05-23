// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using CommunityToolkit.Mvvm.Messaging;
using ShelterVault.Models;
using ShelterVault.Shared.Messages;

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
            RegisterMessages();
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
                        navigationViewItem.PointerReleased += NavigationViewItem_PointerReleased;
                        this.CredentialsMenu.MenuItems.Add(navigationViewItem);
                    }
                }
                else
                {
                    this.CredentialsMenu.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void NavigationViewItem_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var selectedCredential = sender as NavigationViewItem;
            if (selectedCredential != null && selectedCredential.IsSelected)
            {
                this.AppContent.Navigate(typeof(CredentialsPage));
                var cvm = (this.AppContent.Content as CredentialsPage).DataContext as CredentialsViewModel;
                cvm.OnNavigated(selectedCredential.Tag);
            }
        }

        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register<NavigationViewPage, ShowPageRequestMessage>(this, (viewModel, payload) =>
            {
                this.AppContent.Navigate(payload.Value);
            });
        }
    }
}
