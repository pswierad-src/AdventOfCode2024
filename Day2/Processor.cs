namespace Day2;

internal class Processor
{
    public static async Task RunTaskOne(string path)
    {
        var reports = new List<bool>();

        foreach (var line in await File.ReadAllLinesAsync(path))
        {
            var levels = line.Split(" ").Select(int.Parse).ToList();

            var result = CalculateSafety(levels);

            reports.Add(result);
        }

        Console.WriteLine($"Running task 1 for: {path} Safe reports: {reports.Count(r => r)}");
    }

    public static async Task RunTaskTwo(string path)
    {
        var reports = new List<bool>();

        foreach (var line in await File.ReadAllLinesAsync(path))
        {
            var levels = line.Split(" ").Select(int.Parse).ToList();

            var result = CalculateSafety(levels);

            if (!result)
            {
                for (var i = 0; i < levels.Count; i++)
                {
                    var x = new List<int>(levels);
                    x.RemoveAt(i);

                    result = CalculateSafety(x);

                    if (!result)
                    {
                        continue;
                    }

                    break;
                }
            }

            reports.Add(result);
        }

        Console.WriteLine($"Running task 2 for: {path} Safe reports: {reports.Count(r => r == true)}");
    }

    private static bool CalculateSafety(List<int> levels)
    {
        var isAscending = levels[0] < levels[1];

        for (var i = 1; i < levels.Count; i++)
        {
            if (levels[i] > levels[i - 1] != isAscending)
                return false;

            var diff = Math.Abs(levels[i] - levels[i - 1]);

            if (diff is > 3 or < 1)
                return false;
        }

        return true;
    }
}
