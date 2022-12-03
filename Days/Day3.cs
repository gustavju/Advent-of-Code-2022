namespace AoC2022.Days;

public class Day3 : BaseDay
{
    private static int GetPoint(char c) => char.IsAsciiLetterLower(c) ? c - 'a' + 1 : c - 'A' + 27;
    public override string PartOne(string input) =>
        input.Lines()
            .Select(l => GetPoint(l[..(l.Length / 2)].First(l[(l.Length / 2)..].Contains)))
            .Sum()
            .ToString();

    public override string PartTwo(string input)
    {
        int sum = 0;
        string[] strs = input.Lines().ToArray();
        for (int i = 0; i < strs.Length; i += 3)
        {
            sum += GetPoint(strs[i].First(c => strs[i + 1].Contains(c) && strs[i + 2].Contains(c)));
        }
        return sum.ToString();
    }
}
