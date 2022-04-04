using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Aplicacao
{
    public interface IModuloClient : IClient<ModuloModel>
    {
        Task<List<FormularioModel>> ObterFormulariosAsync(int moduloId, string token);
        Task AtualizarFormulariosAsync(int moduloId, List<FormularioModel> formulariosModel, string token);
    }
}
