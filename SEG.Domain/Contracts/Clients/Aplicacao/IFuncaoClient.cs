using SEG.Domain.Enums;
using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Aplicacao
{
    public interface IFuncaoClient : IClient<FuncaoModel>
    {
        Task<List<FuncaoModel>> ObterAsync(EFuncao eFuncaoIni, string token);
    }
}
