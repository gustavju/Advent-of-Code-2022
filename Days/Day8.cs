namespace AoC2022.Days;

public class Day8 : BaseDay
{
    private static int TreesInView(int[] view, int current)
    {
        int i;
        for (i = 0; i < view.Length; i++)
            if (view[i] >= current)
                return i + 1;
        return i;
    }

    private static (int visibleCount, int scenicScore) QuadcopterSearch(string input)
    {
        int[][] rows = input.Lines().Select(l => l.Select(c => c - '0').ToArray()).ToArray();
        // feels like cheating creating the columns, but w/e (°u°)  
        int[][] cols = input.Lines().SelectMany(line => line.Select((item, index) => new { item, index }))
                                .GroupBy(i => i.index, i => i.item)
                                .Select(g => g.Select(c => c - '0').ToArray()).ToArray();

        int visibleCount = 0;
        int scenicScoreMax = 0;
        for (int c = 0; c < rows.ElementAt(0).Length; c++)
        {
            for (int r = 0; r < rows.Length; r++)
            {
                if (c == 0 || r == 0 || c == rows.ElementAt(0).Length - 1 || r == rows.Length - 1)
                {
                    visibleCount++;
                    continue;
                }

                int current = rows[r][c];

                int[] left = rows[r][..c];
                int[] right = rows[r][(c + 1)..];
                int[] up = cols[c][..r];
                int[] down = cols[c][(r + 1)..];

                // Part 1
                if (current > left.Max() || current > right.Max() || current > up.Max() || current > down.Max())
                {
                    visibleCount++;
                }

                // Part 2
                int scenicScore = TreesInView(left.Reverse().ToArray(), current) *
                    TreesInView(right, current) *
                    TreesInView(up.Reverse().ToArray(), current) *
                    TreesInView(down, current);

                if (scenicScore > scenicScoreMax)
                {
                    scenicScoreMax = scenicScore;
                }
            }
        }
        return (visibleCount, scenicScoreMax);
    }

    public override string PartOne(string input)
        => QuadcopterSearch(input).visibleCount.ToString();

    public override string PartTwo(string input)
        => QuadcopterSearch(input).scenicScore.ToString();
}
