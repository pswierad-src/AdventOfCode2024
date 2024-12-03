using System.Text.RegularExpressions;

namespace Day3;

internal static class Processor
{
    public static async Task RunTaskOne(string path)
    {
        var input = await File.ReadAllTextAsync(path);
        var r = new Regex(@"mul\(\d{1,3},\d{1,3}\)", RegexOptions.IgnoreCase);

        var multSum = r.Matches(input)
            .Select(m => m.Value[4..].TrimEnd(')').Split(','))
            .Select(m => (int.Parse(m[0]), int.Parse(m[^1])))
            .Sum(p => p.Item1 * p.Item2);

        Console.WriteLine($"Running task 1 for: {path} Sum of multiplications: {multSum}");
    }

    public static async Task RunTaskTwo(string path)
    {
        var input = await File.ReadAllTextAsync(path);
        var r = new Regex(@"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)", RegexOptions.IgnoreCase);

        var matches = r.Matches(input).Select(m => m.Value).ToList();
        var enabled = true;

        var multSum = 0;

        foreach (var match in matches)
        {
            switch (match)
            {
                case "do()":
                    enabled = true;
                    break;
                case "don't()":
                    enabled = false;
                    break;
                default:
                    if (enabled)
                    {
                        var x = match[4..].TrimEnd(')').Split(',').Select(int.Parse).ToList();
                        multSum += x[0] * x[^1];
                    }
                    break;
            }
        }

        Console.WriteLine($"Running task 2 for: {path} Sum of multiplications: {multSum}");
    }

}
