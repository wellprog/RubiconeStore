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
        public LoginPage()
        {
            InitializeComponent();
            model = new LoginViewModel(this);
            BindingContext = model;
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            await model.LoginMe();
        }
    }
}