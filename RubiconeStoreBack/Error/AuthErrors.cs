using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Error
{
    public static class AuthErrors
    {
        private enum AErrors
        {
            FieldsEmpty = 1000,
            UserNotFound,
            WrongPassword,
            SameLoginFound,
            SameEmailFound,
            WrongAuthKey
        }

        public static T FieldEmptyError<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.FieldsEmpty;
            responce.ErrorDescription = "Заполните все поля";

            return responce;
        }

        public static T UserNotFound<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.UserNotFound;
            responce.ErrorDescription = "Такого пользователя не найдено";

            return responce;
        }

        public static T WrongPassword<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.WrongPassword;
            responce.ErrorDescription = "Пароль пользователя неверен";

            return responce;
        }

        public static T SameLoginFound<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.SameLoginFound;
            responce.ErrorDescription = "Этот логин занят другим пользователем";

            return responce;
        }

        public static T SameEmailFound<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.SameEmailFound;
            responce.ErrorDescription = "Пользователь с такой почтой уже существует";

            return responce;
        }

        public static T WrongAuthKey<T>(this T responce) where T : IErrorResponce
        {
            responce.ErrorCode = (int)AErrors.WrongAuthKey;
            responce.ErrorDescription = "Ключ авторизации неверен";

            return responce;
        }
    }
}
