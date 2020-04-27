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
    public partial class EditGood : ContentPage
    {
        private readonly EditGoodViewModel model;
        public EditGood()
        {
            InitializeComponent();
            model = new EditGoodViewModel(new Good(), new HttpClient(), this);

            BindingContext = model;
        }
    }
}