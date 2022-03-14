using SEG.Domain.Contracts.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IPerfilAplication : IAplication<PerfilModel>
    {
        Task<ResultModel> ObterRestricoesAsync(int perfilId, string token);
        Task<ResultModel> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfilModel> restricoesModel, string token);
    }
}
