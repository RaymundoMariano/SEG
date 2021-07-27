using SEG.Domain.Contracts.Clients.Auth;
using Seguranca.Domain.Auth.Requests;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Auth
{
    public class LoginClient : Api, ILoginClient
    {
        public LoginClient() : base("https://localhost:44366/api/authentications/login") { }

        public async Task<HttpResponseMessage> LoginAsync(LoginRequest login)
        {
            base.NovaRota("", null);
            return await base.Client.PostAsJsonAsync("", login);         
        }
    }
} 
