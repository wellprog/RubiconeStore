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
            if (property.ID == 0)
                await requestHelper.Post<GoodProperty, RequestModel<GoodProperty>>("http://rstore.kikoriki.space/GoodProperty", new RequestModel<GoodProperty>()
                {
                    Content = property,
                    AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
                });
            else
                await requestHelper.Patch<GoodProperty, RequestModel<GoodProperty>>("http://rstore.kikoriki.space/GoodProperty", new RequestModel<GoodProperty>()
                {
                    Content = property,
                    AuthKey = sessionData.UserAuthModel.UserSession.SessionToken
                });

            await page.Navigation.PopAsync();
        }

        public async Task Appearing() { }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            SaveCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }


    }
}
