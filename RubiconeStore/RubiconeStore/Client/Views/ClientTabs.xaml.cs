using RubiconeStore.Client.ViewModels;
using RubiconeStore.MyViewModels;
using RubiconeStore.MyViews;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace RubiconeStore.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientTabs : Xamarin.Forms.TabbedPage
    {
        public ClientTabs()
        {
            InitializeComponent();

            //On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            NavigationLogin.PushAsync(new LoginPage(ShowUser));
            NavigationMain.PushAsync(new SimpleTablePage() { ViewModel = new CategoryViewModel() });
            NavigationBasket.PushAsync(new SimpleTablePage() { ViewModel = new CartViewModel() });
            NavigationAdmin.PushAsync(new SimpleTablePage
            {
                ViewModel = new AdminViewModel()
            });
        }

        private async void ShowUser()
        {
            await NavigationLogin.PushAsync(new UserDetails());
        }
    }
}