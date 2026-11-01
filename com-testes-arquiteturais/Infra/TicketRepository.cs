namespace Tickets.Infra;

public class TicketRepository
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, string> tickets = new();
    public void Save(Guid id, string title) => tickets[id] = title;
    public IReadOnlyDictionary<Guid, string> GetAll() => new Dictionary<Guid, string>(tickets);
}
