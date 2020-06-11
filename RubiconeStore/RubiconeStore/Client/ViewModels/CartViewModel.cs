﻿using RubiconeStore.DataStores;
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
    public class CartViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Корзина";

        public Page Page { get; set; }

        public ObservableCollection<IExecutableModel> Elements { get; } = new ObservableCollection<IExecutableModel>();

        public IEnumerable<ToolbarItem> ToolbarItems { get; } = new ToolbarItem[0];

        private readonly RequestHelper requestHelper = new RequestHelper();
        private readonly SessionDataStore sessionData = new SessionDataStore();


        public async Task Appearing()
        {
            if (sessionData.SessionToken == null)
                return;

            var cart = await requestHelper.Get<CartModel>($"http://rstore.kikoriki.space/Cart/{ sessionData.SessionToken }");

            Elements.Clear();
            foreach (var item in cart.CartItems)
            {
                var cartItem = new ActionModel<CartItemModel>(item)
                {
                    Text = item.Good.Title,
                    Description = item.Count.ToString(),
                    ExecAction = async f => await Page.Navigation.PushAsync(new EditCartItem(item))
                };

                cartItem.AddLeftSwipe("Delete", Color.Red, new Command(async f => await DeleteCartItem(f as CartItemModel)));

                Elements.Add(cartItem);
            }
        }
        public async Task DeleteCartItem(CartItemModel cartItem)
        {
            await requestHelper.Delete<CartItemModel>($"http://rstore.kikoriki.space/Cart/{ sessionData.SessionToken }/{ cartItem.Good.ID }");

            await Page.DisplayAlert("Delete Good success!", cartItem.Good.Title, "Ok");
        }
    }
}
