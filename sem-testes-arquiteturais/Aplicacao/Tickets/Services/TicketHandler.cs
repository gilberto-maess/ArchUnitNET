using Tickets.Dominio;
using Tickets.Infra;
using Tickets.Aplicacao.Notifications.Services;
namespace Tickets.Aplicacao.Tickets.Services;

// Violação 1: uma classe no namespace de serviços deveria terminar com Service.
public class TicketHandler(TicketRepository repository, NotificationService notifications)
{
    public Ticket Create(string title)
    {
        var ticket = new Ticket(title);
        repository.Save(ticket.Id, ticket.Title);
        // Violações 4 e 5: Tickets acessa Notifications diretamente, contra a regra e o diagrama.
        notifications.Notify(ticket.Title);
        return ticket;
    }
}
