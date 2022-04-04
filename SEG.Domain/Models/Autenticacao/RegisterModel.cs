using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Autenticacao
{
    public class RegisterModel
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(40, ErrorMessage = "limite de caracteres excedido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
