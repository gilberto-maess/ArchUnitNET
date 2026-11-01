using Microsoft.AspNetCore.Mvc;
using Tickets.Aplicacao.Tickets.Services;
using Tickets.Dominio;
using Tickets.Infra;
namespace Tickets.Apresentacao;

[ApiController]
[Route("tickets")]
// Violação 3: o controller recebe e consulta um repositório diretamente.
public class TicketsController(TicketHandler handler, TicketRepository repository) : ControllerBase
{
    [HttpPost]
    public Ticket Create(CreateTicketRequest request) => handler.Create(request.Title);

    [HttpGet]
    public IReadOnlyDictionary<Guid, string> List() => repository.GetAll();
}

public record CreateTicketRequest(string Title);
