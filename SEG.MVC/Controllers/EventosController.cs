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
    public class EventosController : Controller
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
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _eventoClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    var eventos = JsonConvert.DeserializeObject<List<EventoModel>>(result.ObjectRetorno.ToString());
                    return View(eventos.FindAll(e => e.CreatedSystem == false).ToList());
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
        // GET: EventosController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var result = await _eventoClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var evento = JsonConvert.DeserializeObject<EventoModel>(result.ObjectRetorno.ToString());
                    return View(evento);
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
                var mensagem = Seguranca.TemPermissao("Evento", "Incluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _eventoClient.InsereAsync(evento, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(evento);
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
                var mensagem = Seguranca.TemPermissao("Evento", "Alterar");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _eventoClient.UpdateAsync(id, evento, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(evento);
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
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _eventoClient.RemoveAsync(id, Token);
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
                ViewBag.ErrorTitle = "Evento";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}

