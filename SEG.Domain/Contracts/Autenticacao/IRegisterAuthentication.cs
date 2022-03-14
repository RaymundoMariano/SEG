using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Autenticacao
{
    public interface IRegisterAuthentication
    {
        Task<ResultModel> RegisterAsync(Models.Autenticacao.RegisterModel register);
    }
}
