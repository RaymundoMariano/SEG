using SEG.Client.Aplicacao;
using SEG.Domain.Contracts.Clients;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SEG.Domain.Models.Aplicacao;

namespace SEG.Client
{
    public class PerfilAplication : Aplication<PerfilModel>, IPerfilAplication
    {
        public PerfilAplication() : base("https://localhost:44366/api/perfis") { }

        #region ObterRestricoesAsync
        public async Task<ResultModel> ObterRestricoesAsync(int perfilId, string token)
        {
            base.NovaRota("/GetRestricoes?perfilId=" + perfilId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultModel> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfilModel> restricoesModel, string token)
        {
            base.NovaRota("/PostRestricoes?perfilId=" + perfilId, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", restricoesModel));
        }
        #endregion
    }
}
