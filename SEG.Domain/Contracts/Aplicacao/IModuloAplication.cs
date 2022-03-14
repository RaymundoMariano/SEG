using SEG.Domain.Contracts.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IModuloAplication : IAplication<ModuloModel>
    {
        Task<ResultModel> ObterFormulariosAsync(int moduloId, string token);
        Task<ResultModel> AtualizarFormulariosAsync(int moduloId, List<FormularioModel> formulariosModel, string token);
    }
}
