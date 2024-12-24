using System.Collections.Concurrent;

namespace Day22;

public class Processor
{
    private ConcurrentBag<long> _secretNumbers = new();
    private List<Dictionary<long, uint>> _offsets = new();
    private List<long> _numbers = new();
    
    public async Task Run(string path)
    {
        _numbers = (await File.ReadAllLinesAsync(path)).Select(long.Parse).ToList();

        Task.WaitAll(_numbers.Select(CalculateSecretNumber).ToArray());
        
        Console.WriteLine($"Total numbers: {_secretNumbers.Sum()}"); 
        
        var resultOffsets = new Dictionary<long, long>();
        
        for (var i = 0; i < _numbers.Count; i++)
        {
            var next = _offsets[i];
            foreach (var pair in next)
                resultOffsets[pair.Key] = resultOffsets.GetValueOrDefault(pair.Key) + pair.Value;
        }

        Console.WriteLine($"Total offsets: {resultOffsets.Values.Max()}");
    }

    private Task CalculateSecretNumber(long startingNumber)
    {
        var secretNumber = (uint)startingNumber;
        var previousPrice = secretNumber % 10;
        uint offsets = 0;
        var visited = new Dictionary<long, uint>();
        
        for (var i = 0; i < 2000; i++)
        {
            var nextNumber = secretNumber << 6;
            secretNumber = nextNumber ^ secretNumber;
            secretNumber %= 16777216;

            nextNumber = (secretNumber >> 5);
            secretNumber = nextNumber ^ secretNumber;
            secretNumber %= 16777216;

            nextNumber = secretNumber << 11;
            secretNumber = nextNumber ^ secretNumber;
            secretNumber %= 16777216;

            var offset = (secretNumber % 10 - previousPrice) + 9;
            
            offsets = (offsets << 8) + offset;
            previousPrice = secretNumber % 10;
            
            visited.TryAdd(offsets, secretNumber % 10);
        }
        _secretNumbers.Add(secretNumber);
        _offsets.Add(visited);

        return Task.CompletedTask;
    }
}