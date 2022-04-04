using Newtonsoft.Json;
using SEG.Domain.Models.Response;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SEG.Client._Base
{
    public abstract class BaseClient
    {
        protected readonly string URI;
        protected HttpClient Client;

        protected BaseClient(string uri)
        {
            URI = uri;
            NovaRota("", null);
        }

        #region NovaRota
        protected void NovaRota(string complememto, string token)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(URI + complememto);

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
            {
                Client.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Bearer", token);
            }
        }
        #endregion

        #region Response
        protected ResponseModel Response(HttpResponseMessage httpResponse)
        {
            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<ResponseModel>(conteudo);

            if (!response.Succeeded) throw new Exception();

            if (response.Errors.Count == 1) throw new ClientException(response.Errors[0]);

            return response;
        }
        #endregion
    }
}
