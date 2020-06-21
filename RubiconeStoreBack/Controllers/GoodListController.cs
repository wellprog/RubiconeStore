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
        [Route("[controller]/{AuthKey}")]
        public ResponceModel<IEnumerable<Good>> GetAll(string AuthKey)
        {
            var responce = _userHelper.IsUserAutorized<IEnumerable<Good>>(AuthKey);
            if (responce != null) return responce;

            var resp = _store.Goods;
            return new ResponceModel<IEnumerable<Good>> { content = resp };
        }

        [HttpGet]
        [Route("[controller]/{AuthKey}/{CategoryId}")]
        public ResponceModel<IEnumerable<GoodCount>> GetAllByCategory(string AuthKey, int CategoryId)
        {
            #region Old Shit
            //var responce = _userHelper.IsUserAutorized<IEnumerable<Good>>(AuthKey);
            //if (responce != null) return responce;

            //var resp = _store.Goods.Where(f => f.GoodCategoryID == CategoryId);
            //return new ResponceModel<IEnumerable<Good>> { content = resp };
            #endregion
            #region New Shit
            var responce = _userHelper.IsUserAutorized<IEnumerable<GoodCount>>(AuthKey);
            if (responce != null) return responce;

            Good[] goods = _store.Goods.Where(f => f.GoodCategoryID == CategoryId).ToArray();
            int[] counts = new int[goods.Length];

            GoodCount[] resp = new GoodCount[goods.Length];

            for (int i = 0; i < goods.Count(); i++)
            {
                int count = (_store.Storages.Where(f => f.GoodID == goods[i].ID) as Storage).Count;
                resp.Append(new GoodCount(goods[i], count));
            }
           
            return new ResponceModel<IEnumerable<GoodCount>> { content = resp };
            #endregion
        }
    }
}
