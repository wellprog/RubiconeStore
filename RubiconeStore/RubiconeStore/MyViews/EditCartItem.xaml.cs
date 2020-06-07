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
    public partial class EditCartItem : ContentPage
    {
        private readonly EditCartItemViewModel model;
        public EditCartItem() : this(new CartItemModel()) { }

        public EditCartItem(CartItemModel cartItem)
        {
            InitializeComponent();
            model = new EditCartItemViewModel(cartItem, new HttpClient(), this);
            BindingContext = model;
        }

        protected async override void OnAppearing()
        {
            await model.Appearing();
            base.OnAppearing();
        }
    }
}