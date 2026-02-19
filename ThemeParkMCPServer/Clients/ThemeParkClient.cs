using System.Net.Http.Json;
using ThemeParkMCPServer.Models;

namespace ThemeParkMCPServer.Clients;

public sealed class ThemeParkClient(HttpClient client) : IThemeParkClient
{
    public async Task<IReadOnlyList<Destination>> GetDestinations(CancellationToken ct = default)
    {
        var response = await client.GetFromJsonAsync<Items>("destinations", ct);
        var items = response?.Destinations ?? [];
        return items;
    }

    public async Task<Entity> GetEntity(string destinationId, CancellationToken ct = default) =>
        await client.GetFromJsonAsync<Entity>($"entity/{destinationId}/children", ct)
            ?? throw new InvalidOperationException("No entity found for the given destination ID");
}