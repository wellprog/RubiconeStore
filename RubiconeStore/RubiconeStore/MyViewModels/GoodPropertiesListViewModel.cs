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
    public class GoodPropertiesListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName { get; private set; } = "Список атрибутов для ";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        public Page Page { get; set; }
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;
        private readonly GoodPropertiesModel viewedGoodProperties;

        public GoodPropertiesListViewModel(GoodPropertiesModel viewedGoodProperties)
        {
            this.viewedGoodProperties = viewedGoodProperties;

            var buttons = new ToolbarItem[1];
            buttons[0] = new ToolbarItem("Add", "", AddProperty);
            ToolbarItems = buttons;

            sessionData = new SessionDataStore();

            requestHelper = new RequestHelper();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async void AddProperty()
        {
            await Page.Navigation.PushAsync(new EditGoodProperty(viewedGoodProperties.Good.GoodCategory)); //Все значение строго определены, так что перенаправляем на страницу изменения Категории
        }

        public async Task Appearing()
        {
            var items = viewedGoodProperties;

            Elements.Clear();
            foreach (var property in items.GoodProperties)
            {
                var itemPropertyValue = items.GoodPropertyValues.Where(f => f.GoodPropertyID == property.ID).FirstOrDefault();

                var good = new ActionModel<GoodProperty>(property)
                {
                    Text = property.Name,
                    Description = itemPropertyValue?.Value,
                    ExecAction = async f => await EditGoodPropertyValue(property)
                };

                good.AddLeftSwipe("Delete", Color.Red, new Command(async f => await DeleteGoodPropertyValue(f as GoodProperty)));
                good.AddRightSwipe("Edit", Color.Yellow, new Command(async f => await EditGoodPropertyValue(f as GoodProperty)));
                //Предложить поставить значение?

                Elements.Add(good);
            }

        }

        public async Task DeleteGoodPropertyValue(GoodProperty item)
        {
            await requestHelper.Delete<GoodProperty>($"http://rstore.kikoriki.space/GoodProperty/{ sessionData.SessionToken }/{ item.ID }");
            foreach (var i in Elements)
                if (item == i.getModelItem()) { Elements.Remove(i); break; }
            await Page.DisplayAlert("Delete Property success!", item.Name, "Ok");
        }

        public async Task EditGoodPropertyValue(GoodProperty item)
        {
            await Page.Navigation.PushAsync(new EditGoodProperty(viewedGoodProperties.Good.GoodCategory, item));
        }

    }
}
