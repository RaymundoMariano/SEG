using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using SEG.Domain.Enums;
using SEG.Domain.Models.Aplicacao;
using SEG.Domain.Models.Response;
using SEG.Service;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SEG.UI.Controllers
{
    public class PerfisController : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IPerfilAplication _perfilClient;
        private readonly IFuncaoAplication _funcaoClient;
        public PerfisController(IPerfilAplication perfilClient, IFuncaoAplication funcaoClient)
        {
            _perfilClient = perfilClient;
            _funcaoClient = funcaoClient;
        }

        #region Index
        // GET: PerfisController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _perfilClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    var perfis = JsonConvert.DeserializeObject<List<PerfilModel>>(result.ObjectRetorno.ToString());
                    return View(perfis.FindAll(p => p.CreatedSystem == false).ToList());
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Details
        // GET: PerfisController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                ViewBag.Seguranca = Seguranca;

                var result = await _perfilClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var perfil = JsonConvert.DeserializeObject<PerfilModel>(result.ObjectRetorno.ToString());

                    var funcaoId = (EFuncao)Seguranca.Perfil.FuncaoId == EFuncao.Presidência
                        ? Seguranca.Perfil.FuncaoId
                        : perfil.FuncaoId - 1;
                    
                    result = await _funcaoClient.ObterAsync((EFuncao)funcaoId, Token);
                    if (result.Succeeded)
                    {
                        ViewBag.Funcoes = JsonConvert.DeserializeObject<List<FuncaoModel>>(result.ObjectRetorno.ToString());
                    }
                    return View(perfil);
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Create
        // GET: PerfisController/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var result = await _funcaoClient.ObterAsync((EFuncao)Seguranca.Perfil.FuncaoId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Funcoes = JsonConvert.DeserializeObject<List<FuncaoModel>>(result.ObjectRetorno.ToString());
                    return View();
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }

        // POST: PerfisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PerfilModel perfil)
        {
            var mensagem = Seguranca.TemPermissao("Perfil", "Incluir");
            if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

            var result = await _perfilClient.InsereAsync(perfil, Token);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
            {
                foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
               
                result = await _funcaoClient.ObterAsync((EFuncao)Seguranca.Perfil.FuncaoId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Funcoes = JsonConvert.DeserializeObject<List<FuncaoModel>>(result.ObjectRetorno.ToString());
                }

                return View(perfil);
            }

            return Error(result);
        }
        #endregion

        #region Edit
        // GET: PerfisController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: PerfisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PerfilModel perfil)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Perfil", "Alterar");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _perfilClient.UpdateAsync(id, perfil, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(perfil);
                }
                return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Delete
        // GET: PerfisController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: PerfisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, PerfilModel perfil)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Perfil", "Excluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _perfilClient.RemoveAsync(id, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region EditRestricoes
        // GET: PerfisController/EditRestricoes
        [AllowAnonymous]
        public async Task<ActionResult> EditRestricoes(int perfilId)
        {
            try
            {
                var result = await _perfilClient.ObterAsync(perfilId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Perfil = JsonConvert.DeserializeObject<PerfilModel>(result.ObjectRetorno.ToString());
                }
                else
                    return Error(result);

                result = await _perfilClient.ObterRestricoesAsync(perfilId, Token);
                if (result.Succeeded)
                {
                    var restricoes = JsonConvert.DeserializeObject<List<RestricaoPerfilModel>>(result.ObjectRetorno.ToString());
                    return View(restricoes);
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }

        // GET: PerfisController/EditRestricoes
        [HttpPost]
        public async Task<ActionResult> EditRestricoes(int perfilId, List<RestricaoPerfilModel> restricoesModel)
        {
            var mensagem = Seguranca.TemPermissao("Perfil", "Associar Restricoes");
            if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

            var result = await _perfilClient.AtualizarRestricoesAsync(perfilId, restricoesModel, User.FindFirstValue("Token"));
            if (result.Succeeded) return RedirectToAction("Edit", new { Id = perfilId });
            else
                return Error(result);
        }
        #endregion

        #region Error
        private ActionResult Error(ETipoErro eTipoErro, string mensagem)
        {
            return Error(new ResultModel()
            {
                ObjectResult = (eTipoErro == ETipoErro.Fatal) 
                    ? (int)EObjectResult.ErroFatal 
                    : (int)eTipoErro,
                Errors = new List<string>() { mensagem }
            });
        }

        private ActionResult Error(ResultModel result)
        {
            if (result.ObjectResult == (int)EObjectResult.ErroFatal)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = "Perfil";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}
