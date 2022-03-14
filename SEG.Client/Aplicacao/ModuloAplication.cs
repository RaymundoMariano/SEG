using SEG.Domain.Contracts.Clients;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class ModuloAplication : Aplication<ModuloModel>, IModuloAplication
    {
        public ModuloAplication() : base("https://localhost:44366/api/modulos") { }

        #region ObterFormulariosAsync
        public async Task<ResultModel> ObterFormulariosAsync(int moduloId, string token)
        {
            base.NovaRota("/GetFormularios?moduloId=" + moduloId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }
        #endregion

        #region AtualizarFormulariosAsync
        public async Task<ResultModel> AtualizarFormulariosAsync(int moduloId, List<FormularioModel> formulariosModel, string token)
        {
            base.NovaRota("/PostFormularios?moduloId=" + moduloId, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", formulariosModel));
        }
        #endregion
    }
}
