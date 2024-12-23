namespace Day20;

public class Processor
{
    public async Task Run(string path, int saveAtLeast)
    {
        var lines = await File.ReadAllLinesAsync(path);
        var map = new bool[lines.Length,lines[0].Length];

        var startPos = new Node(0, 0);
        var endPos = new Node(0, 0);
        
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                map[x,y] = lines[x][y] == '#';
                
                if(lines[x][y] == 'S')
                    startPos = new Node(x, y);
                
                if(lines[x][y] == 'E')
                    endPos = new Node(x, y);
            }
        }
        
        var saves = GetCheats(map, lines.Length, lines[0].Length, startPos, endPos, saveAtLeast, 2);
        Console.WriteLine($"Saves with 2 ps jumps: {saves.Count}");

        var cheats = saves.GroupBy(x => x).Select(g => (g.Key, g.Count()))
            .OrderBy(g => g.Key).ToList();
        
        saves = GetCheats(map, lines.Length, lines[0].Length, startPos, endPos, saveAtLeast, 20);
        Console.WriteLine($"Saves with 20 ps jumps: {saves.Count}");
    }

    private List<int> GetCheats(bool[,] map, int width, int height, Node startPos, Node endPos,  int save, int upTo)
    {
        var pathFinder = new AStar(width, height, map);

        var path = pathFinder.FindPath(startPos, endPos).Select(node => (node.X, node.Y)).ToList();
        
        var pathSet = new Dictionary<int, int>();
        for (var i = 0; i < path.Count; i++)
        {
            pathSet.Add(Hash(path[i].X, path[i].Y), i);
        }

        var seen = new HashSet<int>();
        var saves = new List<int>();

        for (var start = 0; start < path.Count; start++)
        {
            for (var end = 2; end <= upTo; end++)
            {
                seen.Clear();
                for (var jumpDistance = 0; jumpDistance <= end; jumpDistance++)
                {
                    int[] cornerHashes =
                    [
                        Hash(path[start].X + jumpDistance, path[start].Y + end - jumpDistance),
                        Hash(path[start].X - jumpDistance, path[start].Y + end - jumpDistance),
                        Hash(path[start].X + jumpDistance, path[start].Y - end + jumpDistance),
                        Hash(path[start].X - jumpDistance, path[start].Y - end + jumpDistance)
                    ];

                    foreach (var hash in cornerHashes)
                    {
                        var corner = pathSet.GetValueOrDefault(hash, -1);
                        
                        if(corner-start-end >= save && corner != -1 && seen.Add(Hash(start, corner)))
                            saves.Add((corner - start - end));
                    }
                }
            }
        }
        return saves;
    }
    
    private int Hash(int x, int y) => x * 1000 + y;

}