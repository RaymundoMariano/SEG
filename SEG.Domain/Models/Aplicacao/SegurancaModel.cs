using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Aplicacao
{
    public class SegurancaModel : _Model
    {
        [Required]
        public UsuarioModel Usuario { get; set; }

        [Required]
        public ModuloModel Modulo { get; set; }

        [Required]
        public PerfilModel Perfil { get; set; }
    }
}
