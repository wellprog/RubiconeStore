using Newtonsoft.Json;

using RubiconeStore.DataStores;
using RubiconeStore.MyViews;

using Shared.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace RubiconeStore.MyViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient httpClient;
        private readonly SessionDataStore _sessionDataStore;
        private readonly Page page;

        private string login = "";
        public string Login { 
            get
            {
                return login;
            }
            set
            {
                SetProperty(ref login, value);
            }
        }

        private string password = "";
        public string Password { 
            get
            {
                return password;
            }
            set
            {
                SetProperty(ref password, value);
            }
        }

        public Command LoginCommand { get; private set; }
        public Command RegisterCommand { get; private set; }

        public LoginViewModel(Page page)
        {
            httpClient = new HttpClient();
            this.page = page;
            _sessionDataStore = new SessionDataStore();

            LoginCommand = new Command(LoginMe, () => !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password));
            RegisterCommand = new Command(async () => await page.Navigation.PushModalAsync(new RegisterPage()));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            LoginCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }

        public async void LoginMe()
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                await page.DisplayAlert("Ошибка", "Имя пользователя и пароль должны быть заполнены", "Ok");
                return;
            }

            var responce = await httpClient.GetAsync("http://rstore.kikoriki.space/User?email=" + HttpUtility.UrlEncode(login) + "&password=" + HttpUtility.UrlEncode(password));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await page.DisplayAlert("Ошибка запроса", "Во время запроса произошла ошибка", "Ok");
                return;
            }

            var jsonString = await responce.Content.ReadAsStringAsync();
            ResponceModel<UserAuthModel> model = JsonConvert.DeserializeObject<ResponceModel<UserAuthModel>>(jsonString);
 
            if (model.ErrorCode != 0)
            {
                await page.DisplayAlert("Ошибка запроса", model.ErrorDescription, "Ok");
                return;
            }


            _sessionDataStore.UserAuthModel = model.content;

            ShowNext();
        }

        public async void ShowNext()
        {
            await page.Navigation.PushAsync(new SimpleTablePage
            {
                ViewModel = new AdminViewModel()
            });
        }
    }
}
