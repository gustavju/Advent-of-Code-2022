namespace AoC2022.Days;

public class Day6 : BaseDay
{
    private static int GetIndexOfMarker(string s, int length)
    {
        int i = 0;
        while (s[i..(i + length)].Distinct().Count() != length)
        {
            i++;
        }
        return i + length;
    }

    public override string PartOne(string input)
        => GetIndexOfMarker(input, 4).ToString();

    public override string PartTwo(string input)
        => GetIndexOfMarker(input, 14).ToString();
}
