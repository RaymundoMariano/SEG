using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Aplicacao
{
    public interface IFormularioClient : IClient<FormularioModel>
    {
        Task<List<EventoModel>> ObterEventosAsync(int formularioId, string token);
        Task AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token);
    }
}
