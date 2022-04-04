using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Client;
using SEG.Domain;
using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SEG.UI.Controllers
{
    public class ModulosController : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
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
                if (mensagem != null) return Error(mensagem);

                var modulos = await _moduloClient.ObterAsync(Token);

                return View(modulos.FindAll(m => m.CreatedSystem == false).ToList());
            }
            catch { return Error(null); }
        }
        #endregion

        #region Details
        // GET: ModulosController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                return View(await _moduloClient.ObterAsync(id, Token));
            }
            catch { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Modulo", "Incluir");
                    if (mensagem != null) return Error(mensagem);

                    await _moduloClient.InsereAsync(modulo, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(modulo);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(modulo);
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Modulo", "Alterar");
                    if (mensagem != null) return Error(mensagem);

                    await _moduloClient.UpdateAsync(id, modulo, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(modulo);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(modulo);
            }
            catch (Exception) { return Error(null); }
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
                if (mensagem != null) return Error(mensagem);

                await _moduloClient.RemoveAsync(id, Token);

                return RedirectToAction(nameof(Index));
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region EditFormularios
        // GET: ModulosController/EditFormularios
        [AllowAnonymous]
        public async Task<ActionResult> EditFormularios(int moduloId)
        {
            try
            {
                ViewBag.Modulo = await _moduloClient.ObterAsync(moduloId, Token);
                
                var formularios = await _moduloClient.ObterFormulariosAsync(moduloId, Token);
                
                return View(formularios.FindAll(f => f.CreatedSystem == false).ToList());
            }
            catch (Exception) { return Error(null); }
        }

        // GET: ModulosController/EditFormularios
        [HttpPost]
        public async Task<ActionResult> EditFormularios(int moduloId, List<FormularioModel> formulariosModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Modulo", "Associar Formulario");
                if (mensagem != null) return Error(mensagem);

                await _moduloClient.AtualizarFormulariosAsync(moduloId, formulariosModel, Token);
                
                return RedirectToAction("Edit", new { Id = moduloId });
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }

        }
        #endregion

        #region Error
        private ActionResult Error(string mensagem)
        {
            if (mensagem == null)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = "Modulo";
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}
