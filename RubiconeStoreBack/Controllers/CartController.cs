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
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Builder;

namespace RubiconeStoreBack.Controllers
{
    [ApiController]
    //Класс для контролирования корзины пользователя
    public class CartController : BaseController<CartController>
    {
        public CartController(ILogger<CartController> logger, DbStore store, UserHelper userHelper)
            : base(logger, userHelper, store) { }
        

        [Route("[controller]/{AuthKey}")]
        [HttpGet]
        public ResponceModel<CartModel> GetCart(string AuthKey)
        {
            //Проверяем запрос
            var responce = CheckRequest<CartModel>(AuthKey);
            if (responce != null)
                return responce;

            //Ищем или создаем корзину
            Check cart = GetCartInner();

            CartModel model = new CartModel();
            model.Check = cart;

            foreach (var item in cart.Sells)
                model.CartItems.Add(new CartItemModel()
                {
                    Count = item.Count,
                    Good = item.Storage.Good
                });


            return new ResponceModel<CartModel>()
            {
                content = model
            };
        }

        private Check GetCartInner()
        {
            var cart = _store.Checks.Where(f => f.UserID == _user.ID && !f.IsDone)
                                    .Include(f => f.Sells)
                                    .ThenInclude(f => f.Storage)
                                    .ThenInclude(f => f.Good).FirstOrDefault();

            if (cart == null)
            {
                cart = new Check()
                {
                    UserID = _user.ID,
                    IsDone = false,
                    CreatedDate = DateTime.Now
                };

                _store.Add(cart);
                _store.SaveChanges();
            }

            return cart;
        }

        //Положить товар в корзину
        [Route("[controller]")]
        [HttpPost]
        public ResponceModel<CartItemModel> AddToCart(RequestModel<CartItemModel> request)
        {
            //Проверяем запрос
            var responce = CheckRequest(request);
            if (responce != null)
                return responce;

            //Находим товар
            var good = _store.Goods.Where(f => f.ID == request.Content.Good.ID).FirstOrDefault();
            if (good == null)
                return new ResponceModel<CartItemModel>().RecordNotFound();

            //Проверяем количество
            var allGoods = _store.Storages.Where(f => f.GoodID == request.Content.Good.ID).Sum(f => f.Count);
            var selledGoods = _store.Sells.Where(f => f.Storage.GoodID == request.Content.Good.ID).Sum(f => f.Count);

            if (allGoods - selledGoods < request.Content.Count)
                return new ResponceModel<CartItemModel>().NotEnoughGoods();

            //Ищем или создаем корзину
            var cart = GetCartInner(); //(Уже проверяли пользователя на корректность!)

            //Добавляем товар
            var storage = _store.Storages.Where(f => f.GoodID == request.Content.Good.ID).OrderByDescending(f => f.ID).First();
            var sell = cart.Sells.Where(f => f.StorageID == storage.ID).FirstOrDefault();
            if(sell != null)
            {
                sell.Count += request.Content.Count;
            }
            else
            {
                cart.Sells.Add(new Sell
                {
                    Count = request.Content.Count,
                    StorageID = storage.ID
                });
            }

            _store.SaveChanges();


            return new ResponceModel<CartItemModel>();
        }


        [Route("[controller]/{AuthKey}/{ElementId}")]
        [HttpDelete]
        public ResponceModel<bool> DeleteFromCart(string AuthKey, int ElementId)
        {
            //Проверяем запрос
            var responce = CheckRequest<bool>(AuthKey);
            if (responce != null)
                return responce;

            var cart = _store.Checks.Where(f => f.UserID == _user.ID && !f.IsDone)
                        .Include(f => f.Sells)
                        .ThenInclude(f => f.Storage)
                        .FirstOrDefault();

            if (cart == null)
                return new ResponceModel<bool>().NoUserCart();

            var itemToDel = cart.Sells.Where(f => f.Storage.GoodID == ElementId).FirstOrDefault();
            if (itemToDel != null)
            {
                cart.Sells.Remove(itemToDel);
                _store.SaveChanges();
            }

            return new ResponceModel<bool>();
        }

        [Route("[controller]")]
        [HttpPatch]
        public ResponceModel<CartItemModel> UpdateCart(RequestModel<CartItemModel> request)
        {
            //Проверяем запрос
            var responce = CheckRequest<CartItemModel>(request);
            if (responce != null)
                return responce;
            
            //Получаем корзину
            var cart = GetCartInner();

            //Получаем премет
            var updatedItem = cart.Sells.Where(f => f.Storage.Good.ID == request.Content.Good.ID).FirstOrDefault();
            
            //Проверяем количество
            var allGoods = _store.Storages.Where(f => f.GoodID == request.Content.Good.ID).Sum(f => f.Count);
            var selledGoods = _store.Sells.Where(f => f.Storage.GoodID == request.Content.Good.ID).Sum(f => f.Count);

            if (allGoods - selledGoods < request.Content.Count)
                return new ResponceModel<CartItemModel>().NotEnoughGoods();

            //Меняем количество исходного предмета
            updatedItem.Count = request.Content.Count;

            _store.SaveChanges();

            return new ResponceModel<CartItemModel>();
        }

