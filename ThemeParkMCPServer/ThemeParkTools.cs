using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace ThemeParkServer.Tools;

[McpServerToolType]
public static class ThemeParkTools
{
    [McpServerTool, Description("Get a list of theme park destinations")]
    public static async Task<string> GetDestinations(HttpClient client)
    {
        try
        {
            using var jsonDocument = await client.ReadJsonDocumentAsync("destinations");
            var jsonElement = jsonDocument.RootElement;
            var destinations = jsonElement.GetProperty("destinations").EnumerateArray();

            if (!destinations.Any())
            {
                return "No destinations found!";
            }

            return string.Join("\n--\n", destinations.Select(destination => destination.GetProperty("name").GetString()));
        }
        catch (Exception e)
        {
            return $"{e.Message}";
        }
    }

    [McpServerTool, Description("Get a list of attractions in a theme park destination")]
    public static async Task<string> GetAttractions(
        HttpClient client,
        [Description("The name of the destination to fetch attractions for")] string destination)
    {
        // 1. Get a list of destinations
        using var destinationsJsonDocument = await client.ReadJsonDocumentAsync("destinations");
        var destinationJsonElement = destinationsJsonDocument.RootElement;
        var destinations = destinationJsonElement.GetProperty("destinations").EnumerateArray();
        var allDestinations = destinations.SelectMany(destination => destination.GetProperty("parks").EnumerateArray());

        // 2. Filter based on the destination name
        var foundDestination = allDestinations
            .FirstOrDefault(dest => dest.GetProperty("name").GetString()?.Contains(
                destination, StringComparison.OrdinalIgnoreCase
            ) == true
        );

        if (foundDestination.ValueKind == JsonValueKind.Undefined)
        {
            return $"No destination with name {destination} was found!";
        }

        // 3. Grab ID of destination name
        var destinationId = foundDestination.GetProperty("id").GetString();

        // 4. Use the children endpoint (https://api.themeparks.wiki/v1/entity/75ea578a-adc8-4116-a54d-dccb60765ef9/children) and grab attractions
        using var destinationChildrenJsonDocument = await client.ReadJsonDocumentAsync($"entity/{destinationId}/children");
        var destinationChildrenJsonElement = destinationChildrenJsonDocument.RootElement;
        var destinationChildren = destinationChildrenJsonElement.GetProperty("children").EnumerateArray();

        // 5. Profit!
        var destinationChildrenAttractions = destinationChildren
            .Where(dest => dest.GetProperty("entityType").GetString()?.Equals("ATTRACTION", StringComparison.OrdinalIgnoreCase) == true).ToList();

        return string.Join("\n--n", destinationChildrenAttractions.Select(dest => dest.GetProperty("name").GetString()));
    }
}