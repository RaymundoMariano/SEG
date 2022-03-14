namespace SEG.Domain.Models.Aplicacao
{
    public partial class RestricaoPerfilModel
    {
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public int? ModuloId { get; set; }
        public int? FormularioId { get; set; }
        public int? EventoId { get; set; }

        public virtual EventoModel Evento { get; set; }
        public virtual FormularioModel Formulario { get; set; }
        public virtual ModuloModel Modulo { get; set; }
        public virtual PerfilModel Perfil { get; set; }
    }
}
