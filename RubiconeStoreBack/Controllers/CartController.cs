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
    //Класс для контролирования корзины пользователя
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
        //Метод возвращает тот чек пользователя, который является корзиной
        public ResponceModel<Check> getCart(ResponceModel<User> userRequest)
        {
            var user = userRequest.content;
            if (!user.IsModelRight())
                return new ResponceModel<Check>().UserNotFound();

            for (int i = 0; i < user.Checks.Count; i++)
            {
                if(isCart(user.Checks[i]) == true)
                {
                    return new ResponceModel<Check> { content = user.Checks[i] };
                }
            }
            return new ResponceModel<Check>().RecordNotFound();
        }

        //Метод возращает сумму цен товаров в корзине
        public ResponceModel<int> getPrice(ResponceModel<User> userRequest)
        {
            var user = userRequest.content;
            if (!user.IsModelRight())
                return new ResponceModel<int>().UserNotFound();
            var userCart = getCart(userRequest).content;
            int userCartPrice = userCart.getPrice();

            return new ResponceModel<int> { content = userCartPrice };
        }
        
        //Получает предмет в корзине, который лежит в ней по указанному индексу
        public ResponceModel<Sell> getItem(ResponceModel<User> userRequest, int index)
        {
            var user = userRequest.content;
            if (!user.IsModelRight())
                return new ResponceModel<Sell>().UserNotFound();
            var userCart = getCart(userRequest).content;
            Sell gettedItem = userCart.Sells[index];

            return new ResponceModel<Sell> { content = gettedItem };
        }

        [HttpPost]
        //Добавляет предмет в корзину
        public ResponceModel<Sell> addItem(ResponceModel<User> userRequest, ResponceModel<Sell> sellRequest)
        {
            var user = userRequest.content;
            if (!user.IsModelRight())
                return new ResponceModel<Sell>().UserNotFound();
            var userCart = getCart(userRequest).content;

            var addedSell = sellRequest.content;
            if (!addedSell.IsModelRight())
                return new ResponceModel<Sell>().RecordNotFound();

            addedSell.CheckID = userCart.ID;
            userCart.Sells.Add(addedSell);

            _store.Update<User>(user); //!
            //or
            //_store.Remove<User>(user);
            //*Edit user*
            //_store.Add<User>(user);
            _store.SaveChanges();

            return new ResponceModel<Sell> { content = addedSell };
        }

        [HttpDelete]
        //Удаляет предмет из корзины
        public ResponceModel<Sell> deleteItem(ResponceModel<User> userRequest, ResponceModel<Sell> sellRequest)
        {
            var user = userRequest.content;
            if (!user.IsModelRight())
                return new ResponceModel<Sell>().UserNotFound();
            var userCart = getCart(userRequest).content;

            var deletedSell = sellRequest.content;
            if (!deletedSell.IsModelRight())
                return new ResponceModel<Sell>().RecordNotFound();

            deletedSell.CheckID = 0; //!
            userCart.Sells.Remove(deletedSell);

            _store.Update<User>(user);
            _store.SaveChanges();

            return new ResponceModel<Sell> { content = deletedSell };
        }

        //Возвращает количество товаров в корзине
        public ResponceModel<int> getCount(ResponceModel<User> userRequest)
        {
            var user = userRequest.content;
            if (!user.IsModelRight())
                return new ResponceModel<int>().UserNotFound();
            var userCart = getCart(userRequest).content;
            int userCartItemsCount = userCart.Sells.Count;

            return new ResponceModel<int> { content = userCartItemsCount };
        }

        //pay()

        //Внутренний метод. возвращает является ли чек корзиной
        private bool isCart(Check check)
        {
            return !check.IsDone;
        }
    }
}
