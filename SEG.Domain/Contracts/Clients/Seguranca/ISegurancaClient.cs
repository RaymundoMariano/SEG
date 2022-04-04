using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Seguranca
{
    public interface ISegurancaClient
    {
        Task<SegurancaModel> ObterPerfilAsync(string modulo, RegistroModel registro);
    }
}
