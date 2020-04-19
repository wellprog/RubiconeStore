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
    public partial class RegisterPage : ContentPage
    {
        private readonly RegisterViewModel _model;

        public RegisterPage()
        {
            InitializeComponent();
            _model = new RegisterViewModel(this);

            BindingContext = _model;
        }
    }
}