using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client
{
    public class FormularioClient : Client<FormularioModel>, IFormularioClient
    {
        public FormularioClient() : base("https://localhost:44366/api/formularios") { }

        public async Task<ResultResponse> ObterEventosAsync(int formularioId, string token)
        {
            base.NovaRota("/GetEventos?formularioId=" + formularioId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }

        public async Task<ResultResponse> AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token)
        {
            base.NovaRota("/PostEventos?formularioId=" + formularioId, token);
            var httpResponse = await base.Client.PostAsJsonAsync("", eventosModel);

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
    }    
}
