using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RubiconeStoreBack.DataAccess;
using RubiconeStoreBack.Error;
using RubiconeStoreBack.Helpers;
using Shared.Model;
using Shared.Helpers;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly DbStore _store;
        private readonly UserHelper _userHelper;

        public CartController(ILogger<CartController> logger, DbStore store, UserHelper userHelper)
        {
            this._logger = logger;
            this._store = store;
            this._userHelper = userHelper;
        }

        [HttpGet]
        public ResponceModel<Check> getCart(ResponceModel<User> userRequest)
        {
            var user = userRequest.content;

            if (!user.IsModelRight())
                return new ResponceModel<Check>().FieldEmptyError();

            for (int i = 0; i < user.Checks.Count; i++)
            {
                if(isCart(user.Checks[i]) == true)
                {
                    return new ResponceModel<Check> { content = user.Checks[i] };
                }
            }
            return new ResponceModel<Check>().RecordNotFound();
        }

        public ResponceModel<int> getCartPrice(ResponceModel<User> userRequest)
        {
            var userCart = getCart(userRequest).content;
            if (userCart == null)
                return new ResponceModel<int>().RecordNotFound();

            return new ResponceModel<int> { content = userCart.getPrice() };
        }
        
        public ResponceModel<Sell> GetCartItem(ResponceModel<User> userRequest, int index)
        {
            var userCart = getCart(userRequest).content;
            if (userCart == null)
                return new ResponceModel<Sell>().RecordNotFound();

            return new ResponceModel<Sell> { content = userCart.Sells[index] };
        }
        
        public ResponceModel<Sell> addCartItem(ResponceModel<User> userRequest, ResponceModel<Sell> sellRequest)
        {
            var user = userRequest.content;
            var userCart = getCart(userRequest).content;
            var addedSell = sellRequest.content;
            if (userCart == null || !addedSell.IsModelRight())
                return new ResponceModel<Sell>().RecordNotFound();

            _store.Remove<User>(user);

            userCart.Sells.Add(addedSell);

            _store.Add(user);
            _store.SaveChanges();

            return new ResponceModel<Sell> { content = addedSell };
        }

        public ResponceModel<Sell> deleteCartItem(ResponceModel<User> userRequest, ResponceModel<Sell> sellRequest)
        {
            var user = userRequest.content;
            var userCart = getCart(userRequest).content;
            var addedSell = sellRequest.content;
            if (userCart == null || !addedSell.IsModelRight())
                return new ResponceModel<Sell>().RecordNotFound();

            _store.Remove<User>(user);

            userCart.Sells.Remove(addedSell);

            _store.Remove(user);
            _store.SaveChanges();

            return new ResponceModel<Sell> { content = addedSell };
        }
        
        //pay()

        private bool isCart(Check check)
        {
            return !check.IsDone;
        }
    }
}
