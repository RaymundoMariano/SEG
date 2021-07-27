using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IModuloClient : IClient<ModuloModel>
    {
        Task<ResultResponse> ObterFormulariosAsync(int moduloId, string token);
        Task<ResultResponse> AtualizarFormulariosAsync(int moduloId, List<FormularioModel> formulariosModel, string token);
    }
}
