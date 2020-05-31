﻿using RubiconeStore.Client.ViewModels;
using RubiconeStore.MyViews;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RubiconeStore.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientTabs : TabbedPage
    {
        public ClientTabs()
        {
            InitializeComponent();

            NavigationLogin.PushAsync(new LoginPage(ShowUser));
            NavigationMain.PushAsync(new SimpleTablePage() { ViewModel = new CategoryViewModel() });
        }

        private async void ShowUser()
        {
            await NavigationLogin.PushAsync(new UserDetails());
        }
    }
}