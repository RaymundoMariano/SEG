using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Aplicacao
{
    public interface IClient<T> where T : _Model
    {
        Task<List<T>> ObterAsync(string token);
        Task<T> ObterAsync(int id, string token);
        Task InsereAsync(T model, string token);
        Task UpdateAsync(int id, T model, string token);
        Task RemoveAsync(int id, string token);
    }
}
