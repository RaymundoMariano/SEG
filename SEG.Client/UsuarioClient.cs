﻿using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class UsuarioClient : Client<UsuarioModel>, IUsuarioClient
    {
        public UsuarioClient() : base("https://localhost:44366/api/usuarios") { }

        #region ObterRestricoesAsync
        public async Task<ResultResponse> ObterRestricoesAsync(int usuarioId, string token)
        {
            base.NovaRota("/GetRestricoes?usuarioId=" + usuarioId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuarioModel> restricoesModel, string token)
        {
            base.NovaRota("/PostRestricoes?usuarioId=" + usuarioId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", restricoesModel));
        }
        #endregion

        #region ObterPerfisAsync
        public async Task<ResultResponse> ObterPerfisAsync(int usuarioId, string token)
        {
            base.NovaRota("/GetPerfis?usuarioId=" + usuarioId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }
        #endregion

        #region AtualizarPerfisAsync
        public async Task<ResultResponse> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuarioModel> perfisModel, string token)
        {
            base.NovaRota("/PostPerfis?usuarioId=" + usuarioId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", perfisModel));
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
