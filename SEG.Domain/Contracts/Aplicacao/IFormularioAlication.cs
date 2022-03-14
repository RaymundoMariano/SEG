using SEG.Domain.Contracts.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IFormularioAlication : IAplication<FormularioModel>
    {
        Task<ResultModel> ObterEventosAsync(int formularioId, string token);
        Task<ResultModel> AtualizarEventosAsync(int formularioId, List<EventoModel> eventosModel, string token);
    }
}
