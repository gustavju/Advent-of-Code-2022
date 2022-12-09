namespace AoC2022.Days;

public class Day9 : BaseDay
{
    private readonly Dictionary<string, (int x, int y)> _dirs = new()
    {
        {"L", (-1, 0)}, {"R", (1, 0)}, {"U", (0, 1)}, {"D", (0, -1)}
    };

private static (int x, int y) MoveCloser((int x, int y) head, (int x, int y) tail)
{
    (int dx, int dy) = (head.x - tail.x, head.y - tail.y);
    int absDx = Math.Abs(dx);
    int absDy = Math.Abs(dy);

    if (absDx <= 1 && absDy <= 1) // Fine!
        return tail;

    if (absDx >= 2 && absDy >= 2) // Diagonal
    {
        int newX = (head.x > tail.x) ? tail.x + 1 : tail.x - 1;
        int newY = (head.y > tail.y) ? tail.y + 1 : tail.y - 1;
        return (newX, newY);
    }

    if (absDx >= 2) // Left, Right
        return ((head.x > tail.x) ? tail.x + 1 : tail.x - 1, head.y);

    // Up, Down
    return (head.x, (head.y > tail.y) ? tail.y + 1 : tail.y - 1);
}

    public override string PartOne(string input)
    {
        HashSet<(int, int)> tailVisited = new();
        (int x, int y) tail = (0, 0);
        (int x, int y) head = (0, 0);

        tailVisited.Add(tail);

        foreach (var line in input.Lines())
        {
            (int dx, int dy) = _dirs[line.Words().ElementAt(0)];
            int steps = int.Parse(line.Words().ElementAt(1));
            for (int i = 0; i < steps; i++)
            {
                head = (head.x + dx, head.y + dy);
                tail = MoveCloser(head, tail);
                tailVisited.Add(tail);
            }
        }

        return tailVisited.Count.ToString();
    }

    public override string PartTwo(string input)
    {
        List<HashSet<(int, int)>> tailsVisited = new();
        var tails = new (int, int)[9];
        (int x, int y) head = (0, 0);

        for (int i = 0; i < 9; i++)
            tailsVisited.Add(new HashSet<(int, int)>() { (0, 0) });


        foreach (var line in input.Lines())
        {
            (int dx, int dy) = _dirs[line.Words().ElementAt(0)];
            int steps = int.Parse(line.Words().ElementAt(1));
            for (int i = 0; i < steps; i++)
            {
                head = (head.x + dx, head.y + dy);
                tails[0] = MoveCloser(head, tails[0]);
                for (int ti = 1; ti < 9; ti++)
                {
                    tails[ti] = MoveCloser(tails[ti - 1], tails[ti]);
                    tailsVisited[ti].Add(tails[ti]);
                }
            }
        }

        return tailsVisited[8].Count.ToString();
    }
}
