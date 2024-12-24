namespace Day23;

public class Processor
{
    private readonly Dictionary<string, HashSet<string>> _connections = new();
    
    public async Task Run(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);

        foreach (var line in lines)
        {
            var connection = line.Split("-");
            
            _connections.TryAdd(connection[0], new HashSet<string>());
            _connections.TryAdd(connection[1], new HashSet<string>());
            
            _connections[connection[0]].Add(connection[1]);
            _connections[connection[1]].Add(connection[0]);
        }

        var result = new HashSet<string>();

        foreach (var connection in _connections)
        {
            foreach (var connection2 in _connections[connection.Key])
            {
                foreach (var connection3 in _connections[connection2])
                {
                    if(_connections[connection.Key].Contains(connection3) 
                       && (connection.Key.StartsWith('t') || connection2.StartsWith('t') || connection3.StartsWith('t')))
                    {
                        string[] x = [connection.Key, connection2, connection3];
                            
                        result.Add(string.Join(",", x.OrderBy(el => el).ToList()));
                    }
                }
            }
        }
        
        var pcSet = new HashSet<string>();
        
        foreach (var node in _connections.Keys)
            BronKerboshAlgorithm([node], [.._connections[node]], [], pcSet);

        Console.WriteLine($"Sets with at least one T: {result.Count}");
        Console.WriteLine($"Password: {pcSet.MaxBy(c => c.Length)}");
    }

    private void BronKerboshAlgorithm(HashSet<string> r, HashSet<string> p, HashSet<string> x, HashSet<string> o)
    {
        if (p.Count == 0 && x.Count == 0)
        {
            o.Add(string.Join(",", r.OrderBy(el => el).ToList()));
            return;
        }

        HashSet<string> next = [..p];

        foreach (var candidate in p)
        {
            BronKerboshAlgorithm(
                [..r.Union([candidate])],
                [..next.Intersect(_connections[candidate])],
                [..x.Intersect(_connections[candidate])],
                o);
            
            next.Remove(candidate);
            x.Add(candidate);
        }
        
    }
}