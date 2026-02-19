using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using ThemeParkMCPServer.Clients;

namespace ThemeParkServer.Tools;

[McpServerToolType]
public static class ThemeParkTools
{
    [McpServerTool, Description("List available theme park destinations. Use this when the user asks for destinations, resorts, or parks at a high level.")]
    public static async Task<string> GetDestinations(IThemeParkClient client)
    {
        try
        {
            var destinations = await client.GetDestinations();

            if (!destinations.Any())
            {
                return "No destinations found!";
            }

            return string.Join("\n--\n", destinations.Select(destination => destination.Name));
        }
        catch (Exception ex)
        {
            return $"{ex.Message}";
        }
    }

    [McpServerTool, Description("List attractions for one specific destination. Requires a destination name and should not be used for requests that do not name a destination.")]
    public static async Task<string> GetAttractions(
        IThemeParkClient client,
        [Description("Destination name to filter attractions by, for example: Disneyland Resort or Walt Disney World Resort.")] string destination)
    {
        var destinations = await client.GetDestinations();

        if (!destinations.Any())
        {
            return "No destinations found!";
        }

        var parks = destinations.SelectMany(destination => destination.Parks);

        var filteredPark = parks
            .FirstOrDefault(park => park.Name.Contains(destination, StringComparison.OrdinalIgnoreCase));

        if (filteredPark == null)
        {
            return $"No destination with name {destination} was found!";
        }

        var filteredParkId = filteredPark.Id;

        var filteredParkEntity = await client.GetEntity(filteredParkId);
        var filteredParkEntityChildren = filteredParkEntity.Children;

        var attractions = filteredParkEntityChildren
            .Where(child => child.EntityType.Equals("ATTRACTION", StringComparison.OrdinalIgnoreCase));

        return string.Join("\n--\n", attractions.Select(attraction => attraction.Name));
    }
}
