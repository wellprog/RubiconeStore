using Newtonsoft.Json;
using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class UserListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Список пользователей";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; } = new ToolbarItem[0];

        public Page Page { get; set; }
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;

        public UserListViewModel()
        {
            requestHelper = new RequestHelper();
            sessionData = new SessionDataStore();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async Task Appearing()
        {
            IEnumerable<User> model = await requestHelper.Get<IEnumerable<User>>($"http://rstore.kikoriki.space/UserList/{ sessionData.SessionToken }");

            foreach (var item in model)
            {
                Elements.Add(
                    new ActionModel<User>(item)
                    {
                        Text = item.Login,
                        Description = item.Email,
                        ExecAction = async (user) => { await Page?.DisplayAlert("User", user.Email, "Ok"); }
                    }
                );
            }
        }
    }
}
