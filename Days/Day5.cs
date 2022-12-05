namespace AoC2022.Days;

public class Day5 : BaseDay
{
    private static string GoCargoCraneGo(string input, bool craneCanMultiMove)
    {
        var parts = input.Split("\n\n");
        Dictionary<int, string> stacks = parts[0].Lines().SelectMany(inner => inner.Select((item, index) => new { item, index }))
                                .GroupBy(i => i.index, i => i.item)
                                .Select(g => new string(g.ToArray()))
                                .Where(s => char.IsDigit(s[^1]))
                                .ToDictionary(s => int.Parse(s.Substring(s.Length - 1, 1)),
                                              s => new string(s[..^1].Trim().Reverse().ToArray()));

        IEnumerable<int[]> moves = parts[1].Lines().Select(l => l.Words().Where(w => char.IsDigit(w[0])).Select(int.Parse).ToArray());

        foreach (var move in moves)
        {
            int quantity = move[0];
            int from = move[1];
            int to = move[2];

            var tmp = stacks[from][^quantity..];

            if (!craneCanMultiMove)
            {
                tmp = new string(tmp.Reverse().ToArray());
            }

            stacks[from] = stacks[from][..^quantity];
            stacks[to] += tmp;
        }
        return new string(stacks.Select(v => v.Value[^1]).ToArray());
    }

    public override string PartOne(string input)
        => GoCargoCraneGo(input, false);

    public override string PartTwo(string input)
        => GoCargoCraneGo(input, true);
}
