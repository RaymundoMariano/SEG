using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SEG.Domain.Models.Aplicacao
{
    public class ModuloModel : _Model
    {
        public ModuloModel()
        {
            ModulosFormulario = new HashSet<ModuloFormularioModel>();
            PerfisUsuario = new HashSet<PerfilUsuarioModel>();
            RestricoesPerfil = new HashSet<RestricaoPerfilModel>();
            RestricoesUsuario = new HashSet<RestricaoUsuarioModel>();
        }

        [DisplayName("Id")]
        public int ModuloId { get; set; }

        [DisplayName("Módulo")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "Limite de caracteres excedido!")]
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<ModuloFormularioModel> ModulosFormulario { get; set; }
        public virtual ICollection<PerfilUsuarioModel> PerfisUsuario { get; set; }
        public virtual ICollection<RestricaoPerfilModel> RestricoesPerfil { get; set; }
        public virtual ICollection<RestricaoUsuarioModel> RestricoesUsuario { get; set; }
    }
}
