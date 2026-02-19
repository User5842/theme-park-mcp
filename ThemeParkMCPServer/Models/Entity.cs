namespace ThemeParkMCPServer.Models;

public sealed record Entity
{
    public required IReadOnlyList<Child> Children { get; init; }
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string EntityType { get; init; }
    public required string TimeZone { get; init; }
}