using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Error
{
    public static class StorageErrors
    {
        private enum AErrors
        {
            NotEnoughGoods = 4000,
        }

        public static T NotEnoughGoods<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.NotEnoughGoods;
            responce.ErrorDescription = "Недостаточно товара на складе";

            return responce;
        }
    }
}
