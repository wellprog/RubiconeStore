using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public ResponceModel<IEnumerable<GoodProperty>> GetAll(string AuthKey)
        {
            var responce = _userHelper.IsUserAutorized<IEnumerable<GoodProperty>>(AuthKey);
            if (responce != null) return responce;

            var resp = _store.GoodProperties; //!
            return new ResponceModel<IEnumerable<GoodProperty>> { content = resp };
        }
    }
}