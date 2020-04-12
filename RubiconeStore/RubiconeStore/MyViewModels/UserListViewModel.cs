using RubiconeStore.MyModels;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class UserListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "Список пользователей";

        private List<IExecutableModel> elements = new List<IExecutableModel>();
        public IEnumerable<IExecutableModel> Elements => throw new NotImplementedException();

        private readonly INavigation navigation;
        private readonly HttpClient httpClient;

        public UserListViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            this.httpClient = new HttpClient();
        }

        public async Task Appearing()
        {
            var elements = await httpClient.GetAsync("http://www.ru");
        }
    }
}
