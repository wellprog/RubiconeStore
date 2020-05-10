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
    public class GoodPropertyController : BaseController<GoodPropertyController>
    {
        public GoodPropertyController(ILogger<GoodPropertyController> logger, DbStore store, UserHelper userHelper)
            : base(logger, userHelper, store) { }

        [HttpGet]
        [Route("[controller]/{AuthKey}")]
        public ResponceModel<IEnumerable<GoodProperty>> GetAll(string AuthKey)
        {
            var responce = _userHelper.IsUserAutorized<IEnumerable<GoodProperty>>(AuthKey);
            if (responce != null) return responce;

            var resp = _store.GoodProperties; //!
            return new ResponceModel<IEnumerable<GoodProperty>> { content = resp };
        }

        [HttpPost]
        [Route("[controller]")]
        public ResponceModel<GoodProperty> StoreOne(RequestModel<GoodProperty> request) => StoreOne<GoodProperty>(request);

        [HttpPatch]
        [Route("[controller]")]
        public ResponceModel<GoodProperty> PatchOne(RequestModel<GoodProperty> request) => PatchOne<GoodProperty>(request);

        [Route("[controller]/{AuthKey}/{ElementId}")]
        [HttpDelete]
        public ResponceModel<GoodProperty> DeleteOne(string AuthKey, int ElementId) => DeleteOne<GoodProperty>(AuthKey, ElementId);
    }
}