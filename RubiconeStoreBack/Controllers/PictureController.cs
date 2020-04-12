using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Helpers;

using Shared.Model;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly ILogger<PictureController> _logger;
        private readonly DbStore _store;
        private readonly UserHelper _userHelper;

        public PictureController(ILogger<PictureController> logger, DbStore store, UserHelper userHelper)
        {
            this._logger = logger;
            this._store = store;
            this._userHelper = userHelper;
        }

        [HttpPost]
        public ResponceModel<string> AddPicture(RequestModel<PhotoModel<Good>> request)
        {
            var responce = _userHelper.IsUserAutorized<string>(request.AuthKey);
            if (responce != null) return responce;

            if (request.Content.Photo.Length == 0)
                return new ResponceModel<string>().FieldNotPass(nameof(request.Content.Photo));

            var good = _store.Goods.Where(f => f.ID == request.Content.Content.ID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<string>().RecordNotFound();

            string fileName = $"{Guid.NewGuid()}.{request.Content.PhotoType}";

            using (var fs = new FileStream($"img/{fileName}", FileMode.CreateNew))
            using (var ms = new MemoryStream(request.Content.Photo))
                ms.CopyTo(fs);

            good.AddPicture(fileName);
            _store.SaveChanges();

            return new ResponceModel<string> { content = fileName };
        }

        [HttpDelete]
        public ResponceModel<string> DeletePicture(RequestModel<PhotoModel<Good>> request)
        {
            var responce = _userHelper.IsUserAutorized<string>(request.AuthKey);
            if (responce != null) return responce;


            var good = _store.Goods.Where(f => f.ID == request.Content.Content.ID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<string>().RecordNotFound();

            if (!good.hasPicture(request.Content.FileName))
                return new ResponceModel<string>().RecordNotFound();

            good.RemovePicture(request.Content.FileName);
            _store.SaveChanges();

            if (System.IO.File.Exists($"img/{request.Content.FileName}"))
                System.IO.File.Delete($"img/{request.Content.FileName}");

            return new ResponceModel<string> { content = request.Content.FileName };
        }


    }
}
