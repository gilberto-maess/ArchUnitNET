using Tickets.Aplicacao.Tickets.Services;
using Tickets.Aplicacao.Notifications.Services;
using Tickets.Apresentacao;
using Tickets.Dominio;
using Tickets.Infra;
using Xunit;
namespace Tickets.TestesUnitarios;

public class TicketTests
{
    [Fact]
    public void Criar_chamado_armazena_titulo()
    {
        var repository = new TicketRepository();
        var handler = new TicketHandler(repository, new NotificationService());
        var ticket = handler.Create("Impressora indisponível");
        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal(ticket.Title, repository.GetAll()[ticket.Id]);
    }

    [Fact]
    public void Normalizar_titulo_remove_espacos()
    {
        Assert.Equal("Falha de acesso", new Ticket("  Falha de acesso  ").Title);
    }

    [Fact]
    public void Listar_retorna_chamados_armazenados()
    {
        var repository = new TicketRepository();
        var controller = new TicketsController(new TicketHandler(repository, new NotificationService()), repository);
        Assert.Empty(controller.List());
        var ticket = controller.Create(new CreateTicketRequest("Falha de acesso"));
        Assert.Single(controller.List());
        Assert.Equal(ticket.Title, controller.List()[ticket.Id]);
    }

    [Fact]
    public void Notificar_produz_mensagem()
    {
        Assert.Equal("Chamado criado: Falha de acesso", new NotificationService().Notify("Falha de acesso"));
    }
}
