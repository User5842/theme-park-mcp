namespace ThemeParkMCPServer.Models;

public sealed record Location
{
    public required double Longitude { get; init; }
    public required double Latitude { get; init; }
}