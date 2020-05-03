using RubiconeStore.MyViewModels;

using Shared.Model;

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
    public partial class EditCategory : ContentPage
    {
        private readonly EditCategoryViewModel _model;

        public EditCategory() : this(new GoodCategory()) { }

        public EditCategory(GoodCategory category)
        {
            InitializeComponent();
            _model = new EditCategoryViewModel(category, new System.Net.Http.HttpClient(), this);
            BindingContext = _model;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}