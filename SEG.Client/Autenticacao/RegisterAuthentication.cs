using SEG.Domain.Contracts.Autenticacao;
using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Autenticacao
{
    public class RegisterAuthentication : ClientBase, IRegisterAuthentication
    {
        public RegisterAuthentication() : base("https://localhost:44305/api/usuarios/register") { }

        public async Task<ResultModel> RegisterAsync(Domain.Models.Autenticacao.RegisterModel register)
        {
            base.NovaRota("", null);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", register));
        }
    }
}
 