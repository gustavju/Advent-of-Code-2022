namespace AoC2022.Days;

public class Day10 : BaseDay
{
    private readonly int[] _timeIntervals = new int[] { 20, 60, 100, 140, 180, 220 };

    private (int time, int result) Tick(int time, int x, int result, List<char[]>? display = null)
    {
        if (display != null)
        {
            display[time / 40][time % 40] = Math.Abs(x - (time % 40)) <= 1 ? '#' : ' ';
        }
        time++;
        if (_timeIntervals.Any(t => t == time))
        {
            result += time * x;
        }
        return (time, result);
    }

    public override string PartOne(string input)
    {
        int time = 0;
        int x = 1;
        int ans = 0;
        foreach (var line in input.Lines())
        {
            IEnumerable<string> words = line.Words();

            if (words.ElementAt(0) == "noop")
            {
                (time, ans) = Tick(time, x, ans);
            }
            else if (words.ElementAt(0) == "addx")
            {
                (time, ans) = Tick(time, x, ans);
                (time, ans) = Tick(time, x, ans);
                x += int.Parse(words.ElementAt(1));
            }
        }
        return ans.ToString();
    }

    public override string PartTwo(string input)
    {
        int time = 0;
        int x = 1;
        int ans = 0;
        List<char[]> display = new();

        for (int i = 0; i < 6; i++)
        {
            display.Add(new char[40]);
        }

        foreach (var line in input.Lines())
        {
            IEnumerable<string> words = line.Words();

            if (words.ElementAt(0) == "noop")
            {
                (time, ans) = Tick(time, x, ans, display);
            }
            else if (words.ElementAt(0) == "addx")
            {
                (time, ans) = Tick(time, x, ans, display);
                (time, ans) = Tick(time, x, ans, display);
                x += int.Parse(words.ElementAt(1));
            }
        }
        return "\n" + string.Join("\n", display.Select(row => string.Join("", row)));
    }
}
