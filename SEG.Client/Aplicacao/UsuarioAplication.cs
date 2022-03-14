using SEG.Domain.Contracts.Clients;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class UsuarioAplication : Aplication<UsuarioModel>, IUsuarioAplication
    {
        public UsuarioAplication() : base("https://localhost:44366/api/usuarios") { }

        #region ObterRestricoesAsync
        public async Task<ResultModel> ObterRestricoesAsync(int usuarioId, string token)
        {
            base.NovaRota("/GetRestricoes?usuarioId=" + usuarioId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultModel> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuarioModel> restricoesModel, string token)
        {
            base.NovaRota("/PostRestricoes?usuarioId=" + usuarioId, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", restricoesModel));
        }
        #endregion

        #region ObterPerfisAsync
        public async Task<ResultModel> ObterPerfisAsync(int usuarioId, string token)
        {
            base.NovaRota("/GetPerfis?usuarioId=" + usuarioId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }
        #endregion

        #region AtualizarPerfisAsync
        public async Task<ResultModel> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuarioModel> perfisModel, string token)
        {
            base.NovaRota("/PostPerfis?usuarioId=" + usuarioId, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", perfisModel));
        }
        #endregion
    }
}
