using RubiconeStore.MyModels;
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
    public partial class AdminPage : ContentPage
    {
        private readonly ITableViewModel bindModel;

        public AdminPage()
        {
            InitializeComponent();
            bindModel = new AdminViewModel(Navigation);
            BindingContext = bindModel;
        }

        public AdminPage(ITableViewModel model)
        {
            InitializeComponent();
            bindModel = model;
            BindingContext = bindModel;
        }

        private void OnItemTaped(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (IExecutableModel)layout.BindingContext;
            item.Exec();
        }

        private async void PageAppearing(object sender, EventArgs e)
        {

        }
    }
}