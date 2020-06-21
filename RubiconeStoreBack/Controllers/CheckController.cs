using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Helpers;
using Shared.Model;
using Shared.Helpers;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Builder;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    //Класс для контролирования корзины пользователя
    public class CheckController : BaseController<CheckController>
    {
        public CheckController(ILogger<CheckController> logger, DbStore store, UserHelper userHelper)
            : base(logger, userHelper, store) { }


        [Route("[controller]/{AuthKey}")]
        [HttpGet]
        public ResponceModel<IEnumerable<Check>> GetHistory(string AuthKey)
        {
            var history = _store.Checks.Where(f => f.UserID == _user.ID && f.IsDone)
                                    .Include(f => f.Sells)
                                    .ThenInclude(f => f.Storage)
                                    .ThenInclude(f => f.Good);

            return new ResponceModel<IEnumerable<Check>> { content = history };
        }
    }
}
