using RubiconeStore.DataStores;
using RubiconeStore.Helpers;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    class EditGoodPropertyViewModel : BaseViewModel
    {
        private readonly GoodCategory category;
        private readonly GoodProperty property;
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;
        private readonly Page page;

        public EditGoodPropertyViewModel(GoodCategory category, GoodProperty property, HttpClient httpClient, Page page)
        {
            this.category = category;
            this.property = property;
            requestHelper = new RequestHelper(httpClient);
            sessionData = new SessionDataStore();
            SaveCommand = new Command(SaveProperty, CanSave);
            this.page = page;
        }

        public string Name { get { return property.Name; } set { property.Name = value; OnPropertyChanged(); } }
        public string Description { get { return property.Description; } set { property.Description = value; OnPropertyChanged(); } }
        public string DefaultValue { get { return property.DefaultValue; } set { property.DefaultValue = value; OnPropertyChanged(); } }

        public Command SaveCommand { get; }

        public bool CanSave() => property.IsModelRight();

        public async void SaveProperty()
        {
            if (category.ID == 0)
                await requestHelper.Post<Good, RequestModel<GoodCategory>>("http://rstore.kikoriki.space/GoodCategory", new RequestModel<GoodCategory>()
                {
                    Content = category,
                    AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
                });
            else
                await requestHelper.Patch<Good, RequestModel<GoodCategory>>("http://rstore.kikoriki.space/GoodCategory", new RequestModel<GoodCategory>()
                {
                    Content = category,
                    AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
                });

            await page.Navigation.PopAsync();
        }

        public async Task Appearing()
        {
            //var items = await requestHelper.Get<IEnumerable<GoodCategory>>($"http://rstore.kikoriki.space/GoodCategory/{ sessionData.SessionToken }");

            //Categories.Clear();
            //foreach (var item in items)
            //    Categories.Add(item);

            //GoodCategoryIndex = Categories.IndexOf(Categories.Where(f => f.ID == good.GoodCategoryID).FirstOrDefault());
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            SaveCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }


    }
}
