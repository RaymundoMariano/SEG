using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Client;
using SEG.Domain;
using SEG.Domain.Contracts.UnitOfWorks;
using SEG.Domain.Enums;
using SEG.Domain.Models.Aplicacao;
using System;
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

        private readonly IUnitOfWork _unitOfWork;
        public PerfisController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        // GET: PerfisController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(mensagem);

                var perfis = await _unitOfWork.Perfis.ObterAsync(Token);

                return View(perfis.FindAll(p => p.CreatedSystem == false).ToList());
            }
            catch { return Error(null); }
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

                var perfil = await _unitOfWork.Perfis.ObterAsync(id, Token);

                var funcaoId = (EFuncao)Seguranca.Perfil.FuncaoId == EFuncao.Presidência
                    ? Seguranca.Perfil.FuncaoId
                    : perfil.FuncaoId - 1;

                ViewBag.Funcoes = await _unitOfWork.Funcoes.ObterAsync((EFuncao)funcaoId, Token);

                return View(perfil);
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Create
        // GET: PerfisController/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                ViewBag.Funcoes = await _unitOfWork.Funcoes.ObterAsync((EFuncao)Seguranca.Perfil.FuncaoId, Token);
                return View();
            }
            catch (Exception) { return Error(null); }
        }

        // POST: PerfisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PerfilModel perfil)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Perfil", "Incluir");
                    if (mensagem != null) return Error(mensagem);

                    await _unitOfWork.Perfis.InsereAsync(perfil, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(perfil);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                ViewBag.Funcoes = await _unitOfWork.Funcoes.ObterAsync((EFuncao)Seguranca.Perfil.FuncaoId, Token);
                return View(perfil);
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Perfil", "Alterar");
                    if (mensagem != null) return Error(mensagem);

                    await _unitOfWork.Perfis.UpdateAsync(id, perfil, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(perfil);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(perfil);
            }
            catch (Exception) { return Error(null); }
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
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Perfis.RemoveAsync(id, Token);
                
                return RedirectToAction(nameof(Index));
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region EditRestricoes
        // GET: PerfisController/EditRestricoes
        [AllowAnonymous]
        public async Task<ActionResult> EditRestricoes(int perfilId)
        {
            try
            {
                ViewBag.Perfil = await _unitOfWork.Perfis.ObterAsync(perfilId, Token);
                
                return View(await _unitOfWork.Perfis.ObterRestricoesAsync(perfilId, Token));
            }
            catch (Exception) { return Error(null); }
        }

        // GET: PerfisController/EditRestricoes
        [HttpPost]
        public async Task<ActionResult> EditRestricoes(int perfilId, List<RestricaoPerfilModel> restricoesModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Perfil", "Associar Restricoes");
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Perfis.AtualizarRestricoesAsync(perfilId, restricoesModel, User.FindFirstValue("Token"));
                
                return RedirectToAction("Edit", new { Id = perfilId });
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
                ViewBag.ErrorTitle = "Perfil";
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}
