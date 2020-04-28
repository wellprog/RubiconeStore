using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViews;
using Shared.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class GoodListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Список товаров";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        public Page Page { get; set; }
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;

        public GoodListViewModel()
        {
            var buttons = new ToolbarItem[1];
            buttons[0] = new ToolbarItem("Add", "", AddGood);
            ToolbarItems = buttons;
            
            sessionData = new SessionDataStore();

            requestHelper = new RequestHelper();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async void AddGood()
        {
            await Page.Navigation.PushAsync(new EditGood());
        }

        public async Task Appearing()
        {

            var items = await requestHelper.Get<IEnumerable<Good>>("http://rstore.kikoriki.space/Good", new Dictionary<string, object>
            {
                { "AuthKey", sessionData.UserAuthModel.UserSession.SessionToken}
            });

            foreach (var item in items)
            {
                Elements.Add(
                    new ActionModel<Good>(item)
                    {
                        Text = item.Title,
                        Description = item.Price.ToString(),
                        ExecAction = async f => await Page?.DisplayAlert("Good", f.Text, "Ok")
                    }
                );
            }

        }
    }
}
