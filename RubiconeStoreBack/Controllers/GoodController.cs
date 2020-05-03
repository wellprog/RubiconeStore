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
    [Route("[controller]")]
    public class GoodController : BaseController<GoodController>
    {
        public GoodController(ILogger<GoodController> logger, DbStore store, UserHelper userHelper) : base(logger ,userHelper, store)
        { }

        [HttpPost]
        public ResponceModel<Good> StoreOne(RequestModel<Good> request) => StoreOne<Good>(request);

        [HttpPatch]
        public ResponceModel<Good> PatchOne(RequestModel<Good> request) => PatchOne<Good>(request);

        [HttpDelete]
        public ResponceModel<Good> DeleteOne(RequestModel<Good> request) => DeleteOne<Good>(request);
    }
}
