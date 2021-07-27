using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleDeEstoque.Web.Models;

namespace ControleDeEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente, Administrativo, Operador")]
    public class CadDistritoController : Controller
    {
        private const int _quantidadeMaximaDeLinhaPorPagina = 5;
        // GET: Cadastro

        public ActionResult Index()
        {
           // ViewBag.Provincia = ProvinciasModel.RecuperarProvinciasActivos();
            ViewBag.Paises = PaisModel.RecuperarPaisActivos();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantidadeMaximaDeLinhaPorPagina, 10, 15, 20 }, _quantidadeMaximaDeLinhaPorPagina);
            ViewBag.QuantidadeMaximaDeLinhaPorPagina = _quantidadeMaximaDeLinhaPorPagina;
            ViewBag.PaginaActual = 1;
            var lista = DistritoModel.RecuperarLista(ViewBag.PaginaActual, _quantidadeMaximaDeLinhaPorPagina);
            var quant = DistritoModel.RecuperarQuantidade();

            var difQuantPagina = (quant % ViewBag.QuantidadeMaximaDeLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPagina = (quant / ViewBag.QuantidadeMaximaDeLinhaPorPagina) + difQuantPagina;

            return View(lista);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult CadDistritoPagina(int pagina, int tamPag, string filtro)
        {
            var lista = DistritoModel.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCadDistrito(int id)
        {

            return Json(DistritoModel.RecuperarPeloId(id));
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente, Administrativo")]
        public JsonResult ExcluirCadDistrito(int id)
        {
            return Json(DistritoModel.ExcluirPeloId(id));
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult salvarCadDistrito(DistritoModel model)
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