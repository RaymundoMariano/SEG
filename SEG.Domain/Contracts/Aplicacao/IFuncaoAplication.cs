using SEG.Domain.Contracts.Aplicacao;
using SEG.Domain.Enums;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients
{
    public interface IFuncaoAplication : IAplication<FuncaoModel>
    {
        Task<ResultModel> ObterAsync(EFuncao eFuncaoIni, string token);
    }
}
