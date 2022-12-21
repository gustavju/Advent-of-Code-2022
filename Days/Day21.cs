namespace AoC2022.Days;

public class Day21 : BaseDay
{
    private Dictionary<string, string> _monkeys = new();

    private long ResolveMonkeyValue(string dictKey, long? humnValue = null)
    {
        if (humnValue.HasValue && dictKey == "humn")
        {
            return humnValue.Value;
        }

        string current = _monkeys[dictKey];
        if (long.TryParse(current, out long currentInt))
        {
            return currentInt;
        }

        string[] w = current.Words().ToArray();
        return w[1] switch
        {
            "*" => ResolveMonkeyValue(w[0], humnValue) * ResolveMonkeyValue(w[2], humnValue),
            "/" => ResolveMonkeyValue(w[0], humnValue) / ResolveMonkeyValue(w[2], humnValue),
            "+" => ResolveMonkeyValue(w[0], humnValue) + ResolveMonkeyValue(w[2], humnValue),
            "-" => ResolveMonkeyValue(w[0], humnValue) - ResolveMonkeyValue(w[2], humnValue),
            _ => throw new ArgumentException(string.Join(", ", w)),
        };
    }

    public override string PartOne(string input)
    {
        _monkeys = input.Lines().Select(l => l.Split(": ").ToArray()).ToDictionary(k => k[0], v => v[1]);
        return ResolveMonkeyValue("root").ToString();
    }

    public override string PartTwo(string input)
    {
        _monkeys = input.Lines().Select(l => l.Split(": ").ToArray()).ToDictionary(k => k[0], v => v[1]);
        string[] rootChildren = _monkeys["root"].Words().ToArray();
        (string constant, string nonConstant) rootSubNodes = (rootChildren[0], rootChildren[2]);

        if (ResolveMonkeyValue(rootChildren[0], 1) != ResolveMonkeyValue(rootChildren[0], 100))
        {
            rootSubNodes = (rootChildren[2], rootChildren[0]);
        }

        long constantValue = ResolveMonkeyValue(rootSubNodes.constant, 0);

        long lowBound = 0;
        long highBound = 1_000_000_000_000_000;
        long midPoint = (lowBound + highBound) / 2L;
        while (lowBound <= highBound)
        {
            midPoint = (lowBound + highBound) / 2L;
            long score = constantValue - ResolveMonkeyValue(rootSubNodes.nonConstant, midPoint);

            if (score == 0)
            {
                break;
            }

            if (score < 0)
            {
                lowBound = midPoint + 1;
            }
            else
            {
                highBound = midPoint - 1;
            }
        }
        return midPoint.ToString();
    }
}
