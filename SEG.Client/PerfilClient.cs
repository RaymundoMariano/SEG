using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class PerfilClient : Client<PerfilModel>, IPerfilClient
    {
        public PerfilClient() : base("https://localhost:44366/api/perfis") { }

        #region ObterRestricoesAsync
        public async Task<ResultResponse> ObterRestricoesAsync(int perfilId, string token)
        {
            base.NovaRota("/GetRestricoes?perfilId=" + perfilId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultResponse> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfilModel> restricoesModel, string token)
        {
            base.NovaRota("/PostRestricoes?perfilId=" + perfilId, token);
            var httpResponse = await base.Client.PostAsJsonAsync("", restricoesModel);

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
        #endregion
    }
}
