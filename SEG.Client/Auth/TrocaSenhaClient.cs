using SEG.Domain.Contracts.Clients.Auth;
using Seguranca.Domain.Auth.Requests;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Auth
{
    public class TrocaSenhaClient : Api, ITrocaSenhaClient
    {
        public TrocaSenhaClient() : base("https://localhost:44366/api/authentications/trocasenha") { }

        public async Task<HttpResponseMessage> TrocaSenhaAsync(TrocaSenhaRequest trocaSenha)
        {
            base.NovaRota("", null);
            return await base.Client.PostAsJsonAsync("", trocaSenha);         
        }
    }
} 
