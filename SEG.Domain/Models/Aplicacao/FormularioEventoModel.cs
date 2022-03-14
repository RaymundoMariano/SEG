namespace SEG.Domain.Models.Aplicacao
{
    public partial class FormularioEventoModel
    {
        public int Id { get; set; }
        public int FormularioId { get; set; }
        public int EventoId { get; set; }

        public virtual EventoModel Evento { get; set; }
        public virtual FormularioModel Formulario { get; set; }
    }
}
