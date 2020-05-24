using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViewModels;
using RubiconeStore.MyViews;

using Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.Client.ViewModels
{
    public class CategoryViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Категории";

        public Page Page { get; set; }

        public ObservableCollection<IExecutableModel> Elements { get; private set; } = new ObservableCollection<IExecutableModel>();

        public IEnumerable<ToolbarItem> ToolbarItems { get; } = new ToolbarItem[0];

        private readonly RequestHelper requestHelper = new RequestHelper();
        private readonly SessionDataStore sessionData = new SessionDataStore();

        public CategoryViewModel() { }

        public async Task Appearing()
        {
            if (sessionData.SessionToken == null)
                return;

            var items = await requestHelper.Get<IEnumerable<GoodCategory>>($"http://rstore.kikoriki.space/GoodCategory/{ sessionData.SessionToken }");

            Elements.Clear();
            foreach (var item in items)
            {
                var category = new ActionModel<GoodCategory>(item)
                {
                    Text = item.Name,
                    Description = item.Description,
                    ExecAction = async f => await Page.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new GoodViewModel(f) })
                };

                Elements.Add(category);
            }

        }
    }
}
