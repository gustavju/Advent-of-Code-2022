namespace AoC2022.Days;

public class Day7 : BaseDay
{
    private static Dictionary<string, int> GetDirectory(string input)
    {
        Stack<string> currentPath = new();
        Dictionary<string, int> sizeByFolder = new();
        foreach (var line in input.Lines())
        {
            var lineParts = line.Words().ToArray();
            if (lineParts[1] == "cd")
            {
                if (lineParts[2] == "..")
                {
                    _ = currentPath.Pop();
                }
                else
                {
                    currentPath.Push(lineParts[2]);
                }
            }
            else if (lineParts[1] == "ls" || lineParts[0] == "dir")
            {
                continue;
            }
            else
            {
                int fileSize = int.Parse(lineParts[0]);
                for (int i = 1; i < currentPath.Count + 1; i++)
                {
                    string path = string.Join("/", currentPath.Reverse().ToArray()[..i]);
                    if (!sizeByFolder.TryAdd(path, fileSize))
                    {
                        sizeByFolder[path] += fileSize;
                    }
                }
            }
        }

        return sizeByFolder;
    }

    public override string PartOne(string input)
        => GetDirectory(input).Where(kv => kv.Value <= 100000).Sum(kv => kv.Value).ToString();


    public override string PartTwo(string input)
    {
        var sizeByFolder = GetDirectory(input);
        var neededSpace = sizeByFolder["/"] - (70000000 - 30000000);
        return sizeByFolder.Where(kv => kv.Value >= neededSpace).Min(kv => kv.Value).ToString();
    }
}
