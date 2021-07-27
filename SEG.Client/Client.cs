using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class Client<T> : Api, IClient<T> where T : _Model
    {
        public Client(string uri) : base(uri) { }

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

        public async Task<ResultResponse> InsereAsync(T model, string token)
        {
            base.NovaRota("", token);
            var httpResponse = await base.Client.PostAsJsonAsync("", model);

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }

        public async Task<ResultResponse> UpdateAsync(int id, T model, string token)
        {
            base.NovaRota("/" + id, token);
            var httpResponse = await base.Client.PutAsJsonAsync<T>("", model);

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }

        public async Task<ResultResponse> RemoveAsync(int id, string token)
        {
            base.NovaRota("/" + id, token);
            var httpResponse = await base.Client.DeleteAsync("");

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
    }
}
