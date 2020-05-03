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
    public class GoodPropertyController
    {
        private readonly ILogger<UserListController> _logger;
        private readonly DbStore _store;
        private readonly UserHelper _userHelper;

        public GoodPropertyController(ILogger<UserListController> logger, DbStore store, UserHelper userHelper)
        {
            this._logger = logger;
            this._store = store;
            this._userHelper = userHelper;
        }

        [HttpGet]
        [Route("[controller]/{AuthKey}")]
        public ResponceModel<IEnumerable<GoodProperty>> GetAll(string AuthKey)
        {
            var responce = _userHelper.IsUserAutorized<IEnumerable<GoodProperty>>(AuthKey);
            if (responce != null) return responce;

            var resp = _store.GoodProperties; //!
            return new ResponceModel<IEnumerable<GoodProperty>> { content = resp };
        }
    }
}