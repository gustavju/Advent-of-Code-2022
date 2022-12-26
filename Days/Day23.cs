namespace AoC2022.Days;

public class Day23 : BaseDay
{
    private readonly List<(int r, int c)> _directions = new() { (-1, 0), (1, 0), (0, -1), (0, 1) }; // N S W E
    private int ElfScan(string s, bool partTwo = false)
    {
        string[] initState = s.Lines().ToArray();
        HashSet<(int r, int c)> elfs = new();
        for (int r = 0; r < initState.Length; r++)
        {
            for (int c = 0; c < initState[0].Length; c++)
            {
                if (initState[r][c] == '#')
                {
                    _ = elfs.Add((r, c));
                }
            }
        }

        int rounds = 0;
        bool elfMoved = false;
        while (elfMoved || (partTwo && rounds < 10))
        {
            Dictionary<(int r, int c), (int r, int c)> elfUpdates = new();
            foreach ((int r, int c) elf in elfs)
            {
                elfMoved = false;
                bool tryingToMove = false;
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if ((!(dr == 0 && dc == 0)) && elfs.Contains((elf.r + dr, elf.c + dc)))
                            tryingToMove = true;
                    }
                }

                if (tryingToMove)
                {
                    for (int direction = 0; direction < 4; direction++)
                    {
                        int dirOffset = rounds % 4;
                        (int r, int c) = _directions[(dirOffset + direction) % 4];
                        (int r, int c) newElf = (elf.r + r, elf.c + c);
                        if (((dirOffset + direction) % 4 < 2 && !elfs.Contains(newElf) && !elfs.Contains((newElf.r, newElf.c + 1)) && !elfs.Contains((newElf.r, newElf.c - 1))) ||
                            ((dirOffset + direction) % 4 >= 2 && !elfs.Contains(newElf) && !elfs.Contains((newElf.r + 1, newElf.c)) && !elfs.Contains((newElf.r - 1, newElf.c))))
                        {
                            elfUpdates.Add(elf, newElf);
                            elfMoved = true;
                            break;
                        }
                    }
                }

                if (!elfMoved)
                    elfUpdates.Add(elf, elf);

            }
            var collisions = elfUpdates.GroupBy(e => e.Value).Where(grp => grp.Count() > 1);
            foreach (var collition in collisions)
            {
                foreach (var invalidMove in collition)
                {
                    elfUpdates.Remove(invalidMove.Key);
                    elfUpdates.Add(invalidMove.Key, invalidMove.Key);
                }
            }
            elfs = elfUpdates.Values.ToHashSet();
            rounds++;
        }

        int x = Math.Abs(elfs.Min(e => e.c) - elfs.Max(e => e.c));
        int y = Math.Abs(elfs.Min(e => e.r) - elfs.Max(e => e.r));

        return partTwo ? rounds : (((x + 1) * (y + 1)) - elfs.Count);
    }

    public override string PartOne(string input)
        => ElfScan(input).ToString();

    public override string PartTwo(string input)
        => ElfScan(input, true).ToString();
}
