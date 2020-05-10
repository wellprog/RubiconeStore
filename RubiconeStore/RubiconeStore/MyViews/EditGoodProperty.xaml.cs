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
    public partial class EditGoodProperty : ContentPage
    {
        private readonly EditGoodPropertyViewModel model;

        public EditGoodProperty(GoodCategory goodCategory, GoodProperty goodProperty = null)
        {
            InitializeComponent();
            if (goodProperty == null)
                goodProperty = new GoodProperty() { GoodCategoryID = goodCategory.ID };

            model = new EditGoodPropertyViewModel(goodCategory, goodProperty, new HttpClient(), this);
            BindingContext = model;
        }

        protected async override void OnAppearing()
        {
            await model.Appearing();
            base.OnAppearing();
        }
    }
}