namespace Day11;

public class Processor
{
    private readonly Dictionary<(long, int), long> _simulatedStones = new();
    
    public async Task Run(string path)
    {
        var stones = (await File.ReadAllTextAsync(path)).Split(" ").Select(long.Parse).ToList();

        Console.WriteLine($"Initial arrangement: {string.Join(", ", stones)}");
        
        var blink25 = stones.Sum(stone => HandleStone(stone, 25));
        var blink75 = stones.Sum(stone => HandleStone(stone, 75));

        Console.WriteLine($"Sum of stones for 25 blinks: {blink25}");
        Console.WriteLine($"Sum of stones for 75 blinks: {blink75}");
    }

    private long HandleStone(long stone, int times)
    {
        if(_simulatedStones.TryGetValue((stone, times), out var value))
            return value;

        if (times == 0)
            return _simulatedStones[(stone,times)] = 1;

        if (stone == 0)
            return _simulatedStones[(stone, times)] = HandleStone(1, times - 1);
            
        if (stone.ToString().Length % 2 == 0)
        {
            var stoneStr = stone.ToString();

            var left = long.Parse(stoneStr[..(stoneStr.Length / 2)]);
            var right = long.Parse(stoneStr[(stoneStr.Length / 2)..]);
            
            return _simulatedStones[(stone, times)] = HandleStone(left, times - 1) + HandleStone(right, times - 1);
        }
            
        return HandleStone(stone * 2024, times - 1);
    }
}