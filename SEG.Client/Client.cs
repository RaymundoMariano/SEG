using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class Client<T> : Api, IClient<T> where T : _Model
    {
        public Client(string uri) : base(uri) { }

        #region ObterAsync
        public async Task<ResultResponse> ObterAsync(string token)
        {
            base.NovaRota("", token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }

        public async Task<ResultResponse> ObterAsync(int id, string token)
        {
            base.NovaRota("/" + id, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }
        #endregion

        #region InsereAsync
        public async Task<ResultResponse> InsereAsync(T model, string token)
        {
            base.NovaRota("", token);
            return Deserialize(await base.Client.PostAsJsonAsync("", model));
        }
        #endregion

        #region UpdateAsync
        public async Task<ResultResponse> UpdateAsync(int id, T model, string token)
        {
            base.NovaRota("/" + id, token);
            return Deserialize(await base.Client.PutAsJsonAsync<T>("", model));
        }
        #endregion

        #region RemoveAsync
        public async Task<ResultResponse> RemoveAsync(int id, string token)
        {
            base.NovaRota("/" + id, token);
            return Deserialize(await base.Client.DeleteAsync(""));
        }
        #endregion

        #region Deserialize
        private ResultResponse Deserialize(HttpResponseMessage httpResponse)
        {
            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
        #endregion
    }
}
