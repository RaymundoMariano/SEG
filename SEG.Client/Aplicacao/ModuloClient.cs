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
    public class ModuloClient : Client<ModuloModel>, IModuloClient
    {
        public ModuloClient() : base("https://localhost:44366/api/modulos") { }

        #region ObterFormulariosAsync
        public async Task<List<FormularioModel>> ObterFormulariosAsync(int moduloId, string token)
        {
            base.NovaRota("/GetFormularios?moduloId=" + moduloId, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<FormularioModel>>(response.ObjectRetorno.ToString());
        }
        #endregion

        #region AtualizarFormulariosAsync
        public async Task AtualizarFormulariosAsync(int moduloId, List<FormularioModel> formulariosModel, string token)
        {
            base.NovaRota("/PostFormularios?moduloId=" + moduloId, token);
            base.Response(await base.Client.PostAsJsonAsync("", formulariosModel));
        }
        #endregion
    }
}
