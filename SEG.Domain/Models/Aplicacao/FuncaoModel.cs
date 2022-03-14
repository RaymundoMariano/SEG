using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Aplicacao
{
    public partial class FuncaoModel : _Model
    {
        [DisplayName("Id")]
        public int FuncaoId { get; set; }

        [DisplayName("Função")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual PerfilModel Perfil { get; set; }
    }
}
