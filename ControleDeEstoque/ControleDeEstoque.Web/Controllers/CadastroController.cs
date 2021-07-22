using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleDeEstoque.Web.Models;

namespace ControleDeEstoque.Web.Controllers
{
    public class CadastroController : Controller
    {

        private const int _quantidadeMaximaDeLinhaPorPagina = 5;
        // GET: Cadastro
           
        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }
        [Authorize]
        public ActionResult LocalProduto()
        {
            return View();
        }
       
        [Authorize]
        public ActionResult Produto()
        {
            return View();
        }
        [Authorize]
        public ActionResult Pais()
        {
            return View();
        }
        [Authorize]
        public ActionResult Provincia()
        {
            return View();
        }
        [Authorize]
        public ActionResult Cidade()
        {
            return View();
        }
        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }
        



      

    }
}