namespace SEG.Domain.Models.Aplicacao
{
    public partial class PerfilUsuarioModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ModuloId { get; set; }
        public int? PerfilId { get; set; }

        public virtual ModuloModel Modulo { get; set; }
        public virtual PerfilModel Perfil { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
    }
}
