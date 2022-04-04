using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Client;
using SEG.Domain;
using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Models.Aplicacao;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SEG.UI.Controllers
{
    public class EventosController : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IEventoClient _eventoClient;
        public EventosController(IEventoClient eventoClient)
        {
            _eventoClient = eventoClient;
        }

        #region Index
        // GET: EventosController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(mensagem);

                var eventos = await _eventoClient.ObterAsync(Token);

                return View(eventos.FindAll(e => e.CreatedSystem == false));
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Details
        // GET: EventosController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                return View(await _eventoClient.ObterAsync(id, Token));
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Create
        // GET: EventosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventoModel evento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Evento", "Incluir");
                    if (mensagem != null) return Error(mensagem);

                    await _eventoClient.InsereAsync(evento, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(evento);
            }
            catch (ClientException ex) 
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(evento);
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Edit
        // GET: EventosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: EventosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EventoModel evento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Evento", "Alterar");
                    if (mensagem != null) return Error(mensagem);

                    await _eventoClient.UpdateAsync(id, evento, Token);
                    
                    return RedirectToAction(nameof(Index));
                }
                return View(evento);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(evento);
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Delete
        // GET: EventosController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: EventosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, EventoModel evento)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Evento", "Excluir");
                if (mensagem != null) return Error(mensagem);

                await _eventoClient.RemoveAsync(id, Token);

                return RedirectToAction(nameof(Index));
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
                ViewBag.ErrorTitle = "Evento";
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}

