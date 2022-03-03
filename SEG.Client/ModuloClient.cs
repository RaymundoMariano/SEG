using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class ModuloClient : Client<ModuloModel>, IModuloClient
    {
        public ModuloClient() : base("https://localhost:44366/api/modulos") { }

        #region ObterFormulariosAsync
        public async Task<ResultResponse> ObterFormulariosAsync(int moduloId, string token)
        {
            base.NovaRota("/GetFormularios?moduloId=" + moduloId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }
        #endregion

        #region AtualizarFormulariosAsync
        public async Task<ResultResponse> AtualizarFormulariosAsync(int moduloId, List<FormularioModel> formulariosModel, string token)
        {
            base.NovaRota("/PostFormularios?moduloId=" + moduloId, token);
            var httpResponse = await base.Client.PostAsJsonAsync("", formulariosModel);

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
        #endregion
    }
}
