using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("[controller]")]
    public class GoodListController : ControllerBase
    {
        private readonly ILogger<UserListController> _logger;
        private readonly DbStore _store;
        private readonly UserHelper _userHelper;

        public GoodListController (ILogger<UserListController> logger, DbStore store, UserHelper userHelper)
        {
            this._logger = logger;
            this._store = store;
            this._userHelper = userHelper;
        }
        
        [HttpGet]
        public ResponceModel<IEnumerable<Good>> GetAll(string AuthKey)
        {
            var responce = _userHelper.IsUserAutorized<IEnumerable<Good>>(AuthKey);
            if (responce != null) return responce;

            var resp = _store.Goods.Include(f => f.GoodCategory).Include(f => f.GoodPropertyValues).ThenInclude(f => f.GoodProperty);
            return new ResponceModel<IEnumerable<Good>> { content = resp };
        }
    }
}
