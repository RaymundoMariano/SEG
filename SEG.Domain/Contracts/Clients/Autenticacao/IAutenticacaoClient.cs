using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.Domain.Contracts.Clients.Autenticacao
{
    public interface IAutenticacaoClient
    {
        Task<RegistroModel> LoginAsync(LoginModel login);
        Task<RegistroModel> RegisterAsync(RegisterModel register);
        Task<List<string>> TrocaSenhaAsync(TrocaSenhaModel trocaSenha);
    }
}
