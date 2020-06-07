using RubiconeStore.DataStores;
using RubiconeStore.Helpers;

using Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class EditCartItemViewModel : BaseViewModel
    {
        private readonly CartItemModel cartItem;
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;
        private readonly Page page;
        private int startCount;
        public EditCartItemViewModel(CartItemModel cartItem, HttpClient httpClient, Page page)
        {
            this.cartItem = cartItem;
            requestHelper = new RequestHelper(httpClient);
            sessionData = new SessionDataStore();
            SaveCommand = new Command(SaveCartItem, CanSave);
            startCount = cartItem.Count;
            this.page = page;
        }

        public string Title
        {
            get
            {
                return cartItem.Good.Title;
            }
        }

        public int Count
        {
            get
            {
                return cartItem.Count;
            }
            set
            {
                cartItem.Count = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand { get; }

        public bool CanSave()
        {
            return (cartItem.Count != 0 && cartItem.Count != startCount);
        }

        public async void SaveCartItem()
        {
            await requestHelper.Patch<CartItemModel, RequestModel<CartItemModel>>("http://rstore.kikoriki.space/Cart", new RequestModel<CartItemModel>()
            {
                Content = cartItem,
                AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
            });

            await page.Navigation.PopAsync();
        }

        public async Task Appearing()
        {
           
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            SaveCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }
    }
}
