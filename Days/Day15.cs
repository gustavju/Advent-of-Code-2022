namespace AoC2022.Days;

public class Day15 : BaseDay
{
    private (int sc, int sr, int bc, int br) ParseLine(string s)
    {
        string[] words = s.Words().ToArray();
        return (
            int.Parse(words[2].Split("=")[1][..^1]),
            int.Parse(words[3].Split("=")[1][..^1]),
            int.Parse(words[8].Split("=")[1][..^1]),
            int.Parse(words[9].Split("=")[1])
        );
    }

    private static int CalcManhattanDistance(int c1, int r1, int c2, int r2) => Math.Abs(c1 - c2) + Math.Abs(r1 - r2);

    private bool CoordinateOK(int c, int r, HashSet<(int c, int r, int distance)> sensors)
    {
        foreach ((int sc, int sr, int distance) in sensors)
        {
            var dcr = Math.Abs(c - sc) + Math.Abs(r - sr);
            if (dcr <= distance)
            {
                return false;
            }
        }
        return true;
    }

    public override string PartOne(string input)
    {
        var sensorsAndBeacons = input.Lines().Select(l => l).Select(ParseLine);
        HashSet<(int c, int r)> beacos = new();
        HashSet<(int c, int r, int distance)> sensors = new();
        foreach ((int sc, int sr, int bc, int br) in sensorsAndBeacons)
        {
            sensors.Add((sc, sr, CalcManhattanDistance(sc, sr, bc, br)));
            beacos.Add((bc, br));
        }

        int res = 0;
        for (int c = -10000000; c < 10000000; c++)
        {
            int r = 2000000;
            if (!CoordinateOK(c, r, sensors) && !beacos.Contains((c, r)))
                res++;
        }
        return res.ToString();
    }

    public override string PartTwo(string input)
    {
        throw new System.NotImplementedException();
    }
}
