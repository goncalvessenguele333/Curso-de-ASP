using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControleDeEstoque.Web.Models
{
    public class LoginViewModels
    {
        [Required(ErrorMessage ="Informe o usuário")]
        [Display(Name="Usuário")]
        public string Usuario { get; set; }

        [Required(ErrorMessage ="Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Lembra Me")]
        public bool LembrarMe { get; set; }

    }
}