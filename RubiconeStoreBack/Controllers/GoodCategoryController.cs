using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Helpers;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    public class GoodCategoryController : BaseController<GoodCategoryController>
    {
        public GoodCategoryController(ILogger<GoodCategoryController> logger, DbStore store, UserHelper userHelper)
                                    : base(logger, userHelper, store) { }

        [HttpGet]
        [Route("[controller]/{AuthKey}")]
        public ResponceModel<IEnumerable<GoodCategory>> GetAll(string AuthKey)
        {
            var responce = _userHelper.IsUserAutorized<IEnumerable<GoodCategory>>(AuthKey);
            if (responce != null) return responce;

            var resp = _store.GoodCategories; //!
            return new ResponceModel<IEnumerable<GoodCategory>> { content = resp };
        }

        [HttpPost]
        [Route("[controller]")]
        public ResponceModel<GoodCategory> StoreOne(RequestModel<GoodCategory> request) => StoreOne<GoodCategory>(request);

        [HttpPatch]
        [Route("[controller]")]
        public ResponceModel<GoodCategory> PatchOne(RequestModel<GoodCategory> request) => PatchOne<GoodCategory>(request);

        [HttpDelete]
        [Route("[controller]")]
        public ResponceModel<GoodCategory> DeleteOne(RequestModel<GoodCategory> request) => DeleteOne<GoodCategory>(request);


    }
}
