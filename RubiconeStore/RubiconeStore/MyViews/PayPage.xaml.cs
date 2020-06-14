using RubiconeStore.MyViewModels;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RubiconeStore.MyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayPage : ContentPage
    {
        private readonly PayViewModel model;

        public PayPage()
        {
            InitializeComponent();
            model = new PayViewModel(new HttpClient(), this);
            BindingContext = model;
        }

        protected async override void OnAppearing()
        {
            await model.Appearing();
            base.OnAppearing();
        }
    }
}