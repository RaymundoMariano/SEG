using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IFormularioClient : IClient<FormularioModel>
    {
        Task<ResultResponse> ObterEventosAsync(int formularioId, string token);
        Task<ResultResponse> AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token);
    }
}
