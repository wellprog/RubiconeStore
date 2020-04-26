using Newtonsoft.Json;
using RubiconeStore.DataStores;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
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
    public class UserListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Список пользователей";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }

        public Page Page { get; set; }
        private readonly HttpClient httpClient;
        private readonly SessionDataStore sessionData;

        public UserListViewModel()
        {
            httpClient = new HttpClient();
            sessionData = new SessionDataStore();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async Task Appearing()
        {
            var responce = await httpClient.GetAsync("http://rstore.kikoriki.space/UserList?AuthKey=" + sessionData.UserAuthModel.UserSession.SessionToken);
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await Page?.DisplayAlert("Ошибка запроса", "Во время запроса произошла ошибка", "Ok");
                return;
            }

            var jsonString = await responce.Content.ReadAsStringAsync();
            ResponceModel<IEnumerable<User>> model = JsonConvert.DeserializeObject<ResponceModel<IEnumerable<User>>>(jsonString);

            if (model.ErrorCode != 0)
            {
                await Page?.DisplayAlert("Ошибка запроса", model.ErrorDescription, "Ok");
                return;
            }

            foreach (var item in model.content)
            {
                new ActionModel<User>(item)
                {
                    Text = item.Login,
                    Description = item.Email,
                    ExecAction = async (user) => { await Page?.DisplayAlert("User", user.Email, "Ok"); }
                };
            }
        }
    }
}
