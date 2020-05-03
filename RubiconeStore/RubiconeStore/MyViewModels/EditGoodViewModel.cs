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

        private int _goodCategoryIndex = 0;
        public int GoodCategoryIndex
        {
            get
            {
                return _goodCategoryIndex;
            }
            set
            {
                _goodCategoryIndex = value;
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
            good.GoodCategoryID = Categories[GoodCategoryIndex].ID;

            if (good.ID == 0)
                await requestHelper.Post<Good, RequestModel<Good>>("http://rstore.kikoriki.space/Good", new RequestModel<Good>()
                {
                    Content = good,
                    AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
                });
            else
                await requestHelper.Patch<Good, RequestModel<Good>>("http://rstore.kikoriki.space/Good", new RequestModel<Good>()
                {
                    Content = good,
                    AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
                });

            await page.Navigation.PopAsync();
        }

        public async Task Appearing()
        {
            var items = await requestHelper.Get<IEnumerable<GoodCategory>>($"http://rstore.kikoriki.space/GoodCategory/{ sessionData.SessionToken }");

            Categories.Clear();
            foreach (var item in items)
                Categories.Add(item);

            GoodCategoryIndex = Categories.IndexOf(Categories.Where(f => f.ID == good.GoodCategoryID).FirstOrDefault());
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            SaveCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }
    }
}
