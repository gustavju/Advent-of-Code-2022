namespace AoC2022.Days;

public class Day1 : BaseDay
{
    public override string PartOne(string input) =>
        input.Split("\n\n")
            .Select(e => e.Lines().Select(int.Parse))
            .Select(e => e.Sum())
            .Max()
            .ToString();
    

    public override string PartTwo(string input) =>
        input.Split("\n\n")
            .Select(e => e.Lines().Select(int.Parse))
            .Select(e => e.Sum())
            .OrderByDescending(e => e)
            .Take(3)
            .Sum()
            .ToString();
}
