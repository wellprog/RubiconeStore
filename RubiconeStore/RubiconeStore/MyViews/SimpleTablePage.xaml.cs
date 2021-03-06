﻿using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RubiconeStore.MyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleTablePage : ContentPage
    {
        private ITableViewModel _viewModel;
        public ITableViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                _viewModel.Page = this;
                BindingContext = _viewModel;
                setupButtons();
            }
        }

        public SimpleTablePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel?.Appearing();
        }

        private void setupButtons()
        {
            foreach (var item in _viewModel.ToolbarItems)
                ToolbarItems.Add(item);
        }
    }
}