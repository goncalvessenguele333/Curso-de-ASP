using ControleDeEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace ControleDeEstoque.Web.Controllers
{
    public class ContaController : Controller
    {
        // GET: Conta
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModels login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var usuario = UsuarioModel.validarUsuario(login.Usuario,login.Senha);

            if (usuario !=null)
            {
                //FormsAuthentication.SetAuthCookie(usuario.nome, login.LembrarMe);

               var ticket= FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                   1, usuario.nome, DateTime.Now, DateTime.Now.AddHours(12), login.LembrarMe, PerfilModel.RecuperarPeloId(usuario.idPerfil).nome));

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);
                Response.Cookies.Add(cookie);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("","Usarname or Password incorrecto!");
            }

            return View(login);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
           return RedirectToAction("Login", "Conta");
        }
    }
}