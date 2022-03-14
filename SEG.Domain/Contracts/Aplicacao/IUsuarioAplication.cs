using SEG.Domain.Contracts.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IUsuarioAplication : IAplication<UsuarioModel>
    {
        Task<ResultModel> ObterRestricoesAsync(int usuarioId, string token);
        Task<ResultModel> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuarioModel> restricoesModel, string token);
        Task<ResultModel> ObterPerfisAsync(int usuarioId, string token);
        Task<ResultModel> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuarioModel> perfisModel, string token);

    }
}
