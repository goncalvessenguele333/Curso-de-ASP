using ControleDeEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleDeEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class CadUsuarioController: Controller
    {

        private const int _quantidadeMaximaDeLinhaPorPagina = 5;
        // GET: Cadastro
           
        #region Usuario
        private const string _senhaPadrao = "{111@kjvfd}";
     
        public ActionResult Index()
        {
            ViewBag.ListaPeril = PerfilModel.RecuperarListaActivos();
            ViewBag.senhaPadrao = _senhaPadrao;
             ViewBag.ListaTamPag = new SelectList(new int[] { _quantidadeMaximaDeLinhaPorPagina, 10, 15, 20 }, _quantidadeMaximaDeLinhaPorPagina);
            ViewBag.QuantidadeMaximaDeLinhaPorPagina = _quantidadeMaximaDeLinhaPorPagina;
            ViewBag.PaginaActual = 1;
            var lista = UsuarioModel.RecuperarLista(ViewBag.PaginaActual, _quantidadeMaximaDeLinhaPorPagina);
            var quant = UsuarioModel.RecuperarQuantidade();

            var difQuantPagina = (quant % ViewBag.QuantidadeMaximaDeLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPagina = (quant / ViewBag.QuantidadeMaximaDeLinhaPorPagina) + difQuantPagina;

            return View(lista);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public JsonResult GrupoProdutoPagina(int pagina, int tamPag)
        {
            var lista = UsuarioModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {

            return Json(UsuarioModel.RecuperarPeloId(id));
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPeloId(id));
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult salvarUsuario(UsuarioModel model)
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
                    if (model.senha == _senhaPadrao)
                    {
                        model.senha = "";
                    }
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
        #endregion Usuario

    }
}