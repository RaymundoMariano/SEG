using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Enums;
using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class FuncaoClient : Client<FuncaoModel>, IFuncaoClient
    {
        public FuncaoClient() : base("https://localhost:44366/api/funcoes") { }

        #region ObterAsync
        public async Task<List<FuncaoModel>> ObterAsync(EFuncao eFuncaoIni, string token)
        {
            base.NovaRota($"/PostFuncoes?eFuncaoIni={eFuncaoIni}", token);
            var response = base.Response(await base.Client.PostAsJsonAsync("", ""));

            return JsonConvert.DeserializeObject<List<FuncaoModel>>(response.ObjectRetorno.ToString());
        }
        #endregion
    }
}
