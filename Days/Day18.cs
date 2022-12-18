namespace AoC2022.Days;

public class Day18 : BaseDay
{
    private readonly List<(int x, int y, int z)> _adjecent = new()
        { (1,0,0), (-1,0,0), (0,1,0), (0,-1,0), (0,0,1), (0,0,-1) };

    private bool IsConnectedToOutside((int x, int y, int z) coord, HashSet<(int x, int y, int z)> coords)
    {
        Queue<(int x, int y, int z)> queue = new();
        HashSet<(int, int, int)> visited = new();
        queue.Enqueue(coord);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (coords.Contains(current) || visited.Contains(current))
            {
                continue;
            }
            visited.Add(current);

            if (visited.Count > 2000)
            {
                return true;
            }

            foreach ((int dx, int dy, int dz) in _adjecent)
            {
                queue.Enqueue((current.x + dx, current.y + dy, current.z + dz));
            }
        }
        return false;
    }

    private int CalculateSurfaceArea(string input, bool onlyCountExterior = false)
    {
        HashSet<(int x, int y, int z)> coords = input.Lines().Select(l => l.Split(",").Select(int.Parse))
                .Select(i => (i.ElementAt(0), i.ElementAt(1), i.ElementAt(2)))
                .ToHashSet();

        int ans = 0;
        foreach ((int x, int y, int z) in coords)
        {
            foreach ((int dx, int dy, int dz) in _adjecent)
            {
                if ((onlyCountExterior && IsConnectedToOutside((x + dx, y + dy, z + dz), coords)) ||
                    (!onlyCountExterior && !coords.Contains((x + dx, y + dy, z + dz))))
                {
                    ans++;
                }
            }
        }
        return ans;
    }

    public override string PartOne(string input)
        => CalculateSurfaceArea(input).ToString();

    public override string PartTwo(string input)
        => CalculateSurfaceArea(input, true).ToString();
}
