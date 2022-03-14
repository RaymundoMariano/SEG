using SEG.Domain.Contracts.Seguranca;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Seguranca
{
    public class UsuarioSecurity : ClientBase, IUsuarioSecurity
    {
        public UsuarioSecurity() : base("https://localhost:44366/api/usuarios") { }

        public async Task<ResultModel> ObterPerfilAsync(string modulo, UsuarioModel usuario, string token)
        {
            base.NovaRota("/PostUsuario?modulo=" + modulo, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", usuario));
        }
    }
} 
