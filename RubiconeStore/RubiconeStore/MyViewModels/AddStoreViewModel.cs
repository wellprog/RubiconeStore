using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using Shared.Model;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class AddStoreViewModel : BaseViewModel
    {
        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _price;
        public int Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private readonly RequestHelper requestHelper = new RequestHelper();
        private readonly SessionDataStore sessionData = new SessionDataStore();
        private readonly Page page;
        private readonly Good good;

        public AddStoreViewModel(Good good, Page page)
        {
            this.good = good;
            this.page = page;
            SaveCommand = new Command(SaveStore, CanSave);
        }

        public Command SaveCommand { get; }
        public bool CanSave() => Count > 0 && Price > 0;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            SaveCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }

        public async void SaveStore()
        {
            await requestHelper.Post<Storage, RequestModel<Storage>>("http://rstore.kikoriki.space/Store", new RequestModel<Storage>()
            {
                Content = new Storage
                {
                    GoodID = good.ID,
                    Count = Count,
                    Price = Price
                },
                AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
            });

            await page.Navigation.PopAsync();
        }


    }
}
