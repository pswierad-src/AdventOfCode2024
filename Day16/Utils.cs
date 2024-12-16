namespace Day16;

public static class Utils
{
    private static readonly int[] dx = [1, 0, -1, 0];
    private static readonly int[] dy = [0, -1, 0, 1];
    
    public static (int x, int y) FindPoint(char[,] map, char point)
    {
        for (var y = 0; y < map.GetLength(0); y++)
        {
            for (var x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == point)
                    return (x, y);
            }
        }
        
        throw new Exception($"Point {point} not found in maze");
    }
    
    public static void TryRotate(State current, PriorityQueue<State, int> pq, int rotationDirection)
    {
        var newOrientation = (current.Orientation + rotationDirection + 4) % 4;

        var newPath = new List<(int x, int y, int orientation)>(current.Path)
        {
            (current.X, current.Y, newOrientation)
        };

        var newState = new State(
            current.X, 
            current.Y, 
            newOrientation, 
            current.Cost + 1000, 
            newPath
        );

        pq.Enqueue(newState, newState.Cost);
    }
    
    public static void TryMove(char[,] map, State current, PriorityQueue<State, int> pq)
    {
        var newX = current.X + dx[current.Orientation];
        var newY = current.Y + dy[current.Orientation];

        if (IsValidMove(map, newX, newY))
        {
            var newPath = new List<(int x, int y, int orientation)>(current.Path);
            newPath.Add((newX, newY, current.Orientation));

            var newState = new State(
                newX, 
                newY, 
                current.Orientation, 
                current.Cost + 1, 
                newPath
            );

            pq.Enqueue(newState, newState.Cost);
        }
    }
    
    private static bool IsValidMove(char[,] map, int x, int y)
        => x >= 0 
           && x < map.GetLength(1) 
           && y >= 0 && y < map.GetLength(0) 
           && map[y, x] != '#';
    
    public static void PrintPath(char[,] map, List<(int x, int y, int orientation)> path)
    {
        var mazeCopy = (char[,])map.Clone();

        foreach (var step in path)
        {
            if (mazeCopy[step.y, step.x] != 'S' && mazeCopy[step.y, step.x] != 'E')
            {
                mazeCopy[step.y, step.x] = 'O';
            }
        }

        for (var y = 0; y < map.GetLength(0); y++)
        {
            for (var x = 0; x < map.GetLength(1); x++)
            {
                Console.Write(mazeCopy[y, x]);
            }
            Console.WriteLine();
        }
    }
}