using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.Logging;
using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Helpers;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    public class StoreController : BaseController<StoreController>
    {
        public StoreController(ILogger<StoreController> logger, DbStore store, UserHelper userHelper)
            : base(logger, userHelper, store) { }


        [Route("[controller]/{AuthKey}/{ElementId}")]
        [HttpGet]
        public ResponceModel<IEnumerable<Storage>> StoresForGood(string AuthKey, int ElementId)
        {
            //Проверяем запрос
            var responce = CheckRequest<IEnumerable<Storage>>(AuthKey);
            if (responce != null)
                return responce;

            return new ResponceModel<IEnumerable<Storage>>() { content = _store.Storages.Where(f => f.GoodID == ElementId) };
        }
        
        [Route("[controller]")]
        [HttpPost]
        public ResponceModel<Storage> StoreOne(RequestModel<Storage> request) => base.StoreOne(request);
    }
}
