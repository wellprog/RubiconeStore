using Shared.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Error
{
    public static class GoodErrors
    {
        private enum AErrors
        {
            CategoryNotFound = 3000,
            WrongPropertyCategory
        }

        public static T CategoryNotFound<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.CategoryNotFound;
            responce.ErrorDescription = "Соответствующая категория не найдена";

            return responce;
        }

        public static T WrongPropertyCategory<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.WrongPropertyCategory;
            responce.ErrorDescription = "Соответствующая категория не найдена";

            return responce;
        }

    }
}
