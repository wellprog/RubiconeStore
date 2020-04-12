using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Error
{
    public static class ObjectErrors
    {
        private enum AErrors
        {
            RecordNotFound = 2000,
            FieldNotPass,
        }

        public static T RecordNotFound<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.RecordNotFound;
            responce.ErrorDescription = "Запись не найдена";

            return responce;
        }

        public static T FieldNotPass<T>(this T responce, string fieldName) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.FieldNotPass;
            responce.ErrorDescription = $"Поле { fieldName } не передано";

            return responce;
        }
    }
}
