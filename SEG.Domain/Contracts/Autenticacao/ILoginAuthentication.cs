using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Autenticacao
{
    public interface ILoginAuthentication
    {
        Task<ResultModel> LoginAsync(LoginModel login);
    }
}
