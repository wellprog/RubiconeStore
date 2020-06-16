using RubiconeStore.DataStores;
using RubiconeStore.Helpers;
using RubiconeStore.MyModels;
using RubiconeStore.MyViewInterfaces;
using RubiconeStore.MyViewModels;
using RubiconeStore.MyViews;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;

namespace RubiconeStore.Client.ViewModels
{
    public class CheckListViewModel : BaseViewModel, ITableViewModel
    {
        public string PageName => "История покупок";

        public ObservableCollection<IExecutableModel> Elements { get; private set; }
        public IEnumerable<ToolbarItem> ToolbarItems { get; }

        public Page Page { get; set; }
        private readonly SessionDataStore sessionData;
        private readonly RequestHelper requestHelper;

        public CheckListViewModel()
        {
            sessionData = new SessionDataStore();

            requestHelper = new RequestHelper();

            Elements = new ObservableCollection<IExecutableModel>();
        }

        public async Task Appearing()
        {
            var items = await requestHelper.Get<IEnumerable<Check>>($"http://rstore.kikoriki.space/Check/{ sessionData.SessionToken }");

            Elements.Clear();
            foreach (var item in items)
            {
                var good = new ActionModel<Check>(item)
                {
                    Text = "Номер чека: " + item.ID.ToString(),
                    Description = item.CreatedDate.ToString(),
                    ExecAction = async f => await Page.Navigation.PushAsync(new SimpleTablePage() { ViewModel = new CheckViewModel(item) })
                };

                Elements.Add(good);
            }
        }
    }
}
