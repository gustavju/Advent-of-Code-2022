namespace AoC2022.Days;

public class Day19 : BaseDay
{
    private record State
    {
        public State(int oBots, int cBots, int obBots, int gBots, int ore, int clay, int obsidian, int geode, int time)
        {
            OBots = oBots;
            CBots = cBots;
            ObBots = obBots;
            GBots = gBots;
            O = ore;
            C = clay;
            Ob = obsidian;
            G = geode;
            Time = time;
        }
        public int OBots { get; set; }
        public int CBots { get; set; }
        public int ObBots { get; set; }
        public int GBots { get; set; }
        public int O { get; set; }
        public int C { get; set; }
        public int Ob { get; set; }
        public int G { get; set; }
        public int Time { get; set; }
    }

    private static int BFS(int[] blueprint, int t)
    {
        int costOBot = blueprint[1];
        int costCBot = blueprint[2];
        (int costObBotO, int costObBotC) = (blueprint[3], blueprint[4]);
        (int costGBotO, int costGBotOb) = (blueprint[5], blueprint[6]);
        int maxOreNeeded = new[] { costOBot, costCBot, costObBotO, costGBotO }.Max();
        int max = 0;

        Queue<State> queue = new();
        queue.Enqueue(new State(1, 0, 0, 0, 0, 0, 0, 0, t));

        HashSet<State> seen = new();

        while (queue.Count > 0)
        {
            State s = queue.Dequeue();
            max = Math.Max(max, s.G);

            if (s.Time == 0)
            {
                continue;
            }

            s.O = Math.Min(s.O, maxOreNeeded * s.Time);
            s.C = Math.Min(s.C, costObBotC * s.Time);
            s.Ob = Math.Min(s.Ob, costGBotOb * s.Time);

            if (seen.Contains(s))
            {
                continue;
            }

            _ = seen.Add(s);

            int nextO = s.O + s.OBots;
            int nextC = s.C + s.CBots;
            int nextOb = s.Ob + s.ObBots;
            int nextG = s.G + s.GBots;

            if (s.O >= costGBotO && s.Ob >= costGBotOb)
            {
                queue.Enqueue(s with { O = nextO - costGBotO, C = nextC, Ob = nextOb - costGBotOb, G = nextG, GBots = s.GBots + 1, Time = s.Time - 1, });
                continue;
            }

            queue.Enqueue(s with { O = nextO, C = nextC, Ob = nextOb, G = nextG, Time = s.Time - 1 });

            if (s.O >= costOBot && s.OBots < maxOreNeeded)
            {
                queue.Enqueue(s with { O = nextO - costOBot, C = nextC, Ob = nextOb, G = nextG, OBots = s.OBots + 1, Time = s.Time - 1, });
            }

            if (s.O >= costCBot && s.CBots < costObBotC)
            {
                queue.Enqueue(s with { O = nextO - costCBot, C = nextC, Ob = nextOb, G = nextG, CBots = s.CBots + 1, Time = s.Time - 1, });
            }

            if (s.O >= costObBotO && s.C >= costObBotC && s.ObBots < costGBotOb)
            {
                queue.Enqueue(s with { O = nextO - costObBotO, C = nextC - costObBotC, Ob = nextOb, G = nextG, ObBots = s.ObBots + 1, Time = s.Time - 1, });
            }
        }
        return max;
    }

    public override string PartOne(string input) =>
        input.Lines().Select(l => l.Ints().ToArray())
            .Aggregate(0, (res, blueprint) => res += blueprint[0] * BFS(blueprint, 24)).ToString();

    public override string PartTwo(string input) =>
        input.Lines().Select(l => l.Ints().ToArray())
            .Take(3)
            .Aggregate(1, (res, blueprint) => res *= BFS(blueprint, 32)).ToString();
}
