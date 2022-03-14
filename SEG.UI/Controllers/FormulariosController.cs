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
    public class FormulariosController : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IFormularioAlication _formularioClient;
        public FormulariosController(IFormularioAlication formularioClient)
        {
            _formularioClient = formularioClient;
        }

        #region Index
        // GET: FormulariosController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _formularioClient.ObterAsync(Token);
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
        #endregion

        #region Details
        // GET: FormulariosController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var result = await _formularioClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var formulario = JsonConvert.DeserializeObject<FormularioModel>(result.ObjectRetorno.ToString());
                    return View(formulario);
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
        // GET: FormulariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormulariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormularioModel formulario)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Formulario", "Incluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _formularioClient.InsereAsync(formulario, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(formulario);
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
        // GET: FormulariosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: FormulariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, FormularioModel formulario)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Formulario", "Alterar");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _formularioClient.UpdateAsync(id, formulario, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(formulario);
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
        // GET: FormulariosController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: FormulariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, FormularioModel formulario)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Formulario", "Excluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _formularioClient.RemoveAsync(id, Token);
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

        #region EditEventos
        // GET: FormulariosController/EditEventos
        [AllowAnonymous]
        public async Task<ActionResult> EditEventos(int formularioId)
        {
            try
            {
                var result = await _formularioClient.ObterAsync(formularioId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Formulario = JsonConvert.DeserializeObject<FormularioModel>(result.ObjectRetorno.ToString());
                }
                else
                    return Error(result);

                result = await _formularioClient.ObterEventosAsync(formularioId, Token);
                if (result.Succeeded)
                {
                    var eventos = JsonConvert.DeserializeObject<List<EventoModel>>(result.ObjectRetorno.ToString());
                    return View(eventos);
                }
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }

        // GET: FormulariosController/EditEventos
        [HttpPost]
        public async Task<ActionResult> EditEventos(int formularioId, List<EventoModel> eventosModel)
        {
            var mensagem = Seguranca.TemPermissao("Formulario", "Associar Evento");
            if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

            var result = await _formularioClient.AtualizarEventosAsync(formularioId, eventosModel, Token);
            if (result.Succeeded) return RedirectToAction("Edit", new { Id = formularioId });
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
                ViewBag.ErrorTitle = "Formulário";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}

