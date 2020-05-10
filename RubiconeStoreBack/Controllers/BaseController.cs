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
using Shared.Helpers;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace RubiconeStoreBack.Controllers
{
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly ILogger<T> _logger;
        protected readonly UserHelper _userHelper;
        protected readonly DbStore _store;

        protected BaseController(ILogger<T> logger, UserHelper userHelper, DbStore store)
        {
            _userHelper = userHelper;
            _logger = logger;
            _store = store;
        }

        protected ResponceModel<T> CheckRequest<T>(RequestModel<T> request) where T : IValidate
        {
            var responce = _userHelper.IsUserAutorized<T>(request.AuthKey);
            if (responce != null) return responce;


            if (!request.Content.IsModelRight())
                return new ResponceModel<T>().FieldEmptyError();

            return null;
        }

        public ResponceModel<Req> StoreOne<Req>(RequestModel<Req> request) where Req : IValidate
        {
            var responce = CheckRequest(request);
            if (responce != null) return responce;

            _store.Add(request.Content);
            _store.SaveChanges();

            return new ResponceModel<Req> { content = request.Content };
        }

        public ResponceModel<Req> PatchOne<Req>(RequestModel<Req> request) where Req : class, IValidate, IPK
        {
            var responce = CheckRequest(request);
            if (responce != null) return responce;

            var element = _store.Find<Req>(request.Content.ID);
            if (element == null)
                return new ResponceModel<Req>().RecordNotFound();

            element.CopyAllFrom(request.Content);

            _store.SaveChanges();

            return new ResponceModel<Req> { content = element };
        }

        public ResponceModel<Req> DeleteOne<Req>(string authKey, int id) where Req : class, IValidate, IPK
        {
            var responce = _userHelper.IsUserAutorized<Req>(authKey);
            if (responce != null) return responce;

            var element = _store.Find<Req>(id);
            if (element == null)
                return new ResponceModel<Req>().RecordNotFound();

            _store.Remove(element);
            _store.SaveChanges();

            return new ResponceModel<Req> { content = element };
        }



    }
}
