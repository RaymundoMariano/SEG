using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Autenticacao
{
    public class TrocaSenhaModel
    {
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Senha atual")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(40, ErrorMessage = "limite de caracteres excedido")]
        [DataType(DataType.Password)]
        public string SenhaAtual { get; set; }

        [DisplayName("Nova senha")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(40, ErrorMessage = "limite de caracteres excedido")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [DisplayName("Confirme nova senha")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(40, ErrorMessage = "limite de caracteres excedido")]
        [DataType(DataType.Password)]
        public string ConfirmeSenha { get; set; }
    }
}
