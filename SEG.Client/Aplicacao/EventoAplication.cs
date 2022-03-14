using SEG.Domain.Contracts.Clients;
using SEG.Domain.Models.Aplicacao;

namespace SEG.Client.Aplicacao
{
    public class EventoAplication : Aplication<EventoModel>, IEventoAplication
    {
        public EventoAplication() : base("https://localhost:44366/api/eventos") { }       
    }
}
