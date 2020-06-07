using RubiconeStore.DataStores;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViews;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class AdminViewModel : BaseViewModel, ITableViewModel
    {

        private enum Actions
        {
            User,
            Good,
            Category
        }

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; } = new ToolbarItem[0];
        public string PageName => "Главная Админ";

        public Page Page { get; set; }

        private readonly SessionDataStore sessionData = new SessionDataStore();

        public AdminViewModel()
        {
            Elements = new ObservableCollection<IExecutableModel>();
        }

        private async Task ExecAction(Actions a)
        {
            switch (a)
            {
                case Actions.User: await Page?.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new UserListViewModel() }); break;
                case Actions.Good: await Page?.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new GoodListViewModel() }); break;
                case Actions.Category: await Page?.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new GoodCategoryListViewModel() }); break;
                default: await Page.DisplayAlert("Ошибка", "Такого акшена пока нет", "Ok"); break;
            }

            return;
        }

        public async Task Appearing()
        {
            Elements.Clear();

            if (sessionData.SessionToken == null)
                return;

            Elements.Add(new ActionModel<Actions>(Actions.User) { Text = "Пользователи", ExecAction = ExecAction });
            Elements.Add(new ActionModel<Actions>(Actions.Category) { Text = "Категории товаров", ExecAction = ExecAction });
            Elements.Add(new ActionModel<Actions>(Actions.Good) { Text = "Товары", ExecAction = ExecAction });
        }
    }
}
