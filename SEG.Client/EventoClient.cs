using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Models;

namespace SEG.Client
{
    public class EventoClient : Client<EventoModel>, IEventoClient
    {
        public EventoClient() : base("https://localhost:44366/api/eventos") { }       
    }
}
