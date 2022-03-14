using SEG.Domain.Contracts.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SEG.Client.Aplicacao
{
    public class Aplication<T> : ClientBase, IAplication<T> where T : _Model
    {
        public Aplication(string uri) : base(uri) { }

        #region ObterAsync
        public async Task<ResultModel> ObterAsync(string token)
        {
            base.NovaRota("", token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }

        public async Task<ResultModel> ObterAsync(int id, string token)
        {
            base.NovaRota("/" + id, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }
        #endregion

        #region InsereAsync
        public async Task<ResultModel> InsereAsync(T model, string token)
        {
            base.NovaRota("", token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", model));
        }
        #endregion

        #region UpdateAsync
        public async Task<ResultModel> UpdateAsync(int id, T model, string token)
        {
            base.NovaRota("/" + id, token);
            return base.Deserialize(await base.Client.PutAsJsonAsync<T>("", model));
        }
        #endregion

        #region RemoveAsync
        public async Task<ResultModel> RemoveAsync(int id, string token)
        {
            base.NovaRota("/" + id, token);
            return base.Deserialize(await base.Client.DeleteAsync(""));
        }
        #endregion
    }
}
