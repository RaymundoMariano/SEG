using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SEG.Domain.Contracts.Autenticacao;
using SEG.Domain.Contracts.Seguranca;
using SEG.Domain.Models.Response;
using SEG.Domain.Models.Autenticacao;
using SEG.Domain.Models.Aplicacao;
using SEG.Service;
using SEG.Domain.Enums;

namespace SEG.UI.Controllers.Autenticacao
{
    [AllowAnonymous]
    public class ContaController : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Email { get { return User.FindFirst(ClaimTypes.Email).Value; } }
        private readonly IRegisterAuthentication _register;
        private readonly ILoginAuthentication _login;
        private readonly IUsuarioSecurity _usuario;
        private readonly ITrocaSenhaAuthentication _trocaSenha;
        public ContaController(IRegisterAuthentication register
            , ILoginAuthentication login
            , ITrocaSenhaAuthentication trocaSenha
            , IUsuarioSecurity usuario)
        {
            _register = register;
            _login = login;
            _trocaSenha = trocaSenha;
            _usuario = usuario;
        }

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(Domain.Models.Autenticacao.RegisterModel register)
        {
            var result = await _register.RegisterAsync(register);

            foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }

            if (ModelState.IsValid)
            {
                var reg = JsonConvert.DeserializeObject<Domain.Models.Response.RegisterModel>(result.ObjectRetorno.ToString());

                var usuario = new UsuarioModel()
                {
                    Email = register.Email,
                    Nome = register.Nome
                };

                result = await _usuario.ObterPerfilAsync("SegurancaNet", usuario, reg.Token);

                foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }

                if (ModelState.IsValid)
                {
                    reg.Seguranca = JsonConvert.DeserializeObject<SegurancaModel>(result.ObjectRetorno.ToString());

                    if (!await Login(reg, "Register")) return View("Error");
                }

                return RedirectToAction("Index", "Home");
            }
            return View(register);
        }
        #endregion

        #region Login
        [HttpGet]
        public ActionResult Login(string returnURL)
        {
            if (User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); }

            var model = new LoginModel() { ReturnUrl = returnURL };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login)
        {
            var result = await _login.LoginAsync(login);

            foreach (var erro in result.Errors) { ModelState.AddModelError("Email", erro); }

            if (ModelState.IsValid)
            {
                var register = JsonConvert.DeserializeObject<Domain.Models.Response.RegisterModel>(result.ObjectRetorno.ToString());
                
                var usuario = new UsuarioModel()
                {
                    Email = register.Email,
                    Nome = register.Nome
                };

                result = await _usuario.ObterPerfilAsync("SegurancaNet", usuario, register.Token);

                foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }

                if (ModelState.IsValid)
                {
                    register.Seguranca = JsonConvert.DeserializeObject<SegurancaModel>(result.ObjectRetorno.ToString());

                    if (!await Login(register, "Login")) return View("Error");
                }

                if (!string.IsNullOrEmpty(login.ReturnUrl) && Url.IsLocalUrl(login.ReturnUrl))
                {
                    return Redirect(login.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
        #endregion

        #region TrocaSenha
        [HttpGet]
        public ActionResult TrocaSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> TrocaSenha(TrocaSenhaModel trocaSenha)
        {
            if (trocaSenha.NovaSenha != trocaSenha.ConfirmeSenha)
            {
                ModelState.AddModelError("NovaSenha", "Nova senha é diferente da confirmação!");
                return View(trocaSenha);
            }

            trocaSenha.Email = Email;
            var result = await _trocaSenha.TrocaSenhaAsync(trocaSenha);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            foreach (var erro in result.Errors) { ModelState.AddModelError("SenhaAtual", erro); }
            return View(trocaSenha);
        }
        #endregion

        #region LogOut
        [HttpGet]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Conta");
        }
        #endregion

        private async Task<bool> Login(Domain.Models.Response.RegisterModel register, string title)
        {
            Seguranca seguranca;
            try
            {
                seguranca = new (
                    register.Seguranca.Usuario, register.Seguranca.Modulo, register.Seguranca.Perfil);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, register.Nome),
                    new Claim(ClaimTypes.Email, register.Email),
                    new Claim(ClaimTypes.Role, "Usuario_Comum"),
                    new Claim("Perfil", seguranca.Perfil.Nome),
                    new Claim("Token", register.Token),
                    new Claim("Seguranca", JsonConvert.SerializeObject(seguranca))
                };

                var identity = new ClaimsIdentity(claims, "Login");
                var claimPrincipal = new ClaimsPrincipal(identity);

                var auth = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, auth);

                return true;
            }
            catch (Exception) { return false; }
        }

        #region Error
        private void Error(ResultModel result, string title)
        {
            if (result.ObjectResult == (int)EObjectResult.ErroFatal)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = title;
                ViewBag.ErrorMessage = result.Errors[0];
            }
            var x = 1; var y = 0; x = x / y;
        }
        #endregion
    }
}
