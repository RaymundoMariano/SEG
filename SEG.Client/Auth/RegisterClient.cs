using SEG.Domain.Contracts.Clients.Auth;
using Seguranca.Domain.Auth.Requests;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Auth
{
    public class RegisterClient : Api, IRegisterClient
    {
        public RegisterClient() : base("https://localhost:44366/api/authentications/register") { }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterRequest register)
        {
            base.NovaRota("", null);
            return await base.Client.PostAsJsonAsync("", register);
        }
    }
}
 