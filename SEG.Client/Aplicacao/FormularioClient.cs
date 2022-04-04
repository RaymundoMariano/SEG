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
    public class FormularioClient : Client<FormularioModel>, IFormularioClient
    {
        public FormularioClient() : base("https://localhost:44366/api/formularios") { }

        #region ObterEventosAsync
        public async Task<List<EventoModel>> ObterEventosAsync(int formularioId, string token)
        {
            base.NovaRota("/GetEventos?formularioId=" + formularioId, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<EventoModel>>(response.ObjectRetorno.ToString());
        }
        #endregion

        #region AtualizarEventosAsync
        public async Task AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token)
        {
            base.NovaRota("/PostEventos?formularioId=" + formularioId, token);
            base.Response(await base.Client.PostAsJsonAsync("", eventosModel));
        }
        #endregion
    }
}
