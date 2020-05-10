using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Helpers;

using Shared.Helpers;
using Shared.Model;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    public class GoodController : BaseController<GoodController>
    {
        public GoodController(ILogger<GoodController> logger, DbStore store, UserHelper userHelper) : base(logger ,userHelper, store)
        { }

        [Route("[controller]")]
        [HttpPost]
        public ResponceModel<Good> StoreOne(RequestModel<Good> request) => StoreOne<Good>(request);

        [Route("[controller]")]
        [HttpPatch]
        public ResponceModel<Good> PatchOne(RequestModel<Good> request) => PatchOne<Good>(request);

        [Route("[controller]/{AuthKey}/{ElementId}")]
        [HttpDelete]
        public ResponceModel<Good> DeleteOne(string AuthKey, int ElementId) => DeleteOne<Good>(AuthKey, ElementId);
    }
}
