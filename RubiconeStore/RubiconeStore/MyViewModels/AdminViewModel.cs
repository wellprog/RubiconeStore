using RubiconeStore.MyModels;
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
            Good
        }

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public string PageName => "Главная Админ";

        public Page Page { get; set; }

        public AdminViewModel()
        {
            Elements = new ObservableCollection<IExecutableModel>();

            Elements.Add(new ActionModel<Actions>(Actions.User) { Text = "Пользователи", ExecAction = ExecAction });
            Elements.Add(new ActionModel<Actions>(Actions.Good) { Text = "Товары", ExecAction = ExecAction });
        }

        private async Task ExecAction(Actions a)
        {
            switch (a)
            {
                case Actions.User: await Page?.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new UserListViewModel() }); break;
                case Actions.Good: /*navigation.PushAsync();*/ break;
            }

            return;
        }

        public async Task Appearing()
        {
            return;
        }
    }
}
