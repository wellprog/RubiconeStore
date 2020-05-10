using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViews;
using Shared.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class GoodPropertyListViewController : BaseViewModel, ITableViewModel
    {
        public string PageName { get; private set; } = "Список атрибутов для ";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        public Page Page { get; set; }
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;
        private readonly GoodCategory goodCategory;

        public GoodPropertyListViewController(GoodCategory goodCategory)
        {
            this.goodCategory = goodCategory;

            var buttons = new ToolbarItem[1];
            buttons[0] = new ToolbarItem("Add", "", AddProperty);
            ToolbarItems = buttons;

            sessionData = new SessionDataStore();

            requestHelper = new RequestHelper();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async void AddProperty()
        {
            await Page.Navigation.PushAsync(new EditGoodProperty(goodCategory));
        }

        public async Task Appearing()
        {

            var items = await requestHelper.Get<IEnumerable<GoodProperty>>($"http://rstore.kikoriki.space/GoodProperty/{ sessionData.SessionToken }");

            Elements.Clear();
            foreach (var item in items.Where(f => f.GoodCategoryID == goodCategory.ID))
            {
                var good = new ActionModel<GoodProperty>(item)
                {
                    Text = item.Name,
                    Description = item.Description,
                    ExecAction = async f => await Page.Navigation.PushAsync(new EditGoodProperty(goodCategory, item))
                };

                good.AddLeftSwipe("Delete", Color.Red, new Command(async f => await DeleteGoodProperty(f as Good)));

                Elements.Add(good);
            }

        }

        public async Task DeleteGoodProperty(Good item)
        {
            await Page.DisplayAlert("Test", item.Title, "Ok");
        }

        public async Task EditGoodProperty(Good item)
        {
            await Page.DisplayAlert("Test", item.Title, "Ok");
        }

    }
}
