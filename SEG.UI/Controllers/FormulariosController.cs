using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Client;
using SEG.Domain;
using SEG.Domain.Contracts.UnitOfWorks;
using SEG.Domain.Models.Aplicacao;
using System;
using System.Collections.Generic;
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

        private readonly IUnitOfWork _unitOfWork;
        public FormulariosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        // GET: FormulariosController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(mensagem);

                var formularios = await _unitOfWork.Formularios.ObterAsync(Token);

                return View(formularios.FindAll(f => f.CreatedSystem == false));
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Details
        // GET: FormulariosController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                return View(await _unitOfWork.Formularios.ObterAsync(id, Token));
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Formulario", "Incluir");
                    if (mensagem != null) return Error(mensagem);

                    await _unitOfWork.Formularios.InsereAsync(formulario, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(formulario);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(formulario);
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Formulario", "Alterar");
                    if (mensagem != null) return Error(mensagem);

                    await _unitOfWork.Formularios.UpdateAsync(id, formulario, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(formulario);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(formulario);
            }
            catch (Exception) { return Error(null); }
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
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Formularios.RemoveAsync(id, Token);

                return RedirectToAction(nameof(Index));
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region EditEventos
        // GET: FormulariosController/EditEventos
        [AllowAnonymous]
        public async Task<ActionResult> EditEventos(int formularioId)
        {
            try
            {
                ViewBag.Formulario = await _unitOfWork.Formularios.ObterAsync(formularioId, Token);

                return View(await _unitOfWork.Formularios.ObterEventosAsync(formularioId, Token));
            }
            catch (Exception) { return Error(null); }
        }

        // GET: FormulariosController/EditEventos
        [HttpPost]
        public async Task<ActionResult> EditEventos(int formularioId, List<EventoModel> eventosModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Formulario", "Associar Evento");
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Formularios.AtualizarEventosAsync(formularioId, eventosModel, Token);
                
                return RedirectToAction("Edit", new { Id = formularioId });
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
                ViewBag.ErrorTitle = "Formulario";
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}

