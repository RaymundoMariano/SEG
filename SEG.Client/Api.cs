using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SEG.Client
{
    public abstract class Api
    {
        protected readonly string URI;
        protected HttpClient Client;

        protected Api(string uri)
        {
            URI = uri;
            NovaRota("", null);
        }

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
    }
}
