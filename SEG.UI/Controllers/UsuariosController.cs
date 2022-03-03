using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Enums;
using Seguranca.Domain.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SEG.UI.Controllers
{
    public class UsuariosController : Controller
    {
        private Seguranca.Service.Seguranca Seguranca
        {
            get
            {
                return JsonConvert
                    .DeserializeObject<Seguranca.Service.Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IUsuarioClient _usuarioClient;
        private readonly IPerfilClient _perfilClient;
        public UsuariosController(IUsuarioClient usuarioClient, IPerfilClient perfilClient)
        {
            _usuarioClient = usuarioClient;
            _perfilClient = perfilClient;
        }

        #region Index
        // GET: UsuariosController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _usuarioClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(result.ObjectRetorno.ToString());
                    return View(usuarios);
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
        // GET: UsuariosController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                ViewBag.Seguranca = Seguranca;

                var result = await _usuarioClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var usuario = JsonConvert.DeserializeObject<UsuarioModel>(result.ObjectRetorno.ToString());
                    return View(usuario);
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
                var result = await _usuarioClient.ObterAsync(usuarioId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Usuario = JsonConvert.DeserializeObject<UsuarioModel>(result.ObjectRetorno.ToString());
                }
                else
                    return Error(result);

                result = await _usuarioClient.ObterRestricoesAsync(usuarioId, Token);
                if (result.Succeeded)
                {
                    var restricoes = JsonConvert.DeserializeObject<List<RestricaoUsuarioModel>>(result.ObjectRetorno.ToString());
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

        // GET: UsuariosController/EditRestricoes
        [HttpPost]
        public async Task<ActionResult> EditRestricoes(int usuarioId, List<RestricaoUsuarioModel> restricoesModel)
        {
            try
            {
                //var mensagem = Seguranca.TemPermissao("Usuario", "Associar Restricoes");
                //if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _usuarioClient.AtualizarRestricoesAsync(usuarioId, restricoesModel, Token);
                if (result.Succeeded) return RedirectToAction("Edit", new { Id = usuarioId });
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region EditPerfis
        // GET: UsuariosController/EditPerfis
        [AllowAnonymous]
        public async Task<ActionResult> EditPerfis(int usuarioId)
        {
            try
            {
                var result = await _usuarioClient.ObterAsync(usuarioId, Token);
                if (result.Succeeded)
                {
                    var usuario = JsonConvert.DeserializeObject<UsuarioModel>(result.ObjectRetorno.ToString());
                    ViewBag.Usuario = usuario;
                }
                else
                    return Error(result);

                result = await _perfilClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    ViewBag.Perfis = JsonConvert.DeserializeObject<List<PerfilModel>>(result.ObjectRetorno.ToString());
                }
                else
                    return Error(result);

                result = await _usuarioClient.ObterPerfisAsync(usuarioId, Token);
                if (result.Succeeded)
                {
                    var perfisUsuario = JsonConvert.DeserializeObject<List<PerfilUsuarioModel>>(result.ObjectRetorno.ToString());
                    return View(perfisUsuario);
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }

        // GET: UsuariosController/EditPerfis
        [HttpPost]
        public async Task<ActionResult> EditPerfis(int usuarioId, List<PerfilUsuarioModel> perfisModel)
        {
            try
            {
                //var mensagem = Seguranca.TemPermissao("Usuario", "Associar Perfil");
                //if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _usuarioClient.AtualizarPerfisAsync(usuarioId, perfisModel, Token);
                if (result.Succeeded) return RedirectToAction("Details", new { Id = usuarioId });
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
                ViewBag.ErrorTitle = "Usuário";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}
