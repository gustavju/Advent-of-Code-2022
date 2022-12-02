namespace AoC2022.Days;

public class Day2 : BaseDay
{
    private readonly Dictionary<int, int> _wins = new() { { 0, 1 }, { 1, 2 }, { 2, 0 } };

    // switch expressions is somekind of vibe
    private int GetRoundScore(int opponent, int selectedMove) => opponent switch
    {
        int _ when opponent == selectedMove => selectedMove + 1 + 3,
        int _ when _wins[opponent] == selectedMove => selectedMove + 1 + 6,
        _ => selectedMove + 1
    };

    private int GetMove(int outcome, int opponentMove) => outcome switch
    {
        0 => _wins.FirstOrDefault(kv => kv.Value == opponentMove).Key,
        1 => opponentMove,
        _ => _wins[opponentMove],
    };

    // LINQ to the moon?
    public override string PartOne(string input) =>
        input.Lines().Select(l => l.Words())
            .Select(w =>
                (opponent: w.First()[0] - 'A',
                move: w.Last()[0] - 'X'))
            .Aggregate(0, (result, element) => result += GetRoundScore(element.opponent, element.move))
            .ToString();

    public override string PartTwo(string input) =>
        input.Lines().Select(l => l.Words())
            .Select(w =>
                (opponent: w.First()[0] - 'A',
                outcome: w.Last()[0] - 'X'))
            .Aggregate(0, (result, element) => result += GetRoundScore(element.opponent, GetMove(element.outcome, element.opponent)))
            .ToString();
}
