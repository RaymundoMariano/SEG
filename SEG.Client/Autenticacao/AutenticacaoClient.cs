using Newtonsoft.Json;
using SEG.Client._Base;
using SEG.Domain.Contracts.Clients.Autenticacao;
using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Autenticacao
{
    public class AutenticacaoClient : BaseClient, IAutenticacaoClient
    {
        public AutenticacaoClient() : base("https://localhost:44305/api/usuarios/") { }

        #region LoginAsync
        public async Task<RegistroModel> LoginAsync(LoginModel login)
        {
            var registro = new RegistroModel();
            var response = new ResponseModel();
            try
            {
                base.NovaRota("login", null);
                response = base.Response(await base.Client.PostAsJsonAsync("", login));

                return JsonConvert.DeserializeObject<RegistroModel>(response.ObjectRetorno.ToString());
            }
            catch (ClientException)
            {
                registro.Errors = response.Errors;
                return registro;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region RegisterAsync
        public async Task<RegistroModel> RegisterAsync(RegisterModel register)
        {
            var registro = new RegistroModel();
            var response = new ResponseModel();
            try
            {
                base.NovaRota("register", null);
                response = base.Response(await base.Client.PostAsJsonAsync("", register));

                return JsonConvert.DeserializeObject<RegistroModel>(response.ObjectRetorno.ToString());
            }
            catch (ClientException)
            {
                registro.Errors = response.Errors;
                return registro;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region TrocaSenhaAsync
        public async Task<List<string>> TrocaSenhaAsync(TrocaSenhaModel trocaSenha)
        {
            var response = new ResponseModel();
            try
            {
                if (trocaSenha.NovaSenha != trocaSenha.ConfirmeSenha) throw new ClientException(
                    $"Nova senha é diferente da senha de confirmacao!");

                if (trocaSenha.SenhaAtual == trocaSenha.NovaSenha) throw new ClientException(
                    $"Nova senha não pode ser igual a senha atual!");

                base.NovaRota("trocasenha", null);
                response = base.Response(await base.Client.PostAsJsonAsync("", trocaSenha));

                return response.Errors;
            }
            catch (ClientException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
