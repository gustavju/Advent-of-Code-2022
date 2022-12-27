namespace AoC2022.Days;

public class Day24 : BaseDay
{
    private int _heigth, _width;
    private readonly List<(int dr, int dc)> _moves = new() { (-1, 0), (1, 0), (0, -1), (0, 1), (0, 0) };
    private readonly List<char> _blizzardChars = new() { '<', '>', '^', 'v' };
    private readonly Dictionary<int, HashSet<(int r, int c)>> _blizzardsAtTime = new();
    private HashSet<(int r, int c)> CalcBlizzardsAtTime(HashSet<(int r, int c, char dir)> initBliz, int time)
    {
        HashSet<(int r, int c)> res = new();
        foreach ((int r, int c, char dir) in initBliz)
        {
            _ = res.Add(dir switch
            {
                '^' => (1 + (r - 1 - time).Mod(_heigth - 2), c),
                'v' => (1 + (r - 1 + time).Mod(_heigth - 2), c),
                '>' => (r, 1 + (c - 1 + time).Mod(_width - 2)),
                '<' => (r, 1 + (c - 1 - time).Mod(_width - 2)),
                _ => throw new NotImplementedException(),
            });
        }
        return res;
    }

    private HashSet<(int r, int c)> GetBlizzardsAtTime(HashSet<(int r, int c, char dir)> initBliz, int t)
    {
        if (_blizzardsAtTime.TryGetValue(t, out var value))
        {
            return value;
        }
        var blizzardByTime = CalcBlizzardsAtTime(initBliz, t);
        _blizzardsAtTime[t] = blizzardByTime;
        return blizzardByTime;
    }

    private int ElfsInBlizzard(string s, bool partTwo = false)
    {
        string[] map = s.Lines().ToArray();
        var initBlizzards = map.SelectMany((line, rowIdx) => line.Select((c, colIdx) => (rowIdx, colIdx, c)))
                                                                .Where(x => _blizzardChars.Contains(x.c))
                                                                .ToHashSet();
        _heigth = map.Length;
        _width = map[0].Length;
        (int r, int c) start = (0, map[0].IndexOf("."));
        (int r, int c) destination = (_heigth - 1, map[^1].IndexOf("."));

        HashSet<(int r, int c, int t, bool hasDest, bool hasStart)> visitedStates = new();
        Queue<(int r, int c, int t, bool hasDest, bool hasStart)> queue = new();
        queue.Enqueue((start.r, start.c, 0, false, false));
        while (queue.Count > 0)
        {
            (int r, int c, int t, bool hasDest, bool hasStart) current = queue.Dequeue();
            if (current.r >= _heigth || current.r < 0 ||
                current.c >= _width || current.c < 0 ||
                map[current.r][current.c] == '#' ||
                visitedStates.Contains(current))
            {
                continue;
            }
            _ = visitedStates.Add(current);

            if ((current.r, current.c) == destination && !current.hasDest)
            {
                current.hasDest = true;
            }

            if ((current.r, current.c) == start && current.hasDest && !current.hasStart)
            {
                current.hasStart = true;
            }

            if ((current.r, current.c) == destination && (!partTwo || (current.hasDest && current.hasStart)))
            {
                return current.t;
            }

            var blizNext = GetBlizzardsAtTime(initBlizzards, current.t + 1);
            foreach ((int dr, int dc) in _moves)
            {
                (int r, int c) next = (current.r + dr, current.c + dc);
                if (!blizNext.Contains(next))
                    queue.Enqueue((next.r, next.c, current.t + 1, current.hasDest, current.hasStart));
            }
        }
        throw new Exception();
    }

    public override string PartOne(string input)
        => ElfsInBlizzard(input).ToString();

    public override string PartTwo(string input)
        => ElfsInBlizzard(input, true).ToString();

}
