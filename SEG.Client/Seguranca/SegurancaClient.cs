using Newtonsoft.Json;
using SEG.Client._Base;
using SEG.Domain.Contracts.Clients.Seguranca;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Seguranca
{
    public class SegurancaClient : BaseClient, ISegurancaClient
    {
        public SegurancaClient() : base("https://localhost:44366/api/usuarios") { }

        #region ObterPerfilAsync
        public async Task<SegurancaModel> ObterPerfilAsync(string modulo, RegistroModel registro)
        {
            var usuario = new UsuarioModel()
            {
                Email = registro.Email,
                Nome = registro.Nome,
            };

            base.NovaRota("/PostUsuario?modulo=" + modulo, registro.Token);
            var response = base.Response(await base.Client.PostAsJsonAsync("", usuario));

            return JsonConvert.DeserializeObject<SegurancaModel>(response.ObjectRetorno.ToString());
        }
        #endregion
    }
} 
