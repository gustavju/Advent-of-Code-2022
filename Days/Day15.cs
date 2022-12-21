namespace AoC2022.Days;

public class Day15 : BaseDay
{
    private record SensorBeacon((int x, int y) Sensor, (int x, int y) Beacon)
    {
        public Line GetCoverageOnY(int y)
        {
            int dy = Math.Abs(y - Sensor.y);
            if (dy > Radius)
            {
                return Line.Empty;
            }
            int dx = Radius - dy;
            return new Line(Sensor.x - dx, Sensor.x + dx);
        }
        public int Radius { get; } = Math.Abs(Beacon.x - Sensor.x) + Math.Abs(Beacon.y - Sensor.y);
    }

    private record struct Line(int From, int To)
    {
        public static Line Empty = new(0, -1);
        public bool IsEmpty => From > To;
        public IEnumerable<int> Values => IsEmpty ? Enumerable.Empty<int>() : Enumerable.Range(From, To - From + 1);
        public bool Overlaps(Line other)
          => !IsEmpty
          && !other.IsEmpty
          && From <= other.To
          && To >= other.From;

        public Line Intersects(Line other)
          => Overlaps(other) ? new(Math.Max(From, other.From), Math.Min(To, other.To)) : Empty;
    }


    private static int? MergeAndFind(IEnumerable<Line> lines, Line limit)
    {
        IOrderedEnumerable<Line> orderedLines = lines.Select(r => r.Intersects(limit))
                             .Where(r => !r.IsEmpty)
                             .OrderBy(r => r.From)
                             .ThenBy(r => r.To);

        int max = -1;
        foreach (Line line in orderedLines)
        {
            if (max + 1 < line.From)
            {
                return max + 1;
            }
            max = Math.Max(max, line.To);
        }
        return max < limit.To ? max + 1 : null;
    }

    public override string PartOne(string input)
    {
        IEnumerable<SensorBeacon> sensorBeaconPairs = input.Lines()
            .Select(l => l.Ints().ToArray())
            .Select(a => new SensorBeacon((a[0], a[1]), (a[2], a[3])));

        int line = 2_000_000;
        IEnumerable<int> beaconsOnLine = sensorBeaconPairs.Where(d => d.Beacon.y == line).Select(d => d.Beacon.x);

        return sensorBeaconPairs.SelectMany(s => s.GetCoverageOnY(line).Values)
                .Except(beaconsOnLine)
                .Count()
                .ToString();
    }

    public override string PartTwo(string input)
    {
        IEnumerable<SensorBeacon> sensorBeaconPairs = input.Lines()
            .Select(l => l.Ints().ToArray())
            .Select(a => new SensorBeacon((a[0], a[1]), (a[2], a[3])));

        int size = 4_000_000;
        var limit = new Line(0, size);

        for (int y = 0; y <= size; y++)
        {
            IEnumerable<Line> covered = sensorBeaconPairs.Select(s => s.GetCoverageOnY(y));
            int? nonCovered = MergeAndFind(covered, limit);
            if (nonCovered is int x)
            {
                return ((x * (long)size) + y).ToString();
            }
        }
        throw new Exception();
    }
}
