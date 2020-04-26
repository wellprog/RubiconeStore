using RubiconeStore.DataStores;
using RubiconeStore.Helpers;

using Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class EditGoodViewModel : BaseViewModel
    {
        private readonly Good good;
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;
        private readonly Page page;

        public EditGoodViewModel(Good good, HttpClient httpClient, Page page)
        {
            this.good = good;
            requestHelper = new RequestHelper(httpClient);
            sessionData = new SessionDataStore();
            SaveCommand = new Command(SaveGood, CanSave);
            this.page = page;
        }

        public string Text
        {
            get
            {
                return good.Text;
            }
            set
            {
                good.Text = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return good.Title;
            }
            set
            {
                good.Title = value;
                OnPropertyChanged();
            }
        }

        public int Price
        {
            get
            {
                return good.Price;
            }
            set
            {
                good.Price = value;
                OnPropertyChanged();
            }
        }

        public int GoodCategoryID
        {
            get
            {
                return good.GoodCategoryID;
            }
            set
            {
                good.GoodCategoryID = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GoodCategory> Categories { get; } = new ObservableCollection<GoodCategory>();


        public Command SaveCommand { get; }

        public bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Text) && !string.IsNullOrWhiteSpace(Title) && Price > 0;
        }

        public async void SaveGood()
        {
            await requestHelper.Post<Good, RequestModel<Good>>("http://rstore.kikoriki.space/Good", new Dictionary<string, object>(), new RequestModel<Good>() { 
                Content = good,
                AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
            });

            await page.Navigation.PopAsync();
        }

        public async Task Appearing()
        {
            var items = await requestHelper.Get<IEnumerable<GoodCategory>>("http://rstore.kikoriki.space/GoodCategory", new Dictionary<string, object>
            {
                { "AuthKey", sessionData.UserAuthModel.UserSession.SessionToken }
            });


            foreach (var item in items)
                Categories.Add(item);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            SaveCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }
    }
}
