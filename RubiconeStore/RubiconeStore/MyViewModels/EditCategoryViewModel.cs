using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using Shared.Model;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class EditCategoryViewModel : BaseViewModel
    {
        private readonly GoodCategory category;
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;
        private readonly Page page;

        public EditCategoryViewModel(GoodCategory category, HttpClient httpClient, Page page)
        {
            this.category = category;
            requestHelper = new RequestHelper(httpClient);
            sessionData = new SessionDataStore();
            SaveCommand = new Command(SaveCategory, CanSave);
            this.page = page;
        }

        public string Name
        {
            get
            {
                return category.Name;
            }
            set
            {
                category.Name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return category.Description;
            }
            set
            {
                category.Description = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand { get; }

        public bool CanSave() => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Description);

        public async void SaveCategory()
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
    }
}
