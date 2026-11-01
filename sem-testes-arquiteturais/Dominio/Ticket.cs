using Tickets.Infra;
namespace Tickets.Dominio;

public class Ticket
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; }
    public Ticket(string title)
    {
        // Violação 2: o domínio depende de uma classe da infraestrutura.
        Title = TicketTitleFormatter.Normalize(title);
    }
}
