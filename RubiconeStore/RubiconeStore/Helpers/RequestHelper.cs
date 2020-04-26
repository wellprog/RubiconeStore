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

        public async Task<T> Get<T>(string URL, Dictionary<string, object> queryParams)
        {
            string url = URL + "?" + string.Join("&", queryParams.Select(f => f.Key + "=" + HttpUtility.UrlEncode(f.Value.ToString())));

            var responce = await httpClient.GetAsync(url);
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Ошибка запроса - " + responce.StatusCode);
            }

            var jsonString = await responce.Content.ReadAsStringAsync();
            ResponceModel<T> model = JsonConvert.DeserializeObject<ResponceModel<T>>(jsonString);

            if (model.ErrorCode != 0)
                throw new Exception("Ошибка запроса" + model.ErrorDescription);

            return model.content;
        }
    }
}
