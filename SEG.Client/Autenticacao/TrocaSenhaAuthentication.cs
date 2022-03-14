using SEG.Domain.Contracts.Autenticacao;
using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Autenticacao
{
    public class TrocaSenhaAuthentication : ClientBase, ITrocaSenhaAuthentication
    {
        public TrocaSenhaAuthentication() : base("https://localhost:44305/api/usuarios/trocasenha") { }

        public async Task<ResultModel> TrocaSenhaAsync(TrocaSenhaModel trocaSenha)
        {
            base.NovaRota("", null);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", trocaSenha));         
        }
    }
} 
