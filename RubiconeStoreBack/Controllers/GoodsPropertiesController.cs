using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Helpers;

using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    public class GoodsPropertiesController : BaseController<GoodsPropertiesController>
    {
        public GoodsPropertiesController(ILogger<GoodsPropertiesController> logger, DbStore store, UserHelper userHelper) : base(logger, userHelper, store)
        { }

        [Route("[controller]/{AuthKey}/{ElementId}")]
        [HttpGet]
        public ResponceModel<GoodPropertiesModel> PropertiesForGood(string AuthKey, int ElementId)
        {
            var good = _store.Goods.Where(f => f.ID == ElementId).FirstOrDefault();
            if (good == null)
                return new ResponceModel<GoodPropertiesModel>().RecordNotFound();

            var properties = _store.GoodProperties.Where(f => f.GoodCategoryID == good.GoodCategoryID);
            var values = _store.GoodPropertyValues.Where(f => f.GoodID == good.ID);

            return new ResponceModel<GoodPropertiesModel>()
            {
                content = new GoodPropertiesModel()
                {
                    Good = good,
                    GoodProperties = properties,
                    GoodPropertyValues = values
                }
            };
        }

        public ResponceModel<GoodPropertyValue> SetPropertyForGood(RequestModel<GoodPropertyValue> request)
        {
            var good = _store.Goods.Where(f => f.ID == request.Content.GoodID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<GoodPropertyValue>().RecordNotFound();


        }
    }
}
