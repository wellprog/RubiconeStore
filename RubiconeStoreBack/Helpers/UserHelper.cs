using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using Shared.Model;

namespace RubiconeStoreBack.Helpers
{
    public class UserHelper
    {
        private readonly DbStore _store;

        public UserHelper (DbStore store)
        {
            _store = store;
        }

        public ResponceModel<T> IsUserAutorized<T>(string sessionKey, int userId)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
                return new ResponceModel<T>().WrongAuthKey();

            var userSession = _store.UserSessions.Where(f => f.SessionToken == sessionKey && f.IsActive == true).Include(f => f.User).FirstOrDefault();

            if (userSession == null || userSession.User.ID != userId)
                return new ResponceModel<T>().WrongAuthKey();

            return null;
        }

        public ResponceModel<T> IsUserAutorized<T>(string sessionKey)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
                return new ResponceModel<T>().WrongAuthKey();

            var userSession = _store.UserSessions.Where(f => f.SessionToken == sessionKey && f.IsActive == true).FirstOrDefault();

            if (userSession == null)
                return new ResponceModel<T>().WrongAuthKey();

            return null;
        }

        public User GetUser(string sessionKey)
        {
            var userSession = _store.UserSessions.Include(f => f.User).Where(f => f.SessionToken == sessionKey && f.IsActive == true).FirstOrDefault();
            return userSession?.User;
        }
    }
}
