using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Enums;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class FuncaoClient : Client<FuncaoModel>, IFuncaoClient
    {
        public FuncaoClient() : base("https://localhost:44366/api/funcoes") { }

        public async Task<ResultResponse> ObterAsync(EFuncao eFuncaoIni, string token)
        {
            base.NovaRota($"/PostFuncoes?eFuncaoIni={eFuncaoIni}", token);
            var httpResponse = await base.Client.PostAsJsonAsync("", "");

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
    }
}
