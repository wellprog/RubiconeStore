﻿using RubiconeStore.DataStores;
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
            var items = await requestHelper.Get<IEnumerable<Good>>($"http://rstore.kikoriki.space/GoodList/{ sessionData.SessionToken }");

            Elements.Clear();
            foreach (var item in items)
            {
                var good = new ActionModel<Good>(item)
                {
                    Text = item.Title,
                    Description = item.Price.ToString(),
                    ExecAction = async f => await Page.Navigation.PushAsync(new EditGood(item))
                };

                good.AddLeftSwipe("Delete", Color.Red, new Command(async f => await DeleteGood(f as Good)));
                good.AddLeftSwipe("Storage", Color.Blue, new Command(async f => await ShowStorage(f as Good)));
                good.AddRightSwipe("Edit", Color.Yellow, new Command(async f => await EditGood(f as Good)));
                good.AddRightSwipe("Params", Color.Gray, new Command(async f => await ShowParams(f as Good)));

                Elements.Add(good);
            }

        }

        public async Task DeleteGood(Good item)
        {
            await requestHelper.Delete<Good>($"http://rstore.kikoriki.space/Good/{ sessionData.SessionToken }/{ item.ID }");
            foreach (var i in Elements)
                if (item == i.getModelItem()) { Elements.Remove(i); break; }
            await Page.DisplayAlert("Delete Good success!", item.Title, "Ok");
        }

        public async Task EditGood(Good item)
        {
            await Page.Navigation.PushAsync(new EditGood(item));
        }

        public async Task ShowParams(Good item)
        {
            GoodPropertiesModel itemGoodProperties = await requestHelper.Get<GoodPropertiesModel>($"http://rstore.kikoriki.space/GoodsProperties/{ sessionData.SessionToken }/{ item.ID }");
            await Page.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new GoodPropertiesListViewModel(itemGoodProperties) });
        }

        public async Task ShowStorage(Good item)
        {
            await Page.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new StorageViewViewModel(item) });
        }
    }
}
