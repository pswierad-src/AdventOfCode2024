namespace Day19;

public class Processor
{
    private readonly Dictionary<string, long> _dictionary = new();
    
    public async Task Run(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);

        var towels = lines[0].Split(", ");
        var patterns = lines[2..];
        
        var combinations = patterns.Select(p => Match(towels, p)).ToList();
        Console.WriteLine($"Possible towel arrangements: {combinations.Count(c => c != 0)}");
        Console.WriteLine($"Total different arrangements: {combinations.Where(c => c != 0).Sum()}");
    }

    private long Match(string[] towels, string pattern)
    {
        long count;
        
        if (_dictionary.TryGetValue(pattern, out count))
            return count;

        count = pattern switch
        {
            "" => 1,
            _ => towels
                .Where(pattern.StartsWith)
                .Sum(towel => Match(towels, pattern[towel.Length ..]))
        };
        
        _dictionary.Add(pattern, count);

        return count;
    }
}