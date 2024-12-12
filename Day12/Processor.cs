using System.Numerics;

namespace Day12;

public class Processor
{
    private HashSet<(int y, int x)> visited = new();
    private List<int> _fencePlotCount = new();
    private Dictionary<int, List<(int y, int x)>> _plots = new();

    private int _test;

    public async Task Run(string path)
    {
        var land = (await File.ReadAllLinesAsync(path)).Select(l => l.ToCharArray()).ToArray();

        for (int y = 0; y < land.Length; y++)
        {
            for (int x = 0; x < land[0].Length; x++)
            {
                _plots.Add(_test, new List<(int y, int x)>());
                
                var fencesForThisPlot = CheckSurroundingPlotFence(y, x, land, 0, 0);

                _fencePlotCount.Add(fencesForThisPlot.fenceCount * fencesForThisPlot.plotCount);

                _test++;
            }
        }

        Console.WriteLine($"Total plot fences count: {_fencePlotCount.Sum()}");

        var sums = new List<int>();
        
        foreach (var plot in _plots.Where(v => v.Value.Count > 0))
        {
            var corners = 0;
            
            foreach (var cell in plot.Value)
            {
                corners += CheckCorners(land, cell.y, cell.x);
            }

            Console.WriteLine($"Plot {plot.Key}, corners: {corners}, count: {plot.Value.Count * corners}");

            sums.Add(plot.Value.Count * corners);
        }
        
        Console.WriteLine($"Total sum: {sums.Sum()}");
    }
    
    private (int fenceCount, int plotCount) CheckSurroundingPlotFence(int y, int x, char[][] land, int fenceCount, int plotCount)
    {
        if (!visited.Add((y, x)))
            return (fenceCount, plotCount);
        
        _plots[_test].Add((y, x));

        plotCount += 1;
        
        var steps = new List<(int y, int x)>
        {
            (y - 1, x), (y, x + 1), (y + 1 , x), (y, x - 1)
        };

        foreach (var step in steps)
        {
            if (TryGetPlot(land, step.y, step.x, out var plot) && land[y][x] == plot)
            {
                (fenceCount, plotCount) = CheckSurroundingPlotFence(step.y, step.x, land, fenceCount, plotCount);
                continue;
            }

            fenceCount++;
        }

        return (fenceCount, plotCount);
    }

    private int CheckCorners(char[][] land, int y, int x)
    {
        var corners = 0;
        TryGetPlot(land, y, x, out var current);

        TryGetPlot(land, y - 1, x, out var up);
        TryGetPlot(land, y + 1, x, out var down);
        TryGetPlot(land, y, x - 1, out var left);
        TryGetPlot(land, y, x+1, out var right);
        
        TryGetPlot(land, y - 1, x - 1, out var leftup);
        TryGetPlot(land, y - 1, x + 1, out var rightup);
        TryGetPlot(land, y + 1, x + 1, out var rightdown);
        TryGetPlot(land, y+1, x-1, out var leftdown);

        if (up != current && down != current && left != current && right != current)
            return 4;

        //horiz and vert
        if (left == current && right == current && up != current && down != current
            || up == current && down == current && left != current && right != current)
            return 0;

        if (left == current && right == current && up == current && down == current)
        {
            if(rightup != current)
                corners++;
            
            if(rightdown != current)
                corners++;
            
            if(leftdown != current)
                corners++;
            
            if(leftup != current)
                corners++;
            
            return corners;
        }

        if (left == current && up == current && right == current)
        {
            if(leftup != current && leftup != '#')
                corners++;
            
            if(rightup != current && rightup != '#')
                corners++;
            
            return corners;
        }
        
        if (up == current && right == current && down == current)
        {
            if(rightup != current && rightup != '#')
                corners++;
            
            if(rightdown != current && rightdown != '#')
                corners++;
            
            return corners;
        }
        
        if (right == current && down == current && left == current)
        {
            if(rightdown != current && rightdown != '#')
                corners++;
            
            if(leftdown != current && leftdown != '#')
                corners++;
            
            return corners;
        }
        
        if (down == current && left == current && up == current)
        {
            if(leftdown != current && leftdown != '#')
                corners++;
            
            if(leftup != current && leftup != '#')
                corners++;
            
            return corners;
        }
        
        
        
        if (up == current && right == current)
        {
            corners++;
            
            if(rightup != current && rightup != '#')
                corners++;
            
            return corners;
        }
        
        if (right == current && down == current)
        {
            corners++;
            
            if(rightdown != current && rightdown != '#')
                corners++;
            
            return corners;
        }
        
        if (down == current && left == current)
        {   
            corners++;
            
            if(leftdown != current && leftdown != '#')
                corners++;
        
            return corners;
        }

        if (left == current && up == current)
        {   
            corners++;
            
            if(leftup != current && leftup != '#')
                corners++;
        
            return corners;
        }
        
        if (up == current || right == current || down == current || left == current)
            return 2;

        return corners;
    }

    private bool TryGetPlot(char[][] land, int y, int x, out char plot)
    {
        try
        {
            plot = land[y][x];
            return true;
        }
        catch (Exception e)
        {
            plot = '#';
            return false;
        }
    }
    
    
}