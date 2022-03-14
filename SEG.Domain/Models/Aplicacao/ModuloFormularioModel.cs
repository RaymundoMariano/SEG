namespace SEG.Domain.Models.Aplicacao
{
    public partial class ModuloFormularioModel
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public int FormularioId { get; set; }

        public virtual FormularioModel Formulario { get; set; }
        public virtual ModuloModel Modulo { get; set; }
    }
}
