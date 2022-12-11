namespace AoC2022.Days;

public class Day11 : BaseDay
{
    private class Monkey
    {
        public int Id { get; set; }
        private Queue<long> Items { get; set; }
        private Func<long, long> Operation { get; set; }
        public int TestDivisibleBy { get; set; }
        private int IdWhenTrue { get; set; }
        private int IdWhenFalse { get; set; }
        public Monkey(string s)
        {
            IEnumerable<string> lines = s.Lines().Select(l => l.Trim());
            Id = lines.ElementAt(0).Words().ElementAt(1)[0] - '0';
            Items = new Queue<long>(lines.ElementAt(1).Split(":")[1].Split(",").Select(i => long.Parse(i.Trim())));
            Operation = ParseOperaion(lines.ElementAt(2));
            TestDivisibleBy = int.Parse(lines.ElementAt(3).Words().Last());
            IdWhenTrue = int.Parse(lines.ElementAt(4).Words().Last());
            IdWhenFalse = int.Parse(lines.ElementAt(5).Words().Last());
        }
        private static Func<long, long> ParseOperaion(string v)
        {
            IEnumerable<string> words = v.Words();
            return words.ElementAt(4) == "+"
                ? words.Last() == "old" ? (x => x + x) : (x => x + long.Parse(words.Last()))
                : words.Last() == "old" ? (x => x * x) : (x => x * long.Parse(words.Last()));
        }
        public long DoOperation(long i) => Operation(i);
        public int NextMonkeyId(long i) => i % TestDivisibleBy == 0 ? IdWhenTrue : IdWhenFalse;
        public long GetItem() => Items.Dequeue();
        public void GiveItem(long item) => Items.Enqueue(item);
        public bool HasItems() => Items.Count > 0;
    }

    private static long MonkeyBusiness(string input, bool part2 = false)
    {
        Monkey[] monkeys = input.Split("\n\n").Select(m => new Monkey(m)).ToArray();
        long[] inspections = new long[monkeys.Length];
        int lcm = monkeys.Select(m => m.TestDivisibleBy).LowestCommonMultiple();
        int rounds = part2 ? 10000 : 20;

        for (int i = 0; i < rounds; i++)
        {
            foreach (Monkey monkey in monkeys)
            {
                while (monkey.HasItems())
                {
                    inspections[monkey.Id]++;

                    long item = monkey.GetItem();
                    long itemAfterOp = monkey.DoOperation(item);

                    long newItemLvl = part2 ? itemAfterOp % lcm : (int)Math.Floor(itemAfterOp / 3.0);

                    int nextMonkeyId = monkey.NextMonkeyId(newItemLvl);
                    monkeys[nextMonkeyId].GiveItem(newItemLvl);
                }
            }
        }
        return inspections.OrderByDescending(v => v).Take(2).Aggregate(1L, (res, val) => res *= val);
    }

    public override string PartOne(string input)
        => MonkeyBusiness(input).ToString();

    public override string PartTwo(string input)
        => MonkeyBusiness(input, true).ToString();
}
