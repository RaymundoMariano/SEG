using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Enums;
using Seguranca.Domain.Models;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IFuncaoClient : IClient<FuncaoModel>
    {
        Task<ResultResponse> ObterAsync(EFuncao eFuncaoIni, string token);
    }
}
