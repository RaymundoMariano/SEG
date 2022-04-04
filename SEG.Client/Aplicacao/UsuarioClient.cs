using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class UsuarioClient : Client<UsuarioModel>, IUsuarioClient
    {
        public UsuarioClient() : base("https://localhost:44366/api/usuarios") { }

        #region ObterRestricoesAsync
        public async Task<List<RestricaoUsuarioModel>> ObterRestricoesAsync(int usuarioId, string token)
        {
            base.NovaRota("/GetRestricoes?usuarioId=" + usuarioId, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<RestricaoUsuarioModel>>(response.ObjectRetorno.ToString());
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuarioModel> restricoesModel, string token)
        {
            base.NovaRota("/PostRestricoes?usuarioId=" + usuarioId, token);
            base.Response(await base.Client.PostAsJsonAsync("", restricoesModel));
        }
        #endregion

        #region ObterPerfisAsync
        public async Task<List<PerfilUsuarioModel>> ObterPerfisAsync(int usuarioId, string token)
        {
            base.NovaRota("/GetPerfis?usuarioId=" + usuarioId, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<PerfilUsuarioModel>>(response.ObjectRetorno.ToString());
        }
        #endregion

        #region AtualizarPerfisAsync
        public async Task AtualizarPerfisAsync(int usuarioId, List<PerfilUsuarioModel> perfisModel, string token)
        {
            base.NovaRota("/PostPerfis?usuarioId=" + usuarioId, token);
            base.Response(await base.Client.PostAsJsonAsync("", perfisModel));
        }
        #endregion
    }
}
