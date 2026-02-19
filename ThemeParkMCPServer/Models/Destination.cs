namespace ThemeParkMCPServer.Models;

public sealed record Destination
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyList<Park> Parks { get; init; }
    public required string Slug { get; init; }
}