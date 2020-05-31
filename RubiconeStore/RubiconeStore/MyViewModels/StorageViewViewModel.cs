using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViews;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace RubiconeStore.MyViewModels
{
    public class StorageViewViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Склад для товара - ";

        public Page Page { get; set; }

        public ObservableCollection<IExecutableModel> Elements { get; } = new ObservableCollection<IExecutableModel>();

        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        private readonly SessionDataStore sessionData = new SessionDataStore();
        private readonly RequestHelper requestHelper = new RequestHelper();
        private readonly Good good;

        public StorageViewViewModel(Good good)
        {
            this.good = good;

            ToolbarItem[] items = new ToolbarItem[1];
            items[0] = new ToolbarItem("Add", "", async () => { await Page.Navigation.PushAsync(new AddStore(good)); });

            ToolbarItems = items;
        }

        public async Task Appearing()
        {
            var items = await requestHelper.Get<IEnumerable<Storage>>($"http://rstore.kikoriki.space/Store/{ sessionData.SessionToken }/{ good.ID }");

            Elements.Clear();
            foreach (var item in items)
            {
                var good = new ActionModel<Storage>(item)
                {
                    Text = item.Count.ToString(),
                    Description = item.Price.ToString(),
                };

                Elements.Add(good);
            }

        }
    }
}
