using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Aplicacao
{
    public class EventoModel : _Model
    {
        [DisplayName("Id")]
        public int EventoId { get; set; }

        [DisplayName("Evento")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "Limite de caracteres excedido!")]
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }
    }
}
