using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Aplicacao
{
    public partial class PerfilModel : _Model
    {
        public PerfilModel()
        {
            PerfisUsuario = new HashSet<PerfilUsuarioModel>();
            RestricoesPerfil = new HashSet<RestricaoPerfilModel>();
        }

        [DisplayName("Id")]
        public int PerfilId { get; set; }

        [DisplayName("Perfil")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "Limite de caracteres excedido!")]
        public string Descricao { get; set; }

        [DisplayName("Função")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int FuncaoId { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual FuncaoModel Funcao { get; set; }
        public virtual ICollection<PerfilUsuarioModel> PerfisUsuario { get; set; }
        public virtual ICollection<RestricaoPerfilModel> RestricoesPerfil { get; set; }
    }
}
