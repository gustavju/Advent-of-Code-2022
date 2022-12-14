using System.Text.Json.Nodes;

namespace AoC2022.Days;

public class Day13 : BaseDay
{
    private class PacketComparer : IComparer<JsonNode>
    {
        public int Compare(JsonNode? x, JsonNode? y)
        {
            if (x is JsonValue && y is JsonValue)
            {
                return x.GetValue<int>().CompareTo(y.GetValue<int>());
            }
            JsonArray xArr = x switch { JsonArray arr => arr, _ => new JsonArray(x?.GetValue<int>()) };
            JsonArray yArr = y switch { JsonArray arr => arr, _ => new JsonArray(y?.GetValue<int>()) };
            foreach ((JsonNode? xItem, JsonNode? yItem) in Enumerable.Zip(xArr, yArr))
            {
                int c = Compare(xItem, yItem);
                if (c != 0)
                {
                    return c;
                }
            }
            return xArr.Count - yArr.Count;
        }
    }

    public override string PartOne(string input) =>
        input.Lines()
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => JsonNode.Parse(l))
            .Chunk(2)
            .Select((pair, idx) => new PacketComparer().Compare(pair[0], pair[1]) < 0 ? idx + 1 : 0)
            .Sum().ToString();

    public override string PartTwo(string input)
    {
        JsonNode? packet1 = JsonNode.Parse("[2]");
        JsonNode? packet2 = JsonNode.Parse("[6]");

        List<JsonNode?> orderdPackets = input.Lines()
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => JsonNode.Parse(l))
            .Append(packet1).Append(packet2)
            .OrderBy(x => x, new PacketComparer())
            .ToList();

        return ((orderdPackets.IndexOf(packet1) + 1) * (orderdPackets.IndexOf(packet2) + 1)).ToString();
    }
}
