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
    public class UsuariosController : Controller
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
        public UsuariosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        // GET: UsuariosController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(mensagem);

                return View(await _unitOfWork.Usuarios.ObterAsync(Token));
            }
            catch { return Error(null); }
        }
        #endregion

        #region Details
        // GET: UsuariosController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                ViewBag.Seguranca = Seguranca;

                return View(await _unitOfWork.Usuarios.ObterAsync(id, Token));
            }
            catch { return Error(null); }
        }
        #endregion

        #region Edit
        // GET: UsuariosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }
        #endregion

        #region EditRestricoes
        // GET: UsuariosController/EditRestricoes
        [AllowAnonymous]
        public async Task<ActionResult> EditRestricoes(int usuarioId)
        {
            try
            {
                ViewBag.Usuario = await _unitOfWork.Usuarios.ObterAsync(usuarioId, Token);
                
                return View(await _unitOfWork.Usuarios.ObterRestricoesAsync(usuarioId, Token));
            }
            catch { return Error(null); }
        }

        // GET: UsuariosController/EditRestricoes
        [HttpPost]
        public async Task<ActionResult> EditRestricoes(int usuarioId, List<RestricaoUsuarioModel> restricoesModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Usuario", "Associar Restricoes");
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Usuarios.AtualizarRestricoesAsync(usuarioId, restricoesModel, Token);
                
                return RedirectToAction("Edit", new { Id = usuarioId });
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region EditPerfis
        // GET: UsuariosController/EditPerfis
        [AllowAnonymous]
        public async Task<ActionResult> EditPerfis(int usuarioId)
        {
            try
            {
                ViewBag.Usuario = await _unitOfWork.Usuarios.ObterAsync(usuarioId, Token);

                ViewBag.Perfis = await _unitOfWork.Perfis.ObterAsync(Token);
                
                return View(await _unitOfWork.Usuarios.ObterPerfisAsync(usuarioId, Token));
            }
            catch (Exception) { return Error(null); }
        }

        // GET: UsuariosController/EditPerfis
        [HttpPost]
        public async Task<ActionResult> EditPerfis(int usuarioId, List<PerfilUsuarioModel> perfisModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Usuario", "Associar Perfil");
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Usuarios.AtualizarPerfisAsync(usuarioId, perfisModel, Token);
                
                return RedirectToAction("Details", new { Id = usuarioId });
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
                ViewBag.ErrorTitle = "Usuario";
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}
