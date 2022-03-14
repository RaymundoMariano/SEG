using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Aplicacao
{
    public interface IAplication<T> where T : _Model
    {
        Task<ResultModel> ObterAsync(string token);
        Task<ResultModel> ObterAsync(int id, string token);
        Task<ResultModel> InsereAsync(T model, string token);
        Task<ResultModel> UpdateAsync(int id, T model, string token);
        Task<ResultModel> RemoveAsync(int id, string token);
    }
}
