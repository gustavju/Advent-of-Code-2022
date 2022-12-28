using System.Text.RegularExpressions;

namespace AoC2022.Days;

public partial class Day16 : BaseDay
{
    private record Valve(string Name, int Flow, List<string> Connections);

    private Dictionary<string, Valve> _valves = new();
    private List<string> _nonZeroValves = new();
    private int[,] _nonZeroValveDistance = new int[0, 0];
    private int[] _valveMasks = Array.Empty<int>();

    private Valve ParseValve(string s)
    {
        Match[] c = ValveNameRegex().Matches(s).ToArray();
        return new(c[0].Value, s.Ints().FirstOrDefault(), c[1..].Select(a => a.Value).ToList());
    }

    private int[,] GetDistanceMatrix(List<string> valveList)
    {
        var matrix = new int[_valves.Count, _valves.Count];
        for (int i = 0; i < valveList.Count; i++) //Fill in the default values
        {
            for (int j = i; j < valveList.Count; j++)
            {
                if (i == j)
                {
                    matrix[i, j] = 0;
                }
                else if (_valves[valveList[i]].Connections.Contains(valveList[j]))
                {
                    matrix[i, j] = 1;
                    matrix[j, i] = 1;
                }
                else
                {
                    matrix[i, j] = int.MaxValue / 2;
                    matrix[j, i] = int.MaxValue / 2;
                }

            }
        }
        for (int k = 0; k < valveList.Count; k++)
        {
            for (int i = 0; i < valveList.Count; i++)
            {
                for (int j = i + 1; j < valveList.Count; j++)
                {
                    if (matrix[i, k] + matrix[k, j] < matrix[i, j])
                    {
                        matrix[i, j] = matrix[i, k] + matrix[k, j];
                        matrix[j, i] = matrix[i, k] + matrix[k, j];
                    }
                }
            }
        }
        return matrix;
    }

    private void Traverse(int node, int time, int opened, int flow, Dictionary<int, int> memo)
    {
        memo[opened] = int.Max(memo.GetValueOrDefault(opened, 0), flow);
        for (int i = 0; i < _nonZeroValves.Count; i++)
        {
            var newTime = time - _nonZeroValveDistance[node, i] - 1;
            if ((_valveMasks[i] & opened) != 0 || newTime <= 0)
            {
                continue;
            }
            Traverse(i, newTime, opened | _valveMasks[i], flow + (newTime * _valves[_nonZeroValves[i]].Flow), memo);
        }
    }

    public override string PartOne(string input)
    {
        _valves = input.Lines().Select(ParseValve).ToDictionary(e => e.Name, e => e);
        List<string> valveList = _valves.Values.OrderBy(a => a.Name).Select(a => a.Name).ToList();
        int[,] distanceMatrix = GetDistanceMatrix(valveList);
        _nonZeroValves = valveList.Where(v => v == "AA" || _valves[v].Flow != 0).ToList();

        List<int> indices = valveList.Select((v, idx) => (idx, valve: _valves[v]))
            .Where(t => t.valve.Flow == 0 && t.valve.Name != "AA")
            .Select(t => t.idx).Reverse().ToList();

        _nonZeroValveDistance = distanceMatrix;
        foreach (var i in indices)
        {
            _nonZeroValveDistance = _nonZeroValveDistance.TrimArray(i, i);
        }

        _valveMasks = new int[_nonZeroValveDistance.GetLength(0)];
        for (int i = 0; i < _valveMasks.Length; i++)
        {
            _valveMasks[i] = 1 << i;
        }

        Dictionary<int, int> memo = new();
        Traverse(0, 30, 0, 0, memo);
        return memo.Values.Max().ToString();
    }

    public override string PartTwo(string input)
    {
        Dictionary<int, int> memo = new();
        Traverse(0, 26, 0, 0, memo);

        int maxFlow = 0;
        foreach ((KeyValuePair<int, int> kv, KeyValuePair<int, int> kv2) in memo.SelectMany(kv => memo.Select(kv2 => (kv, kv2))))
        {
            if ((kv.Key & kv2.Key) != 0)
                continue;
            maxFlow = int.Max(maxFlow, kv.Value + kv2.Value);
        }
        return maxFlow.ToString();

    }

    [GeneratedRegex("([A-Z]{2})")]
    private static partial Regex ValveNameRegex();
}
