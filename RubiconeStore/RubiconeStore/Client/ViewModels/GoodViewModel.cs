using RubiconeStore.Client.Views;
using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViewModels;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RubiconeStore.Client.ViewModels
{
    public class GoodViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Товары";

        public Page Page { get; set; }

        public ObservableCollection<IExecutableModel> Elements { get; private set; } = new ObservableCollection<IExecutableModel>();

        public IEnumerable<ToolbarItem> ToolbarItems { get; } = new ToolbarItem[0];

        private readonly RequestHelper requestHelper = new RequestHelper();
        private readonly SessionDataStore sessionData = new SessionDataStore();
        private readonly GoodCategory category;

        public GoodViewModel (GoodCategory category) { this.category = category; }

        public async Task Appearing()
        {
            //var items = await requestHelper.Get<IEnumerable<Good>>($"http://rstore.kikoriki.space/GoodList/{ sessionData.SessionToken }/{ category.ID }");

            //Elements.Clear();
            //foreach (var item in items)
            //{
            //    var good = new ActionModel<Good>(item)
            //    {
            //        Text = item.Title,
            //        Description = item.Price.ToString(),
            //        ExecAction = async f => { item.GoodCategory = category; await Page.Navigation.PushAsync(new GoodDetails(item)); }
            //    };

            //    Elements.Add(good);
            //}


            var items = await requestHelper.Get<IEnumerable<GoodCount>>($"http://rstore.kikoriki.space/GoodList/{ sessionData.SessionToken }/{ category.ID }");
            
            Elements.Clear();
            foreach (var item in items)
            {
                var good = new ActionModel<GoodCount>(item)
                {
                    Text = $"{item.Good.Title} {item.Count}",
                    Description = item.Good.Price.ToString(),
                    ExecAction = async f => { item.Good.GoodCategory = category; await Page.Navigation.PushAsync(new GoodDetails(item.Good)); }
                };

                Elements.Add(good);
            }
        }

    }
}
