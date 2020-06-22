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


        [Route("[controller]/{UserID}")]
        [HttpGet]
        public ResponceModel<IEnumerable<Check>> GetHistory(int UserID)
        {
            User gettedUser = _store.Users.Where(f => f.ID == UserID).First();
            if(gettedUser == null)
                return new ResponceModel<IEnumerable<Check>>().WrongAuthKey();

            var history = gettedUser.Checks?.Where(f => f.IsDone);

            return new ResponceModel<IEnumerable<Check>> { content = history };
        }
    }
}
