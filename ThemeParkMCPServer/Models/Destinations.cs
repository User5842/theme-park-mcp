namespace ThemeParkMCPServer.Models;

public sealed record Items
{
    public required IReadOnlyList<Destination> Destinations { get; init; }
}