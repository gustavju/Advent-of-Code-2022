namespace AoC2022.Days;

public class Day14 : BaseDay
{

    private static int SimulateSand(string input, bool part2 = false)
    {
        IEnumerable<IEnumerable<(int, int)>> rockSections = input.Lines()
                    .Select(l => l.Split(" -> ")
                        .Select(w => (int.Parse(w.Split(",")[0]), int.Parse(w.Split(",")[1]))));

        HashSet<(int, int)> set = new();
        foreach (var section in rockSections)
        {
            (int x, int y) origin = section.First();
            foreach ((int x, int y) destination in section.Skip(1))
            {
                (int dx, int dy) = (destination.x - origin.x, destination.y - origin.y);
                int lineLength = Math.Max(Math.Abs(dx), Math.Abs(dy));
                for (int i = 0; i <= lineLength; i++)
                {
                    _ = set.Add((
                        dx != 0 ? (dx < 0 ? (origin.x - i) : (origin.x + i)) : origin.x,
                        dy != 0 ? (dy < 0 ? (origin.y - i) : (origin.y + i)) : origin.y));
                }
                origin = destination;
            }
        }

        int rHi = set.Max(s => s.Item2);
        for (int c = -1000; c < 1000; c++)
        {
            _ = set.Add((c, rHi + 2));
        }


        int sands = 0;
        while (true)
        {
            (int c, int r) newSand = (500, 0);
            while (true)
            {
                if (!part2 && newSand.r + 1 > rHi)
                {
                    return sands;
                }

                if (!set.Contains((newSand.c, newSand.r + 1)))
                {
                    newSand = (newSand.c, newSand.r + 1);
                }
                else if (!set.Contains((newSand.c - 1, newSand.r + 1)))
                {
                    newSand = (newSand.c - 1, newSand.r + 1);
                }
                else if (!set.Contains((newSand.c + 1, newSand.r + 1)))
                {
                    newSand = (newSand.c + 1, newSand.r + 1);
                }
                else if (newSand == (500, 0))
                {
                    return sands + 1;
                }
                else
                {
                    _ = set.Add(newSand);
                    sands++;
                    break;
                }
            }
        }
    }

    public override string PartOne(string input) => SimulateSand(input).ToString();

    public override string PartTwo(string input) => SimulateSand(input, true).ToString();
}
