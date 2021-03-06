using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Security.Principal;
using System.Web.Security;


namespace ControleDeEstoque.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        void Application_Error(Object Sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if(ex is HttpRequestValidationException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                Response.Write("{\"Resultado\":\"AVISO\",\"Mensagens\":[\"Somente texto sem caracteres especiais podem ser enviados.\"],\"IdSalvo\":\"\"}");
                Response.End();
            }
            else if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.End();

            }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null && cookie.Value != string.Empty)
            {
                FormsAuthenticationTicket ticket;
                try
                {
                    ticket = FormsAuthentication.Decrypt(cookie.Value);
                }
                catch
                {
                    return;
                }

                //var partes = ticket.UserData.Split('|');
               // var id = Convert.ToInt32(partes[0]);
                var perfis = ticket.UserData.Split(';');

                if (Context.User != null)
                {
                    Context.User = new GenericPrincipal(Context.User.Identity, perfis);
                }
            }
        }
    }
}
