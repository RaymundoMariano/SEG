using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Enums;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SEG.MVC.Controllers
{
    public class ModulosController : Controller
    {
        private Seguranca.Domain.Seguranca Seguranca
        {
            get
            {
                return JsonConvert
                    .DeserializeObject<Seguranca.Domain.Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IModuloClient _moduloClient;
        public ModulosController(IModuloClient moduloClient)
        {
            _moduloClient = moduloClient;
        }

        #region Index
        // GET: ModulosController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _moduloClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    var modulos = JsonConvert.DeserializeObject<List<ModuloModel>>(result.ObjectRetorno.ToString());
                    return View(modulos.FindAll(m => m.CreatedSystem == false).ToList());
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
        // GET: ModulosController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var result = await _moduloClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var modulo = JsonConvert.DeserializeObject<ModuloModel>(result.ObjectRetorno.ToString());
                    return View(modulo);
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
        // GET: ModulosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModulosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ModuloModel modulo)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Modulo", "Incluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _moduloClient.InsereAsync(modulo, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(modulo);
                }
                return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Edit
        // GET: ModulosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: ModulosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ModuloModel modulo)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Modulo", "Alterar");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _moduloClient.UpdateAsync(id, modulo, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(modulo);
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
        // GET: ModulosController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: ModulosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ModuloModel modulo)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Modulo", "Excluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _moduloClient.RemoveAsync(id, Token);
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

        #region EditFormularios
        // GET: ModulosController/EditFormularios
        [AllowAnonymous]
        public async Task<ActionResult> EditFormularios(int moduloId)
        {
            try
            {
                var result = await _moduloClient.ObterAsync(moduloId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Modulo = JsonConvert.DeserializeObject<ModuloModel>(result.ObjectRetorno.ToString());
                }
                else
                    return Error(result);

                result = await _moduloClient.ObterFormulariosAsync(moduloId, Token);
                if (result.Succeeded)
                {
                    var formularios = JsonConvert.DeserializeObject<List<FormularioModel>>(result.ObjectRetorno.ToString());
                    return View(formularios.FindAll(f => f.CreatedSystem == false).ToList());
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }

        // GET: ModulosController/EditFormularios
        [HttpPost]
        public async Task<ActionResult> EditFormularios(int moduloId, List<FormularioModel> formulariosModel)
        {
            var mensagem = Seguranca.TemPermissao("Modulo", "Associar Formulario");
            if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

            var result = await _moduloClient.AtualizarFormulariosAsync(moduloId, formulariosModel, Token);
            if (result.Succeeded) return RedirectToAction("Edit", new { Id = moduloId });
            else
                return Error(result);
        }
        #endregion

        #region Error
        private ActionResult Error(ETipoErro eTipoErro, string mensagem)
        {
            return Error(new ResultResponse()
            {
                ObjectResult = (eTipoErro == ETipoErro.Fatal)
                    ? (int)EObjectResult.ErroFatal
                    : (int)eTipoErro,
                Errors = new List<string>() { mensagem }
            });
        }

        private ActionResult Error(ResultResponse result)
        {
            if (result.ObjectResult == (int)EObjectResult.ErroFatal)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = "Módulo";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}
