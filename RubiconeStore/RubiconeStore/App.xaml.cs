using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RubiconeStore.Services;
using RubiconeStore.Views;
using RubiconeStore.MyViews;
using RubiconeStore.Client.Views;

namespace RubiconeStore
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();

            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new NavigationPage(new EditGood());
            MainPage = new ClientTabs();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
