namespace SEG.Domain.Models.Aplicacao
{
    public partial class RestricaoUsuarioModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? ModuloId { get; set; }
        public int? FormularioId { get; set; }
        public int? EventoId { get; set; }

        public virtual EventoModel Evento { get; set; }
        public virtual FormularioModel Formulario { get; set; }
        public virtual ModuloModel Modulo { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
    }
}
