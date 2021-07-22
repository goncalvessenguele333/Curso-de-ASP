using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleDeEstoque.Web.Models;

namespace ControleDeEstoque.Web.Controllers
{
    public class CadUnidadeMedidaController : Controller
    {
        // GET: CadUnidadeMedida
        private const int _quantidadeMaximaDeLinhaPorPagina = 5;
        // GET: Cadastro
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantidadeMaximaDeLinhaPorPagina, 10, 15, 20 }, _quantidadeMaximaDeLinhaPorPagina);
            ViewBag.QuantidadeMaximaDeLinhaPorPagina = _quantidadeMaximaDeLinhaPorPagina;
            ViewBag.PaginaActual = 1;
            var lista = UnidadeMedidaModel.RecuperarLista(ViewBag.PaginaActual, _quantidadeMaximaDeLinhaPorPagina);
            var quant = UnidadeMedidaModel.RecuperarQuantidade();

            var difQuantPagina = (quant % ViewBag.QuantidadeMaximaDeLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPagina = (quant / ViewBag.QuantidadeMaximaDeLinhaPorPagina) + difQuantPagina;

            return View(lista);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UnidadeMedidaPagina(int pagina, int tamPag)
        {
            var lista = UnidadeMedidaModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarUnidadeMedida(int id)
        {

            return Json(UnidadeMedidaModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult salvarUnidadeMedida(UnidadeMedidaModel model)
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