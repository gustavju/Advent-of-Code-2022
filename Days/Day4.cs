namespace AoC2022.Days;

// The LINQ Strikes Back
public class Day4 : BaseDay
{
    // [0] = x1, [1] = x2, [2] = y1, [3] = y1, 
    private int[] Parse(string s) =>
        s.Split(',').SelectMany(s => s.Split('-').Select(int.Parse)).ToArray();

    public override string PartOne(string input) =>
        input.Lines().Select(Parse)
             .Count(p => (p[0] <= p[2] && p[3] <= p[1]) || (p[2] <= p[0] && p[1] <= p[3]))
             .ToString();

    public override string PartTwo(string input) =>
        input.Lines().Select(Parse)
             .Count(p => p[0] <= p[3] && p[2] <= p[1])
             .ToString();
}
