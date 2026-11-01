using Tickets.Aplicacao.Tickets.Services;
using Tickets.Aplicacao.Notifications.Services;
using Tickets.Infra;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<TicketRepository>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddTransient<TicketHandler>();
var app = builder.Build();
app.MapControllers();
app.Run();
