using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IUsuarioClient : IClient<UsuarioModel>
    {
        Task<ResultResponse> ObterRestricoesAsync(int usuarioId, string token);
        Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuarioModel> restricoesModel, string token);
        Task<ResultResponse> ObterPerfisAsync(int usuarioId, string token);
        Task<ResultResponse> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuarioModel> perfisModel, string token);

    }
}
