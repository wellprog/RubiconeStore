using RubiconeStore.MyModels;
using System;
using System.Collections.Generic;
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

        private List<IExecutableModel> elements = new List<IExecutableModel>();
        public IEnumerable<IExecutableModel> Elements => elements;
        public string PageName => "Главная Админ";

        private readonly INavigation navigation;

        public AdminViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            elements.Add(new ActionModel<Actions>(Actions.User) { Text = "Пользователи", ExecAction = ExecAction });
            elements.Add(new ActionModel<Actions>(Actions.Good) { Text = "Товары", ExecAction = ExecAction });
        }

        private void ExecAction(Actions a)
        {
            switch (a)
            {
                case Actions.User: /*navigation.PushAsync();*/ break;
                case Actions.Good: /*navigation.PushAsync();*/ break;
            }
        }

        public async Task Appearing()
        {
            return;
        }
    }
}
