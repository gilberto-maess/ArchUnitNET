using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Tickets.Apresentacao;
using Tickets.Aplicacao.Tickets.Services;
using Tickets.Dominio;
using Tickets.Infra;
using Xunit;
namespace Tickets.TestesArquiteturais;

public class ArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(typeof(TicketsController).Assembly, typeof(TicketHandler).Assembly,
            typeof(Ticket).Assembly, typeof(TicketRepository).Assembly).Build();

    [Fact]
    public void Nivel1_Servicos_devem_terminar_com_Service() =>
        Classes().That().ResideInNamespace("Tickets.Aplicacao.Tickets.Services")
            .Or().ResideInNamespace("Tickets.Aplicacao.Notifications.Services")
            .Should().HaveNameEndingWith("Service").Check(Architecture);

    [Fact]
    public void Nivel2_Dominio_nao_pode_depender_de_Infra() =>
        Types().That().ResideInNamespace("Tickets.Dominio")
            .Should().NotDependOnAny(Types().That().ResideInNamespace("Tickets.Infra"))
            .Check(Architecture);

    [Fact]
    public void Nivel3_Controllers_nao_podem_acessar_repositorios() =>
        Classes().That().HaveNameEndingWith("Controller")
            .Should().NotDependOnAny(Classes().That().HaveNameEndingWith("Repository"))
            .Check(Architecture);

    [Fact]
    public void Nivel4_Modulos_nao_podem_depender_um_do_outro()
    {
        var tickets = Types().That().ResideInNamespace("Tickets.Aplicacao.Tickets.Services");
        var notifications = Types().That().ResideInNamespace("Tickets.Aplicacao.Notifications.Services");
        tickets.Should().NotDependOnAny(notifications)
            .And(notifications.Should().NotDependOnAny(tickets)).Check(Architecture);
    }

    [Fact]
    public void Nivel5_Modulos_devem_respeitar_diagrama() =>
        Types().That().ResideInNamespace("Tickets.Aplicacao.Tickets.Services")
            .Or().ResideInNamespace("Tickets.Aplicacao.Notifications.Services")
            .Should().AdhereToPlantUmlDiagram(Path.Combine(AppContext.BaseDirectory, "arquitetura.puml"))
            .Check(Architecture);
}
