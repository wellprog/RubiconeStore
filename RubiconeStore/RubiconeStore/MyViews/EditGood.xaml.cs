﻿using RubiconeStore.MyViewModels;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RubiconeStore.MyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditGood : ContentPage
    {
        private readonly EditGoodViewModel model;
        public EditGood() : this(new Good()) { }

        public EditGood(Good good)
        {
            InitializeComponent();
            model = new EditGoodViewModel(good, new HttpClient(), this);
            BindingContext = model;
        }

        protected async override void OnAppearing()
        {
            await model.Appearing();
            base.OnAppearing();
        }
    }
}