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
    public class GoodPropertyListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName { get; private set; } = "Список атрибутов для ";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        public Page Page { get; set; }
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;
        private readonly GoodCategory goodCategory;

        public GoodPropertyListViewModel(GoodCategory goodCategory)
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
                    ExecAction = async f => await EditGoodProperty(item)
                };

                good.AddLeftSwipe("Delete", Color.Red, new Command(async f => await DeleteGoodProperty(f as GoodProperty)));
                good.AddRightSwipe("Edit", Color.Yellow, new Command(async f => await EditGoodProperty(f as GoodProperty)));

                Elements.Add(good);
            }

        }

        public async Task DeleteGoodProperty(GoodProperty item)
        {
            await requestHelper.Delete<GoodProperty>($"http://rstore.kikoriki.space/GoodProperty/{ sessionData.SessionToken }/{ item.ID }");
            foreach (var i in Elements)
                if (item == i.getModelItem()) { Elements.Remove(i); break; }
            await Page.DisplayAlert("Delete Property success!", item.Name, "Ok");
        }

        public async Task EditGoodProperty(GoodProperty item)
        {
            await Page.Navigation.PushAsync(new EditGoodProperty(goodCategory, item));
        }

    }
}
