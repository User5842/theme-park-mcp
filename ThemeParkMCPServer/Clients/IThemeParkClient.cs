using ThemeParkMCPServer.Models;

namespace ThemeParkMCPServer.Clients;

public interface IThemeParkClient
{
    Task<IReadOnlyList<Destination>> GetDestinations(CancellationToken ct = default);

    Task<Entity> GetEntity(string destinationId, CancellationToken ct = default);
}