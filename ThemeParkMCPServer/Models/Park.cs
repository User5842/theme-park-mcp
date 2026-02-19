namespace ThemeParkMCPServer.Models;

public sealed record Park
{
    public required string Id { get; init; }
    public required string Name { get; init; }
}