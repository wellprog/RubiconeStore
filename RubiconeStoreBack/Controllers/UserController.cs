using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Model;

using Shared.Model;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbStore _store;

        public UserController(ILogger<UserController> logger, DbStore store)
        {
            _logger = logger;
            _store = store;
        }

        [HttpGet]
        public ResponceModel<UserAuthModel> Autorize(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return new ResponceModel<UserAuthModel>().FieldEmptyError();
            }

            var user = _store.Users.Where(f => f.Email == email).Include(f => f.UserSessions).FirstOrDefault();

            if (user == null)
                return new ResponceModel<UserAuthModel>().UserNotFound();

            if (!user.IsPasswordRight(password))
            {
                return new ResponceModel<UserAuthModel>().WrongPassword();
            }

            user.UserSessions.Where(f => f.IsActive == true).ToList().ForEach(f => f.IsActive = false);

            UserSession session = new UserSession() { UserID = user.ID };

            _store.Add(session);
            _store.SaveChanges();

            return new ResponceModel<UserAuthModel>()
            {
                content = new UserAuthModel
                {
                    User = user,
                    UserSession = session
                }
            };
        }


        [HttpPost]
        public ResponceModel<UserAuthModel> Register(User user)
        {
            if (!user.IsModelRight()) return new ResponceModel<UserAuthModel>().FieldEmptyError();

            var foundUser = _store.Users.Where(f => f.Email == user.Email).Include(f => f.UserSessions).FirstOrDefault();

            if (foundUser != null)
                return new ResponceModel<UserAuthModel>().SameUserFound();

            user.UserSessions = new List<UserSession>();
            user.UserSessions.Add(new UserSession());
            user.PreparePassword();

            _store.Add(user);
            _store.SaveChanges();

            return new ResponceModel<UserAuthModel>()
            {
                content = new UserAuthModel
                {
                    User = user,
                    UserSession = user.UserSessions.First()
                }
            };
        }

        [HttpPatch]
        public ResponceModel<UserAuthModel> Update(RequestModel<User> userModel)
        {
            if (string.IsNullOrWhiteSpace(userModel.AuthKey))
                return new ResponceModel<UserAuthModel>().WrongAuthKey();

            var userSession = _store.UserSessions.Where(f => f.SessionToken == userModel.AuthKey && f.IsActive == true).Include(f => f.User).FirstOrDefault();

            if (userSession == null || userSession.User.ID != userModel.Content.ID)
                return new ResponceModel<UserAuthModel>().WrongAuthKey();

            userSession.User.Email = userModel.Content.Email;
            userSession.User.FirstName = userModel.Content.FirstName;
            userSession.User.LastName = userModel.Content.LastName;
            userSession.User.Login = userModel.Content.Login;
            userSession.User.Phone = userModel.Content.Phone;

            _store.SaveChanges();

            return new ResponceModel<UserAuthModel>()
            {
                content = new UserAuthModel
                {
                    User = userSession.User,
                    UserSession = userSession
                }
            };
        }
    }
}
