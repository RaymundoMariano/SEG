using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IPerfilClient : IClient<PerfilModel>
    {
        Task<ResultResponse> ObterRestricoesAsync(int perfilId, string token);
        Task<ResultResponse> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfilModel> restricoesModel, string token);
    }
}
