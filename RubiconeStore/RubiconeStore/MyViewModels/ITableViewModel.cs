using RubiconeStore.MyModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubiconeStore.MyViewModels
{
    public interface ITableViewModel
    {
        string PageName { get; }
        IEnumerable<IExecutableModel> Elements { get; }
        Task Appearing();
    }
}
