using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using Shared.Model;

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
    public partial class GoodDetails : ContentPage
    {
        private readonly Good good;
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;
        private GoodPropertiesModel itemGoodProperties;


        public GoodDetails(Good good)
        {
            sessionData = new SessionDataStore();
            requestHelper = new RequestHelper();
            this.good = good;

            InitializeComponent();
            BindingContext = good;

            AddToCart.Command = new Command(AddToCartDelegate);
        }

        private void DrawAll()
        {
            Params.Children.Clear();
            foreach (var property in itemGoodProperties.GoodProperties)
            {
                StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label name = new Label() { Text = property.Name, WidthRequest = 100, Margin = new Thickness(20, 0, 0, 0) };
                var pValue = itemGoodProperties.GoodPropertyValues.Where(f => f.GoodPropertyID == property.ID).FirstOrDefault();
                Label value = new Label() { Text = pValue == null ? "Не задано" : pValue.Value };

                stackLayout.Children.Add(name);
                stackLayout.Children.Add(value);

                Params.Children.Add(stackLayout);
            }
        }

        protected override async void OnAppearing()
        {
            itemGoodProperties = await requestHelper.Get<GoodPropertiesModel>($"http://rstore.kikoriki.space/GoodsProperties/{ sessionData.SessionToken }/{ good.ID }");
            DrawAll();
        }

        private async void AddToCartDelegate()
        {
            int count = 0;

            if (int.TryParse(ItemCount.Text, out count))
            {
                await requestHelper.Post<CartItemModel, RequestModel<CartItemModel>>("http://rstore.kikoriki.space/Cart", new RequestModel<CartItemModel>
                {
                    AuthKey = sessionData.SessionToken,
                    Content = new CartItemModel
                    {
                        Good = good,
                        Count = count
                    }
                });

                await DisplayAlert("Добавление в корзину", $"Добавление в корзину товара { good.Title } прошло успешно", "Ok");
            } else
            {
                await DisplayAlert("Ошибка", $"Неверное количество товара { good.Title }", "Ok");
            }
        }
    }
}