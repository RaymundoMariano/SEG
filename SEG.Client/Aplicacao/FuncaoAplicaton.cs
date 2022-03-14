using SEG.Domain.Contracts.Clients;
using SEG.Domain.Enums;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class FuncaoAplicaton : Aplication<FuncaoModel>, IFuncaoAplication
    {
        public FuncaoAplicaton() : base("https://localhost:44366/api/funcoes") { }

        public async Task<ResultModel> ObterAsync(EFuncao eFuncaoIni, string token)
        {
            base.NovaRota($"/PostFuncoes?eFuncaoIni={eFuncaoIni}", token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", ""));
}
    }
}
