namespace AoC2022.Days;

public class Day12 : BaseDay
{
    private readonly List<(int dr, int dc)> _dirs = new() { (-1, 0), (0, 1), (1, 0), (0, -1) };
    private int BFS(string input, bool part2 = false)
    {
        char[][] charGrid = input.Lines().Select(l => l.ToArray()).ToArray();
        int[][] grid = charGrid.Select(r => r.Select(c => c switch
        {
            'S' => 1,
            'E' => 26,
            _ => c - 'a' + 1
        }).ToArray()).ToArray();

        Queue<((int, int), int)> queue = new();
        for (int r = 0; r < charGrid.Length; r++)
            for (int c = 0; c < charGrid[r].Length; c++)
                if (charGrid[r][c] == 'S' || (part2 && charGrid[r][c] == 'a'))
                    queue.Enqueue(((r, c), 0));

        HashSet<(int, int)> visited = new();
        while (queue.Count > 0)
        {
            ((int r, int c), int steps) = queue.Dequeue();

            if (visited.Contains((r, c)))
            {
                continue;
            }

            _ = visited.Add((r, c));

            if (charGrid[r][c] == 'E')
            {
                return steps;
            }

            foreach ((int dr, int dc) in _dirs)
            {
                int rr = r + dr;
                int cc = c + dc;
                if (rr >= 0 && rr < grid.Length &&
                    cc >= 0 && cc < grid[0].Length &&
                    grid[rr][cc] <= grid[r][c] + 1)
                {
                    queue.Enqueue(((rr, cc), steps + 1));
                }
            }
        }
        throw new Exception("Sad times for hiking Elves :(");
    }

    public override string PartOne(string input) => BFS(input).ToString();

    public override string PartTwo(string input) => BFS(input, true).ToString();
}
