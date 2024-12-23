namespace Day20;

public class Node : IComparable<Node>
{
    public int X { get; set; }
    public int Y { get; set; }
    public double G { get; set; }
    public double H { get; set; }
    public double F => G + H;
    public Node Parent { get; set; }

    public Node(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int CompareTo(Node other)
    {
        return F.CompareTo(other.F);
    }

    public override bool Equals(object obj)
    {
        if (obj is Node other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> _data;

    public PriorityQueue()
    {
        _data = new List<T>();
    }

    public void Enqueue(T item)
    {
        _data.Add(item);
        var ci = _data.Count - 1;
        while (ci > 0)
        {
            var pi = (ci - 1) / 2;
            if (_data[ci].CompareTo(_data[pi]) >= 0)
                break;
            (_data[ci], _data[pi]) = (_data[pi], _data[ci]);
            ci = pi;
        }
    }

    public T Dequeue()
    {
        var li = _data.Count - 1;
        var frontItem = _data[0];
        _data[0] = _data[li];
        _data.RemoveAt(li);

        --li;
        var pi = 0;
        while (true)
        {
            var ci = pi * 2 + 1;
            if (ci > li) break;
            var rc = ci + 1;
            if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0)
                ci = rc;
            if (_data[pi].CompareTo(_data[ci]) <= 0) break;
            (_data[pi], _data[ci]) = (_data[ci], _data[pi]);
            pi = ci;
        }
        return frontItem;
    }

    public int Count => _data.Count;
}

public class AStar
{
    private readonly int _width;
    private readonly int _height;
    private readonly bool[,] _grid;

    public AStar(int width, int height, bool[,] grid)
    {
        _width = width;
        _height = height;
        _grid = grid;
    }

    public List<Node> FindPath(Node start, Node end)
    {
        var openSet = new PriorityQueue<Node>();
        var openSetDict = new Dictionary<(int, int), Node>();
        var closedSet = new HashSet<(int, int)>();

        start.G = 0;
        start.H = CalculateHeuristic(start, end);
        openSet.Enqueue(start);
        openSetDict[(start.X, start.Y)] = start;

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();
            var currentPos = (current.X, current.Y);
            openSetDict.Remove(currentPos);

            if (current.X == end.X && current.Y == end.Y)
            {
                return ReconstructPath(current);
            }

            closedSet.Add(currentPos);

            foreach (var neighbor in GetNeighbors(current))
            {
                var neighborPos = (neighbor.X, neighbor.Y);
                if (closedSet.Contains(neighborPos))
                    continue;

                var movementCost = Math.Abs(current.X - neighbor.X) + Math.Abs(current.Y - neighbor.Y) > 1
                    ? Math.Sqrt(2)
                    : 1.0;

                var tentativeG = current.G + movementCost;

                if (openSetDict.TryGetValue(neighborPos, out var existingNeighbor))
                {
                    if (tentativeG >= existingNeighbor.G)
                        continue;

                    existingNeighbor.G = tentativeG;
                    existingNeighbor.Parent = current;
                }
                else
                {
                    neighbor.G = tentativeG;
                    neighbor.H = CalculateHeuristic(neighbor, end);
                    neighbor.Parent = current;
                    openSet.Enqueue(neighbor);
                    openSetDict[neighborPos] = neighbor;
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighbors(Node node)
    {
        var neighbors = new List<Node>();
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (var i = 0; i < dx.Length; i++)
        {
            var newX = node.X + dx[i];
            var newY = node.Y + dy[i];

            if (!IsValidPosition(newX, newY) || _grid[newX, newY])
                continue;

            if (i < 4 && _grid[node.X + dx[i], node.Y] && _grid[node.X, node.Y + dy[i]])
                continue;

            neighbors.Add(new Node(newX, newY));
        }

        return neighbors;
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    private static double CalculateHeuristic(Node node, Node end)
    {
        double dx = Math.Abs(node.X - end.X);
        double dy = Math.Abs(node.Y - end.Y);
        return (dx + dy) + (Math.Sqrt(2) - 2) * Math.Min(dx, dy);
    }

    private static List<Node> ReconstructPath(Node? node)
    {
        var path = new List<Node>();
        var current = node;

        while (current != null)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }
}
