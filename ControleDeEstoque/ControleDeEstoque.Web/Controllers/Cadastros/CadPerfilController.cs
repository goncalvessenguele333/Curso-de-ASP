using ControleDeEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleDeEstoque.Web.Controllers.Cadastros
{
    [Authorize(Roles = "Gerente")]
    public class CadPerfilController : Controller
    {
      
        // GET: CadPerfil
        private const int _quantidadeMaximaDeLinhaPorPagina = 5;
        // GET: Cadastro

        public ActionResult Index()
        {
         
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantidadeMaximaDeLinhaPorPagina, 10, 15, 20 }, _quantidadeMaximaDeLinhaPorPagina);
            ViewBag.QuantidadeMaximaDeLinhaPorPagina = _quantidadeMaximaDeLinhaPorPagina;
            ViewBag.PaginaActual = 1;

            var lista = PerfilModel.RecuperarLista(ViewBag.PaginaActual, _quantidadeMaximaDeLinhaPorPagina);
            var quant = PerfilModel.RecuperarQuantidade();

            var difQuantPagina = (quant % ViewBag.QuantidadeMaximaDeLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPagina = (quant / ViewBag.QuantidadeMaximaDeLinhaPorPagina) + difQuantPagina;

            return View(lista);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult PerfilPagina(int pagina, int tamPag)
        {
            var lista = PerfilModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult RecuperarPerfil(int id)
        {

            return Json(PerfilModel.RecuperarPeloId(id));
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
         public JsonResult ExcluirPerfil(int id)
        {
            return Json(PerfilModel.ExcluirPeloId(id));
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult salvarPerfil(PerfilModel model)
        {
            var resultado = "Ok";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;
            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception)
                {
                    resultado = "ERRO";
                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

    }
}