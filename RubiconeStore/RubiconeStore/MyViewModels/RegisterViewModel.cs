using Newtonsoft.Json;
using RubiconeStore.DataStores;
using Shared.Model;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private readonly User _user;
        private readonly HttpClient _httpClient;
        private readonly Page _page;
        private readonly SessionDataStore _sessionDataStore;

        public string Login { get
            {
                return _user.Login;
            } set
            {
                _user.Login = value;
                OnPropertyChanged();
            } 
        }

        public string Email
        {
            get
            {
                return _user.Email;
            }
            set
            {
                _user.Email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return _user.Password;
            }
            set
            {
                _user.Password = value;
                OnPropertyChanged();
            }
        }

        private string _password1;
        public string Password1
        {
            get
            {
                return _password1;
            }
            set
            {
                _password1 = value;
                OnPropertyChanged();
            }
        }

        public Command RegisterCommand { get; private set; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            RegisterCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }

        public RegisterViewModel(Page page)
        {
            _user = new User();
            RegisterCommand = new Command(RegisterIt, CanRegister);
            _httpClient = new HttpClient();
            _page = page;
            _sessionDataStore = new SessionDataStore();
        }

        async void RegisterIt()
        {
            var stringPayload = JsonConvert.SerializeObject(_user);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var responce = await _httpClient.PostAsync("http://rstore.kikoriki.space/User", content);

            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await _page.DisplayAlert("Ошибка запроса", "Во время запроса произошла ошибка", "Ok");
                return;
            }

            var jsonString = await responce.Content.ReadAsStringAsync();
            ResponceModel<UserAuthModel> model = JsonConvert.DeserializeObject<ResponceModel<UserAuthModel>>(jsonString);

            if (model.ErrorCode != 0)
            {
                await _page.DisplayAlert("Ошибка запроса", model.ErrorDescription, "Ok");
                return;
            }

            await _page.Navigation.PopModalAsync();
        }

        bool CanRegister()
        {
            return _user.IsModelRight() && Password == Password1;
        }

    }
}
