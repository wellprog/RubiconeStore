using RubiconeStore.MyModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewInterfaces
{
    public interface ITableViewModel
    {
        string PageName { get; }
        Page Page { get; set; }
        ObservableCollection<IExecutableModel> Elements { get; }
        Task Appearing();
    }
}
