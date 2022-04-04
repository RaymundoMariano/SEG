using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Models.Aplicacao;

namespace SEG.Client.Aplicacao
{
    public class EventoClient : Client<EventoModel>, IEventoClient
    {
        public EventoClient() : base("https://localhost:44366/api/eventos") { }       
    }
}
