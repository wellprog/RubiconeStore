﻿using Newtonsoft.Json;

using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
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
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore _sessionDataStore;
        private readonly Page Page;
        public Action NextAction { get; set; }

        private string loginOrEmail = "";
        public string LoginOrEmail
        {
            get
            {
                return loginOrEmail;
            }
            set
            {
                SetProperty(ref loginOrEmail, value);
            }
        }

        private string password = "";
        public string Password
        {
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
            requestHelper = new RequestHelper();
            this.Page = page;
            _sessionDataStore = new SessionDataStore();

            LoginCommand = new Command(LoginMe, () => !string.IsNullOrEmpty(LoginOrEmail) && !string.IsNullOrEmpty(Password));
            RegisterCommand = new Command(async () => await page.Navigation.PushModalAsync(new RegisterPage()));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            LoginCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }

        public async void LoginMe()
        {
            if (string.IsNullOrEmpty(loginOrEmail) || string.IsNullOrEmpty(password))
            {
                await Page.DisplayAlert("Ошибка!", "Имя пользователя и пароль должны быть заполнены", "Ok");
                return;
            }

            ResponceModel<UserAuthModel> responce = await requestHelper.GetWithResponce<UserAuthModel>("http://rstore.kikoriki.space/User", new Dictionary<string, object>
            {
                { "loginOrEmail", loginOrEmail },
                { "password", password }
            });

            if (responce.ErrorCode == 0)
            {
                _sessionDataStore.UserAuthModel = responce.content;

                ShowNext();
            }
            else
                await Page.DisplayAlert("Ошибка!", responce.ErrorDescription, "Ok");
        }

        public void ShowNext()
        {
            NextAction?.Invoke();
        }
    }
}
