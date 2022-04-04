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
using SEG.Domain.Models.Response;
using SEG.Domain.Models.Autenticacao;
using SEG.Domain;
using SEG.Domain.Contracts.Clients.Autenticacao;
using SEG.Domain.Contracts.Clients.Seguranca;
using SEG.Client;

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

        private readonly IAutenticacaoClient _autenticacaoClient;
        private readonly ISegurancaClient _segurancaClient;
        public ContaController(IAutenticacaoClient autenticacaoClient, ISegurancaClient segurancaClient)
        {
            _autenticacaoClient = autenticacaoClient;
            _segurancaClient = segurancaClient;
        }

        #region Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel register)
        {
            var registro = new RegistroModel();
            try
            {
                if (ModelState.IsValid)
                {
                    registro = await _autenticacaoClient.RegisterAsync(register);

                    foreach (var erro in registro.Errors) { ModelState.AddModelError("UserName", erro); }

                    if (ModelState.IsValid)
                    {
                        registro.Seguranca = await _segurancaClient.ObterPerfilAsync("SegurancaNet", registro);
                        
                        if (!await Autenticado(registro, "Register")) return View("Error");
                    }
                    else
                    {
                        return View(register);
                    }

                    return RedirectToAction("Index", "Home");
                }
                return View(register);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("UserName", ex.Message);
                return View(register);
            }
            catch (Exception) { return Error(null, null); }
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
            var registro = new RegistroModel();
            try
            {
                if (ModelState.IsValid)
                {
                    registro = await _autenticacaoClient.LoginAsync(login);

                    foreach (var erro in registro.Errors) { ModelState.AddModelError("Email", erro); }

                    if (ModelState.IsValid)
                    {
                        registro.Seguranca = await _segurancaClient.ObterPerfilAsync("SegurancaNet", registro);

                        if (!await Autenticado(registro, "Login")) return View("Error");
                    }
                    else
                    {
                        return View(login);
                    }

                    if (!string.IsNullOrEmpty(login.ReturnUrl) && Url.IsLocalUrl(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                return View(login);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(login);
            }
            catch (Exception) { return Error(null, null); }
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
            trocaSenha.Email = Email;

            try
            {
                if (ModelState.IsValid)
                {
                    var errors = await _autenticacaoClient.TrocaSenhaAsync(trocaSenha);

                    foreach (var erro in errors) { ModelState.AddModelError("SenhaAtual", erro); }

                    if (ModelState.IsValid) return RedirectToAction("Index", "Home");
                }
                return View(trocaSenha);
            }
            catch (ClientException ex) 
            { 
                ModelState.AddModelError("SenhaAtual", ex.Message);
                return View(trocaSenha);
            }
            catch (Exception) { return Error(null, null); }
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

        #region Autenticado
        private async Task<bool> Autenticado(RegistroModel register, string title)
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
        #endregion

        #region Error
        private ActionResult Error(string mensagem, string title)
        {
            if (mensagem == null)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = title;
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}
