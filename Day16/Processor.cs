namespace Day16;

public class Processor
{
    private char[,] _map;
    private int rows, cols;
    
    private HashSet<(int,int)> _seats = []; 
    private Dictionary<((int, int), (int, int)), int> _seen = [];

    public async Task Run(string path, bool print = false)
    {
        var input = await File.ReadAllLinesAsync(path);
        
        rows = input.Length;
        cols = input[0].Length;
        _map = new char[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                _map[i, j] = input[i][j];
            }
        }

        var shortestPaths = FindShortestPaths();
        
        Console.WriteLine($"Shortest Path Cost: {shortestPaths.Item1[0].Cost}");
        
        if(print)
            Utils.PrintPath(_map, shortestPaths.Item1[0].Path);

        var start = Utils.FindPoint(_map, 'S');
        
        Console.WriteLine($"Seats: {GetPaths([], (start.y, start.x) , (0,1),0, [], shortestPaths.Item1[0].Cost)}");
    }
    
    private (List<State>, HashSet<(int x, int y)>) FindShortestPaths()
    {
        var (sx, sy) = Utils.FindPoint(_map, 'S');
        var (ex, ey) = Utils.FindPoint(_map, 'E');

        var pq = new PriorityQueue<State, int>();
        var visited = new Dictionary<(int x, int y, int orientation), int>();
        
        var allShortestPaths = new List<State>();
        var uniqueTilesAcrossPaths = new HashSet<(int x, int y)>();
        var lowestCost = int.MaxValue;

        var startPath = new List<(int x, int y, int orientation)> 
            { (sx, sy, 0) };
        uniqueTilesAcrossPaths.Add((startPath[0].x, startPath[0].y));
        
        pq.Enqueue(new State(sx, sy, 0, 0, startPath), 0);

        while (pq.Count > 0)
        {
            var current = pq.Dequeue();

            if (current.X == ex && current.Y == ey)
            {
                if (current.Cost < lowestCost)
                {
                    lowestCost = current.Cost;
                    allShortestPaths.Clear();
                    uniqueTilesAcrossPaths.Clear();
                }
                
                allShortestPaths.Add(current);

                foreach (var step in current.Path)
                {
                    if (_map[step.y, step.x] != '#')
                    {
                        uniqueTilesAcrossPaths.Add((step.x, step.y));
                    }
                }
                
                continue;
            }

            var stateKey = (current.X, current.Y, current.Orientation);
            if (visited.TryGetValue(stateKey, out int prevCost))
            {
                if (prevCost <= current.Cost)
                    continue;
            }
            visited[stateKey] = current.Cost;
            
            // if (visited.Contains(stateKey))
            //     continue;
            // visited.Add(stateKey);

            Utils.TryMove(_map, current, pq);
            Utils.TryRotate(current, pq, -1);
            Utils.TryRotate(current, pq, 1);
        }

        return (allShortestPaths, uniqueTilesAcrossPaths);
    }
    
    //Fucking hell - I swear next time I see some 2D puzzle I'm gonna snap
    private int GetPaths(HashSet<(int, int)> seen, (int, int) current, (int, int) dir, int cost, HashSet<(int, int)> path, int shortestCost)
    {
        if(_seen.GetValueOrDefault((current, dir), int.MaxValue) < cost) 
            return _seats.Count;
        
        _seen[(current, dir)] = cost;
        path.Add(current);
        if (cost == shortestCost && _map[current.Item1, current.Item2] == 'E')
        {
            foreach (var location in path.ToList())
                _seats.Add(location);
        }
        else if (cost < shortestCost)
        {
            seen.Add(current);

            var directions = new List<(int y, int x)>()
            {
                (0, 1), (0, -1), (1, 0), (-1, 0)
            };
            
            directions = directions.Where(x => Math.Abs(dir.Item1) != Math.Abs(x.Item1) && Math.Abs(dir.Item2) != Math.Abs(x.Item2)).ToList();
            directions.Add(dir);
            var n = directions
                .Where(_ =>_map[current.Item1, current.Item2] != '#')
                .Select(direction => ((current.Item1 + direction.Item1, current.Item2 + direction.Item2), direction))
                .ToList();
            
            foreach (var (pos, direction) in n)
            {
                if (seen.Contains(pos)) 
                    continue;
                
                GetPaths(seen, pos, direction, (Math.Abs(dir.Item1) != Math.Abs(direction.Item1) && Math.Abs(dir.Item2) != Math.Abs(direction.Item2) ? 1001 : 1) + cost, path, shortestCost);
            }
            seen.Remove(current);
        }
        path.Remove(current);

        return _seats.Count;
    }
}

public class State : IComparable<State>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Orientation { get; set; }
    public int Cost { get; set; }
    public List<(int x, int y, int orientation)> Path { get; set; }

    public State(int x, int y, int orientation, int cost, List<(int x, int y, int orientation)> path)
    {
        X = x;
        Y = y;
        Orientation = orientation;
        Cost = cost;
        Path = path;
    }

    public int CompareTo(State other)
    {
        return Cost.CompareTo(other.Cost);
    }
}
