namespace AoC2022.Days;

public class Day22 : BaseDay
{
    private enum Direction { Right = 0, Down = 1, Left = 2, Up = 3 };
    private readonly (int c, int r)[] _dirMoves = new (int, int)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
    private static List<string> ParseInstructions(string s)
    {
        List<string> instructions = new();
        int i = 0;
        while (i < s.Length)
        {
            string n = "";
            while (i < s.Length && char.IsDigit(s[i]))
            {
                n += s[i];
                i++;
            }
            if (!string.IsNullOrEmpty(n))
            {
                instructions.Add(n);
                continue;
            }
            instructions.Add(s[i].ToString());
            i++;
        }
        return instructions;
    }
    public override string PartOne(string input)
    {
        var mapLines = input.Split("\n\n")[0].Lines();
        var map = mapLines.Select(s => s.PadRight(mapLines.Max(m => m.Length), ' ').ToArray()).ToArray();
        var instructions = ParseInstructions(input.Split("\n\n")[1]);

        (int r, int c, Direction dir) = (0, 0, Direction.Right);
        for (c = 0; c < map[0].Length; c++)
        {
            if (map[0][c] == '.')
            {
                break;
            }
        }

        // for (int i = 0; i < map.Length; i++)
        // {
        //     char[]? row = map[i];
        //     for (int i1 = 0; i1 < row.Length; i1++)
        //     {
        //         char col = row[i1];
        //         if ((i, i1) == (c, r))
        //         {
        //             Console.Write('!');
        //         }
        //         else
        //             Console.Write(col);
        //     }
        //     Console.WriteLine(row.Length);
        // }

        foreach (var instruction in instructions)
        {
            Console.WriteLine("Instruct: " + instruction);
            Console.WriteLine($"{r}:{c}, {dir}");
            Console.WriteLine();
            if (instruction == "L")
            {
                dir = (Direction)(((int)dir + 3) % 4);
            }
            else if (instruction == "R")
            {
                dir = (Direction)(((int)dir + 1) % 4);
            }
            else
            {
                int steps = int.Parse(instruction);
                for (int i = 0; i < steps; i++)
                {
                    Console.WriteLine(i);
                    var rr = (r + _dirMoves[(int)dir].r) % map.Length;
                    if (rr == -1)
                    {
                        rr += map.Length;
                    }
                    var cc = (c + _dirMoves[(int)dir].c) % map[0].Length;
                    if (cc == -1)
                    {
                        cc += map[0].Length;
                    }
                    Console.WriteLine(rr + ", " + cc);
                    while (char.IsWhiteSpace(map[rr][cc]))
                    {
                        //Console.WriteLine(rr + ", " + cc);
                        rr = (rr + _dirMoves[(int)dir].r) % map.Length;
                        if (rr == -1)
                        {
                            rr += map.Length;
                        }
                        cc = (cc + _dirMoves[(int)dir].c) % map[0].Length;
                        if (cc == -1)
                        {
                            cc += map[0].Length;
                        }
                    }
                    if (map[rr][cc] == '#')
                    {
                        break;
                    }
                    r = rr;
                    c = cc;
                }
            }
            Console.WriteLine($"{r}:{c}, {(int)dir}");
        }
        var res = ((r + 1) * 1000) + ((c + 1) * 4) + (int)dir;
        return res.ToString();
    }

    // Facing is 0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^)

    public override string PartTwo(string input)
    {
        throw new System.NotImplementedException();
    }
}
