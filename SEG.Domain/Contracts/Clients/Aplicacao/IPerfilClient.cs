using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Aplicacao
{
    public interface IPerfilClient : IClient<PerfilModel>
    {
        Task<List<RestricaoPerfilModel>> ObterRestricoesAsync(int perfilId, string token);
        Task AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfilModel> restricoesModel, string token);
    }
}