        [Route("[controller]/{AuthKey}")]
        [HttpPost]
        public ResponceModel<bool> FinishCart(string AuthKey)
        {
            //Проверяем запрос
            var responce = CheckRequest<bool>(AuthKey);
            if (responce != null)
                return responce;

            //Получаем корзину
            var cart = GetCartInner();

            //Ставим флаг завершенности
            cart.IsDone = true;

            return new ResponceModel<bool>();
        }


        //[HttpGet]
        ////Метод возращает сумму цен товаров в корзине
        //public ResponceModel<int> getPrice(RequestModel<User> userRequest)
        //{
        //    //Проверяем все входные данные на корректность
        //    var user = userRequest.Content;
        //    if (!user.IsModelRight())
        //        return new ResponceModel<int>().UserNotFound();

        //    var userCart = user.Cart;
        //    if (userCart == null)
        //        return new ResponceModel<int>().RecordNotFound();

        //    //Через метод класса Cart получаем сумму товаров (В будущем для этого можно будет выделить переменную в классе Cart, чтобы каждый раз заново не считать сумму стоимостей покупок)
        //    int userCartPrice = userCart.getPrice();

        //    return new ResponceModel<int> { content = userCartPrice };
        //}

        ////Получает предмет в корзине, который лежит в ней по указанному индексу
        //public ResponceModel<Sell> getItem(int index, RequestModel<User> userRequest)
        //{
        //    //Проверяем все входные данные на корректность
        //    var user = userRequest.Content;
        //    if (!user.IsModelRight())
        //        return new ResponceModel<Sell>().UserNotFound();

        //    var userCart = user.Cart;
        //    if (userCart == null)
        //        return new ResponceModel<Sell>().RecordNotFound();

        //    //Прямо из корзины забираем Sell
        //    Sell gettedItem = userCart.Sells[index];

        //    return new ResponceModel<Sell> { content = gettedItem };
        //}

        //[HttpPost]
        ////Добавляет предмет в корзину
        //public ResponceModel<Sell> addItem(int sellID, RequestModel<User> userRequest)
        //{
        //    //Проверяем все входные данные на корректность
        //    var user = userRequest.Content;
        //    if (!user.IsModelRight())
        //        return new ResponceModel<Sell>().UserNotFound();

        //    var addedSell = _store.Sells.Where(b => b.ID == sellID).FirstOrDefault();
        //    if (!addedSell.IsModelRight())
        //        return new ResponceModel<Sell>().RecordNotFound();

        //    var userCart = userRequest.Content.Cart;
        //    if (userCart == null)
        //        return new ResponceModel<Sell>().RecordNotFound();

        //    //Добавляем addedSell в UserCart
        //    addedSell.CheckID = userCart.ID;
        //    userCart.Sells.Add(addedSell);
        //    //Сохраняем изменения в БД
        //    _store.Update<Sell>(addedSell);
        //    _store.Update<Cart>(userCart);
        //    _store.SaveChanges();

        //    return new ResponceModel<Sell> { content = addedSell };
        //}

        //[HttpDelete]
        ////Удаляет предмет из корзины
        //public ResponceModel<Sell> deleteItem(int sellID, RequestModel<User> userRequest)
        //{
        //    //Проверяем все входные данные на корректность
        //    var user = userRequest.Content;
        //    if (!user.IsModelRight())
        //        return new ResponceModel<Sell>().UserNotFound();

        //    var deletedSell = _store.Sells.Where(b => b.ID == sellID).FirstOrDefault();
        //    if (!deletedSell.IsModelRight())
        //        return new ResponceModel<Sell>().RecordNotFound();

        //    var userCart = userRequest.Content.Cart;
        //    if (userCart == null)
        //        return new ResponceModel<Sell>().RecordNotFound();

        //    //Удаляем addedSell из UserCart
        //    deletedSell.CheckID = 0; //! Вероятно, лучше удалить deletedSell вообще из БД, так как Sell лежащие в корзине не влияют на Storages, и удалив их оттуда к ним нельзя вернуться
        //    userCart.Sells.Remove(deletedSell);
        //    //Сохраняем изменения в БД
        //    _store.Update<Sell>(deletedSell);
        //    _store.Update<Cart>(userCart);
        //    _store.SaveChanges();

        //    return new ResponceModel<Sell> { content = deletedSell };
        //}

        ////Возвращает количество товаров в корзине
        //public ResponceModel<int> getCount(RequestModel<User> userRequest)
        //{
        //    var user = userRequest.Content;
        //    if (!user.IsModelRight())
        //        return new ResponceModel<int>().UserNotFound();

        //    var userCart = user.Cart;
        //    if (userCart == null)
        //        return new ResponceModel<int>().RecordNotFound();

        //    int userCartItemsCount = userCart.Sells.Count;

        //    return new ResponceModel<int> { content = userCartItemsCount };
        //}

        //pay() //Через паттерн "Адаптер" легко превратит Cart в Check
    }
}
