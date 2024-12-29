namespace Day21;

public class Processor
{
    public async Task Run(string path, int robotsCount)
    {
        var input = await File.ReadAllLinesAsync(path);
        
        var cache = new Dictionary<string, long>[robotsCount + 1];
        for (var i = 0; i <= robotsCount; i++)
            cache[i] = new Dictionary<string, long>();
        
        var total = input.Sum(code => CalculatePath(code, robotsCount, cache));

        Console.WriteLine($"Total: {total}");
    }

    private long CalculatePath(string code, int numberOfRobots, Dictionary<string, long>[] cache)
    {
        var numericCodes = new List<string>();
        CalculatePathNumValues(code, "", 11, numericCodes);
        
        var shortestSequence = numericCodes.Select(numericCode 
                => Utils.CalculateCost(numericCode, numberOfRobots, cache))
            .Prepend(long.MaxValue)
            .Min();

        var numericPart = code.Where(char.IsDigit).Aggregate(0, (current, t) => current * 10 + t - '0');
        
        return shortestSequence * numericPart;
    }

    private void CalculatePathNumValues(string code, string path, int location, List<string> paths)
    {
        if (code == "")
        {
            paths.Add(path);
            return;
        }

        var symbol = code[0] switch
        {
            'A' => 11,
            '0' => 10,
            '1' => 6,
            '2' => 7,
            '3' => 8,
            '4' => 3,
            '5' => 4,
            '6' => 5,
            '7' => 0,
            '8' => 1,
            '9' => 2,
            _ => 9
        };

        foreach (var step in Utils.StepsToDigit[location][symbol])
            CalculatePathNumValues(code[1..], path + step, symbol, paths);
    }
}