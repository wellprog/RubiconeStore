using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViews;
using Shared.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class GoodCategoryListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Список категорий";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; } = new ToolbarItem[1];

        public Page Page { get; set; }
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;

        public GoodCategoryListViewModel()
        {
            var buttons = new ToolbarItem[1];
            buttons[0] = new ToolbarItem("Add", "", AddCategory);
            ToolbarItems = buttons;


            requestHelper = new RequestHelper();
            sessionData = new SessionDataStore();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async void AddCategory()
        {
            await Page.Navigation.PushAsync(new EditCategory());
        }

        public async Task Appearing()
        {

            var items = await requestHelper.Get<IEnumerable<GoodCategory>>($"http://rstore.kikoriki.space/GoodCategory/{ sessionData.SessionToken }");

            Elements.Clear();
            foreach (var item in items)
            {
                Elements.Add(
                    new ActionModel<GoodCategory>(item)
                    {
                        Text = item.Name,
                        Description = item.Description,
                        ExecAction = async f => await Page.Navigation.PushAsync(new EditCategory(item))
                    }
                );
            }

        }
    }
}
