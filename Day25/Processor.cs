namespace Day25;

public class Processor
{
    private List<List<int>> _locks = new();
    private List<List<int>> _keys = new(); 
    
    public async Task Run(string path)
    {
        var input = (await File.ReadAllTextAsync(path)).Split(Environment.NewLine + Environment.NewLine);

        foreach (var combination in input)
        {
            var currentCombination = combination.Split(Environment.NewLine)[1..^1].ToList();
            
            var currentTumbler = new List<int>();
                
            for (var i = 0; i < currentCombination[0].Length; i++)
            {
                var a = currentCombination.Select(cl => cl[i]).ToList();
                    
                currentTumbler.Add(a.Count(cl => cl == '#'));
            }
            
            switch (combination.First())
            {
                case '#':
                    //lock
                    _locks.Add(currentTumbler);
                    break;
                case '.':
                    //key
                    _keys.Add(currentTumbler);
                    break;
            }
        }

        var fittingPairs = 0;

        foreach (var l in _locks)
        {
            foreach (var k in _keys)
            {
                if(MatchKeyLock(l, k))
                    fittingPairs++;
            }
        }

        Console.WriteLine($"Part 1: {fittingPairs} fitting pairs");
    }

    private bool MatchKeyLock(List<int> l, List<int> k)
    {
        for (int i = 0; i < 5; i++)
        {
            if (k[i] + l[i] > 5)
            {
                Console.WriteLine($"Lock ({string.Join(',', l)}) overlaps with key ({string.Join(',', k)}) on {i} column");
                return false;
            }
        }

        Console.WriteLine($"Lock ({string.Join(',', l)}) fits with key ({string.Join(',', k)})");
        return true;
    }
}