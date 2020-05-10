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
    public partial class EditGoodProperty : ContentPage
    {

        public EditGoodProperty(GoodCategory goodCategory, GoodProperty goodProperty = null)
        {
            InitializeComponent();
        }
    }
}