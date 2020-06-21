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
using System.Linq;

using Xamarin.Forms;
using RubiconeStore.Client.ViewModels;
using RubiconeStore.Client.Views;

namespace RubiconeStore.MyViewModels
{
    class CheckViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Чек";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        public Page Page { get; set; }
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;

        private readonly Check check;

        public CheckViewModel(Check check)
        {
            this.check = check;

            sessionData = new SessionDataStore();
            requestHelper = new RequestHelper();
            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async Task Appearing()
        {
            Elements.Clear();
            foreach (var sell in check.Sells)
            {
                var item = sell.Storage.Good;

                var good = new ActionModel<Good>(item)
                {
                    Text = item.Title,
                    Description = item.Price.ToString() + " x" + sell.Count,
                    ExecAction = async f => await ShowCheckItemParams(sell)
                };

                Elements.Add(good);
            }
        }

        public async Task ShowCheckItemParams(Sell item)
        {
            await Page.Navigation.PushAsync(new GoodDetails(item.Storage.Good));
        }
    }
}
