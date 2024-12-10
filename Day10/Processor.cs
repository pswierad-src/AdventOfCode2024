namespace Day10;

public class Processor
{
    private int[][] _map;
    
    public async Task Run(string path)
    {
        _map = (await File.ReadAllLinesAsync(path))
            .Select(line => line.ToCharArray().Select(c => c - '0').ToArray()).ToArray();

        var trailHead = 0;
        var sumRating = 0;
        var startingPoints = GetStartingPoints();

        foreach (var paths in startingPoints.Select(s => GetPaths(s.y, s.x, [])))
        {
            trailHead += paths.Distinct().Count();
            sumRating += paths.Count;
        }

        Console.WriteLine($"Trailhead count: {trailHead}");
        Console.WriteLine($"Sum of all valid path rating: {sumRating}");
    }

    private List<(int y, int x)> GetPaths(int startY, int startX, List<(int y, int x)> paths)
    {
        var validNextSteps = GetValidNextSteps(startY, startX, _map);
        
        foreach (var validStep in validNextSteps)
        {
            if(_map[validStep.y][validStep.x] - _map[startY][startX] is not 1)
                continue;
            
            if(_map[validStep.y][validStep.x] == 9 &&  _map[startY][startX] != 9)
                paths.Add(validStep);
            else
                paths = GetPaths(validStep.y, validStep.x, paths);
        }
        
        return paths;
    }

    private List<(int y, int x)> GetValidNextSteps(int y, int x, int[][] map)
    {
        var steps = new List<(int y, int x)>
        {
            (y - 1, x), (y, x + 1), (y + 1 , x), (y, x - 1)
        };
        
        return steps.Where(step => IsValidStep(step.y, step.x, map)).ToList();
    }

    private bool IsValidStep(int y, int x, int[][] map) 
        => x >= 0 && y >= 0 && x < map.Length && y < map[0].Length;
    
    private List<(int y, int x)> GetStartingPoints()
    {
        var response = new List<(int y, int x)>();
        
        for (var y = 0; y < _map.Length; y++)
        {
            for (var x = 0; x < _map[0].Length; x++)
            {
                if(_map[y][x] == 0)
                    response.Add((y,x));
            }
        }

        return response;
    }
}