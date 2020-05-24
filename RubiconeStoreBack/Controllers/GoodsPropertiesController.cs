using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var properties = _store.GoodProperties.Where(f => f.GoodCategoryID == good.GoodCategoryID).ToArray();
            var values = _store.GoodPropertyValues.Where(f => f.GoodID == good.ID).ToArray();

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

        [Route("[controller]/{AuthKey}")]
        [HttpPost]
        public ResponceModel<GoodPropertyValue> SetPropertyForGood(string AuthKey, RequestModel<GoodPropertyValue> request)
        {
            var good = _store.Goods.Include(f => f.GoodCategory).Where(f => f.ID == request.Content.GoodID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<GoodPropertyValue>().RecordNotFound();

            if (good.GoodCategory == null)
                return new ResponceModel<GoodPropertyValue>().CategoryNotFound();

            var property = _store.GoodProperties.Where(f => f.ID == request.Content.GoodPropertyID).FirstOrDefault();
            if (property == null)
                return new ResponceModel<GoodPropertyValue>().RecordNotFound();

            if (property.GoodCategoryID != good.GoodCategory.ID)
                return new ResponceModel<GoodPropertyValue>().WrongPropertyCategory();

            if (request.Content.ID == 0)
            {
                _store.Add(request.Content);
                _store.SaveChanges();

                return new ResponceModel<GoodPropertyValue>() { content = request.Content };
            }
            else
            {
                var element = _store.GoodPropertyValues.Where(f => f.ID == request.Content.ID).FirstOrDefault();
                element.Value = request.Content.Value;

                _store.SaveChanges();

                return new ResponceModel<GoodPropertyValue>() { content = element };
            }
        }
    }
}
