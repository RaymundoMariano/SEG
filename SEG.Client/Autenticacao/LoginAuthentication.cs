using SEG.Domain.Contracts.Autenticacao;
using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Autenticacao
{
    public class LoginAuthentication : ClientBase, ILoginAuthentication
    {
        public LoginAuthentication() : base("https://localhost:44305/api/usuarios/login") { }

        public async Task<ResultModel> LoginAsync(LoginModel login)
        {
            base.NovaRota("", null);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", login));
        }
    }
} 
