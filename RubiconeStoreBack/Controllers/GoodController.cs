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

using Shared.Helpers;
using Shared.Model;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodController : ControllerBase
    {
        private readonly ILogger<GoodController> _logger;
        private readonly DbStore _store;
        private readonly UserHelper _userHelper;

        public GoodController(ILogger<GoodController> logger, DbStore store, UserHelper userHelper)
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

        [HttpPost]
        public ResponceModel<Good> StoreOne(RequestModel<Good> request)
        {
            var responce = CheckGoodRequest(request);
            if (responce != null) return responce;

            _store.Add(request.Content);
            _store.SaveChanges();

            return new ResponceModel<Good> { content = request.Content };
        }

        [HttpPatch]
        public ResponceModel<Good> PatchOne(RequestModel<Good> request)
        {
            var responce = CheckGoodRequest(request);
            if (responce != null) return responce;

            var good = _store.Goods.Where(f => f.ID == request.Content.ID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<Good>().RecordNotFound();

            good.CopyAllFrom(request.Content);

            _store.SaveChanges();

            return new ResponceModel<Good> { content = good };
        }

        [HttpDelete]
        public ResponceModel<Good> DeleteOne(RequestModel<Good> request)
        {
            var responce = CheckGoodRequest(request);
            if (responce != null) return responce;

            var good = _store.Goods.Where(f => f.ID == request.Content.ID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<Good>().RecordNotFound();

            _store.Remove(good);
            _store.SaveChanges();

            return new ResponceModel<Good> { content = request.Content };
        }

        private ResponceModel<Good> CheckGoodRequest(RequestModel<Good> request)
        {
            var responce = _userHelper.IsUserAutorized<Good>(request.AuthKey);
            if (responce != null) return responce;


            if (!request.Content.IsModelRight())
                return new ResponceModel<Good>().FieldEmptyError();

            return null;
        }

    }
}
