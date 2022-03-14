using SEG.Domain.Contracts.Clients;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class FormularioAplication : Aplication<FormularioModel>, IFormularioAlication
    {
        public FormularioAplication() : base("https://localhost:44366/api/formularios") { }

        #region ObterEventosAsync
        public async Task<ResultModel> ObterEventosAsync(int formularioId, string token)
        {
            base.NovaRota("/GetEventos?formularioId=" + formularioId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }
        #endregion

        #region AtualizarEventosAsync
        public async Task<ResultModel> AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token)
        {
            base.NovaRota("/PostEventos?formularioId=" + formularioId, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", eventosModel));
        }
        #endregion
    }
}
