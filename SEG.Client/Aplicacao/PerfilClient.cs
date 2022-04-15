using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Contracts.Clients.Aplicacao;
using System;
using Newtonsoft.Json;
using SEG.Domain.Models.Response;

namespace SEG.Client.Aplicacao
{
    public class PerfilClient : Client<PerfilModel>, IPerfilClient
    {
        public PerfilClient() : base("https://localhost:44366/api/perfis") { }

        #region ObterRestricoesAsync
        public async Task<List<RestricaoPerfilModel>> ObterRestricoesAsync(int perfilId, string token)
        {
            try
            {
                base.NovaRota("/GetRestricoes?perfilId=" + perfilId, token);
                var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

                if (!response.Succeeded) throw new Exception();

                return JsonConvert.DeserializeObject<List<RestricaoPerfilModel>>(response.ObjectRetorno.ToString());
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfilModel> restricoesModel, string token)
        {
            base.NovaRota("/PostRestricoes?perfilId=" + perfilId, token);
            base.Response(await base.Client.PostAsJsonAsync("", restricoesModel));
        }
        #endregion
    }
}
