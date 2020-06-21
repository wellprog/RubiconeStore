using Newtonsoft.Json;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RubiconeStore.Helpers
{
    public class RequestHelper
    {
        private readonly HttpClient httpClient;

        public RequestHelper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public RequestHelper()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<T> Get<T>(string URL, Dictionary<string, object> queryParams = null)
        {
            string url = Url(URL, queryParams);

            var responce = await httpClient.GetAsync(url);
            return await GetResponce<T>(responce);
        }

        public async Task<ResponceModel<T>> GetWithResponce<T>(string URL, Dictionary<string, object> queryParams = null)
        {
            string url = Url(URL, queryParams);

            var responce = await httpClient.GetAsync(url);
            return await GetAllResponce<T>(responce);
        }

        public async Task<T> Delete<T>(string URL, Dictionary<string, object> queryParams = null)
        {
            string url = Url(URL, queryParams);

            var responce = await httpClient.DeleteAsync(url);
            return await GetResponce<T>(responce);
        }

        public async Task<T> Post<T, Req>(string URL, Req request, Dictionary<string, object> queryParams = null)
        {
            string url = Url(URL, queryParams);
            var responce = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            return await GetResponce<T>(responce);
        }

        public async Task<T> Put<T, Req>(string URL, Req request, Dictionary<string, object> queryParams = null)
        {
            string url = Url(URL, queryParams);
            var responce = await httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            return await GetResponce<T>(responce);
        }

        public async Task<T> Patch<T, Req>(string URL, Req request, Dictionary<string, object> queryParams = null)
        {
            string url = Url(URL, queryParams);
            var responce = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json") });
            return await GetResponce<T>(responce);
        }

        private static async Task<ResponceModel<T>> GetAllResponce<T>(HttpResponseMessage responce)
        {
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Ошибка запроса - " + responce.StatusCode);
            }

            var jsonString = await responce.Content.ReadAsStringAsync();
            ResponceModel<T> model = JsonConvert.DeserializeObject<ResponceModel<T>>(jsonString);

            return model;
        }
        private static async Task<T> GetResponce<T>(HttpResponseMessage responce)
        {
            return (await GetAllResponce<T>(responce)).content;
        }


        private string Url(string URL, Dictionary<string, object> queryParams = null) => 
                queryParams == null ?
                URL :
                URL + "?" + string.Join("&", queryParams.Select(f => f.Key + "=" + HttpUtility.UrlEncode(f.Value.ToString())));

    }
}
