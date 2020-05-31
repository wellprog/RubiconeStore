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
    public partial class AddStore : ContentPage
    { 
        public AddStore(Good good)
        {
            InitializeComponent();
            BindingContext = new AddStoreViewModel(good, this);
        }
    }
}