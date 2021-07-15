using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleDeEstoque.Web.Controllers
{
    public class GraficoController : Controller
    {
        // GET: Grafico
        [Authorize]
        public ActionResult PerdasMes()
        {
            return View();
        }
        [Authorize]
        public ActionResult EntradasSaidasMes()
        {
            return View();
        }
    }
}