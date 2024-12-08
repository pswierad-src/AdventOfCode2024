namespace Day8;

public class Processor
{
    public async Task Run(string path)
    {
        var input = (await File.ReadAllLinesAsync(path)).Select(l => l.ToCharArray()).ToArray();
        
        var antennas = input
            .Select((line, y)
                => line.Select((code, x)
                        => (code, y, x))
                    .Where(e => e.code != '.')
                    .ToList())
            .SelectMany(l => l).ToList();
        
        var antinodes = new HashSet<(int, int)>();
        var resonatingAntinodes = new HashSet<(int, int)>();

        foreach (var code in antennas.Select(a => a.code).Distinct().ToList())
        {
            var filteredAntennas = antennas.Where(a => a.code == code).ToList();

            foreach (var antenna in filteredAntennas)
            {
                var potentialAntinodes = GetPotentialAntinodes(
                    (antenna.y, antenna.x),
                    filteredAntennas.Where(a => a.y != antenna.y && a.x != antenna.x).Select(a => (a.y, a.x)).ToList(),
                    false
                );
                
                var potentialResonantAntinodes = GetPotentialAntinodes(
                    (antenna.y, antenna.x),
                    filteredAntennas.Where(a => a.y != antenna.y && a.x != antenna.x).Select(a => (a.y, a.x)).ToList(),
                    true
                );

                foreach (var antinode in FilterOutOfBounds(potentialAntinodes, input.Length, input[0].Length))
                    antinodes.Add(antinode);
                
                foreach (var antinode in FilterOutOfBounds(potentialResonantAntinodes, input.Length, input[0].Length))
                    resonatingAntinodes.Add(antinode);
            }
        }

        Console.WriteLine($"Potential locations for antinodes: {antinodes.Count}");
        Console.WriteLine($"Potential locations for resonant antinodes: {resonatingAntinodes.Count}");
    }

    private List<(int y, int x)> GetPotentialAntinodes((int y, int x) from, List<(int y, int x)> antennas, bool calculateWithResonance)
    {
        List<(int y, int x)> potentialAntinodesDistances = antennas.Select(a => (a.y - from.y, a.x - from.x)).ToList();
        
        if(!calculateWithResonance)
            return potentialAntinodesDistances.Select(distance => (from.y - distance.Item1, from.x - distance.Item2)).ToList();
        
        var potentialAntinodes = new List<(int y, int x)>();
        
        foreach (var distance in potentialAntinodesDistances)
        {
            (int y, int x) antinodeCoords = (from.y - distance.y, from.x - distance.x);
            (int y, int x) nodeCoords = (from.y + distance.y, from.x + distance.x);
            
            //If it works it works :P
            for (var i = 0; i < 100; i++)
            {
                potentialAntinodes.Add(antinodeCoords);
                potentialAntinodes.Add(nodeCoords);
                
                antinodeCoords = (antinodeCoords.y - distance.y, antinodeCoords.x - distance.x);
                nodeCoords = (nodeCoords.y + distance.y, nodeCoords.x + distance.x);
            }
        }
        
        return potentialAntinodes;
    }

    private List<(int y, int x)> FilterOutOfBounds(List<(int y, int x)> antinodes, int mapY, int mapX) =>
        antinodes.Where(a 
                => Enumerable.Range(0, mapY).Contains(a.y) 
                   && Enumerable.Range(0, mapX).Contains(a.x))
            .ToList();
}