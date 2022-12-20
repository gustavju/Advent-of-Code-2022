namespace AoC2022.Days;

public class Day20 : BaseDay
{
    private static int GetNewIndex(int currentIndex, long value, int length)
    {
        int newIndex = (int)((currentIndex + value) % length);
        return newIndex > 0 ? newIndex : length + newIndex;
    }

    private static long Decrypt(string input, long decryptionKey, int mixes)
    {
        IEnumerable<(int index, long value)> original = input.Lines().Select(long.Parse).Select((value, index) => (index, value * decryptionKey));
        List<(int index, long value)> modified = original.ToList();

        for (int i = 0; i < mixes; i++)
        {
            foreach ((int index, long value) current in original)
            {
                int mIndex = modified.IndexOf(modified.First(w => w.index == current.index));
                modified.RemoveAt(mIndex);
                modified.Insert(GetNewIndex(mIndex, current.value, modified.Count), current);
            }
        }
        int zeroIdx = modified.IndexOf(modified.First(w => w.value == 0));
        return new[] { 1000, 2000, 3000 }.Select(nth => modified[(zeroIdx + nth) % modified.Count].value).Sum();
    }
    public override string PartOne(string input) =>
        Decrypt(input, 1, 1).ToString();

    public override string PartTwo(string input) =>
        Decrypt(input, 811589153, 10).ToString();
}
