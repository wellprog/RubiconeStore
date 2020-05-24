using RubiconeStore.DataStores;
using RubiconeStore.MyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RubiconeStore.MyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel model;
        private readonly SessionDataStore _sessionDataStore;

        public LoginPage()
        {
            InitializeComponent();
            model = new LoginViewModel(this);
            BindingContext = model;
            _sessionDataStore = new SessionDataStore();
            model.NextAction = showAdmin;
        }

        public LoginPage(Action Next) : this()
        {
            model.NextAction = Next;
        }

        protected override async void OnAppearing()
        {
            if (_sessionDataStore.UserAuthModel != null)
            {
                model.ShowNext();
            }
        }

        protected async void showAdmin()
        {
            await this.Navigation.PushAsync(new SimpleTablePage
            {
                ViewModel = new AdminViewModel()
            });
        }
    }
}