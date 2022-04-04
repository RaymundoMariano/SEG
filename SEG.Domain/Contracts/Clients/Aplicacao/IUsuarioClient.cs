using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Aplicacao
{
    public interface IUsuarioClient : IClient<UsuarioModel>
    {
        Task<List<RestricaoUsuarioModel>> ObterRestricoesAsync(int usuarioId, string token);
        Task AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuarioModel> restricoesModel, string token);
        Task<List<PerfilUsuarioModel>> ObterPerfisAsync(int usuarioId, string token);
        Task AtualizarPerfisAsync(int usuarioId, List<PerfilUsuarioModel> perfisModel, string token);
    }
}
