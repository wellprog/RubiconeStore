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
        public ResponceModel<UserAuthModel> Autorize(string loginOrEmail, string password)
        {
            if (string.IsNullOrEmpty(loginOrEmail) || string.IsNullOrEmpty(password))
                return new ResponceModel<UserAuthModel>().FieldEmptyError();

            var foundUser = _store.Users.Where(f => f.Login == loginOrEmail).Include(f => f.UserSessions).FirstOrDefault();
            if (foundUser == null)
                foundUser = _store.Users.Where(f => f.Email == loginOrEmail).Include(f => f.UserSessions).FirstOrDefault();

            if (foundUser == null)
                return new ResponceModel<UserAuthModel>().UserNotFound();

            if (!foundUser.IsPasswordRight(password))
                return new ResponceModel<UserAuthModel>().WrongPassword();

            foundUser.UserSessions.Where(f => f.IsActive == true).ToList().ForEach(f => f.IsActive = false);

            UserSession session = new UserSession() { UserID = foundUser.ID };

            _store.Add(session);
            _store.SaveChanges();

            return new ResponceModel<UserAuthModel>()
            {
                content = new UserAuthModel
                {
                    User = foundUser,
                    UserSession = session
                }
            };
        }


        [HttpPost]
        public ResponceModel<UserAuthModel> Register(User user)
        {
            if (!user.IsModelRight())
                return new ResponceModel<UserAuthModel>().FieldEmptyError();

            var foundLogin = _store.Users.Where(f => f.Login == user.Login).Include(f => f.UserSessions).FirstOrDefault();
            if (foundLogin != null)
                return new ResponceModel<UserAuthModel>().SameLoginFound();

            var foundEmail = _store.Users.Where(f => f.Email == user.Email).Include(f => f.UserSessions).FirstOrDefault();
            if (foundEmail != null)
                return new ResponceModel<UserAuthModel>().SameEmailFound();

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
