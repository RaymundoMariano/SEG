using Seguranca.Domain.Auth.Requests;
using System.Net.Http;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Auth
{
    public interface IRegisterClient
    {
        Task<HttpResponseMessage> RegisterAsync(RegisterRequest register);
    }
}
