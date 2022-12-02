using System.Diagnostics;

using AoC2022;

string day = DateTime.Now.ToString("dd");
bool isTest = false;

if (args.Length > 0)
{
    day = args[0];
}

if (args.Length > 1)
{
    isTest = args[1].StartsWith("t");
}

string input = File.ReadAllText($"./Inputs/Day{day}{(isTest ? "Test" : "")}.txt");

Type dayClassType = Type.GetType($"AoC2022.Days.Day{day}") ?? throw new FileNotFoundException();

BaseDay dayClass = Activator.CreateInstance(dayClassType) as BaseDay ?? throw new TypeLoadException();

Stopwatch sw = Stopwatch.StartNew();
string partOneResult = dayClass.PartOne(input);
sw.Stop();
Console.WriteLine("--------------------------------");
Console.WriteLine($"Result 1: {partOneResult}, Time taken: {sw.ElapsedMilliseconds}ms");
Console.WriteLine("--------------------------------");

sw = Stopwatch.StartNew();
string partTwoResult = dayClass.PartTwo(input);
sw.Stop();
Console.WriteLine($"Result 2: {partTwoResult}, Time taken: {sw.ElapsedMilliseconds}ms");
Console.WriteLine("--------------------------------");

Environment.Exit(-1);
