using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Seguranca
{
    public interface IUsuarioSecurity
    {
        Task<ResultModel> ObterPerfilAsync(string modulo, UsuarioModel usuario, string token);
    }
}
