namespace ThemeParkMCPServer.Models;

public sealed record Child
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string EntityType { get; init; }
    public required string? Slug { get; init; }
    public required string ParentId { get; init; }
    public required string ExternalId { get; init; }
}