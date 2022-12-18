namespace AoC2022.Days;

public class Day17 : BaseDay
{
    private static HashSet<(int x, long y)> GetRock(long i, long y) => i switch
    {
        0 => new HashSet<(int, long)>() { (2, y), (3, y), (4, y), (5, y) },
        1 => new HashSet<(int, long)>() { (3, y + 2), (2, y + 1), (3, y + 1), (4, y + 1), (3, y) },
        2 => new HashSet<(int, long)>() { (4, y + 2), (4, y + 1), (4, y), (3, y), (2, y) },
        3 => new HashSet<(int, long)>() { (2, y), (2, y + 1), (2, y + 2), (2, y + 3) },
        4 => new HashSet<(int, long)>() { (2, y), (2, y + 1), (3, y), (3, y + 1) },
        _ => throw new ArgumentException("It aint right!")
    };

    private static HashSet<(int x, long y)> MoveLeft(HashSet<(int x, long y)> rock) => Move(rock, (-1, 0), 0);
    private static HashSet<(int x, long y)> MoveRight(HashSet<(int x, long y)> rock) => Move(rock, (1, 0), 6);
    private static HashSet<(int x, long y)> MoveDown(HashSet<(int x, long y)> rock) => Move(rock, (0, -1), null);
    private static HashSet<(int x, long y)> Move(HashSet<(int x, long y)> rock, (int x, long y) modifier, int? xRestriction)
    {
        if (xRestriction.HasValue && rock.Any(r => r.x == xRestriction.Value))
        {
            return rock;
        }
        HashSet<(int x, long y)> newRock = new();
        foreach ((int x, long y) in rock)
        {
            _ = newRock.Add((x + modifier.x, y + modifier.y));
        }
        return newRock;
    }

    private static long CalculateTowerHeight(string input, long goalRockCount = 2022)
    {
        HashSet<(int x, long y)> placed = new();
        for (int x = 0; x < 7; x++)
        {
            placed.Add((x, 0L));
        }

        int t = 0;
        long maxY = 0L;
        long rockCount = 0L;
        long added = 0L;
        Dictionary<(int t, long rocks), (long rockCount, long maxY)> seen = new();

        while (rockCount < goalRockCount)
        {
            HashSet<(int x, long y)> rock = GetRock(rockCount % 5, maxY + 4);
            while (true)
            {
                char dir = input[t % input.Length];

                HashSet<(int x, long y)> rockAfterMove = dir == '>' ? MoveRight(rock) : MoveLeft(rock);
                if (!placed.Intersect(rockAfterMove).Any())
                {
                    rock = rockAfterMove;
                }
                t = (t + 1) % input.Length;

                rockAfterMove = MoveDown(rock);
                if (placed.Intersect(rockAfterMove).Any())
                {
                    placed.UnionWith(rock);
                    maxY = placed.Max(p => p.y);

                    var cacheKey = (t, rockCount % 5);
                    if (seen.ContainsKey(cacheKey) && rockCount >= 2022)
                    {
                        (long seenRockCount, long seenMaxY) = seen[cacheKey];
                        long rockDiff = rockCount - seenRockCount;
                        long yDiff = maxY - seenMaxY;
                        long posibleFastForwards = (goalRockCount - rockCount) / rockDiff;
                        rockCount += posibleFastForwards * rockDiff;
                        added += posibleFastForwards * yDiff;
                    }
                    seen[cacheKey] = (rockCount, maxY);
                    break;
                }
                rock = rockAfterMove;
            }
            rockCount++;
        }
        return maxY + added;
    }

    public override string PartOne(string input) => CalculateTowerHeight(input).ToString();

    public override string PartTwo(string input) => CalculateTowerHeight(input, 1_000_000_000_000).ToString();
}
