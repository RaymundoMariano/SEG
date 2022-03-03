using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class FormularioClient : Client<FormularioModel>, IFormularioClient
    {
        public FormularioClient() : base("https://localhost:44366/api/formularios") { }

        #region ObterEventosAsync
        public async Task<ResultResponse> ObterEventosAsync(int formularioId, string token)
        {
            base.NovaRota("/GetEventos?formularioId=" + formularioId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }
        #endregion

        #region AtualizarEventosAsync
        public async Task<ResultResponse> AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token)
        {
            base.NovaRota("/PostEventos?formularioId=" + formularioId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", eventosModel));
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
