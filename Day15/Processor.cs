namespace Day15;

public class Processor
{
    private char[][] _map;
    private char[] _instructions;
    
    public async Task<int> Run(string path, bool print, int runPart)
    {
        var input = (await File.ReadAllTextAsync(path)).Split(Environment.NewLine + Environment.NewLine);
        _map = input[0].Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        _instructions = input[1].ToCharArray();
        
        return runPart switch
        {
            1 => RunPartOne(print),
            2 => RunPartTwo(print),
            _ => throw new NotImplementedException()
        };
    }
    
    private int RunPartTwo(bool print)
    {
        var wideMap = new char[_map.Length][];
        
        for (var y = 0; y < _map.Length; y++)
        {
            var xInd = 0;
            wideMap[y] = new char[_map[y].Length * 2];
            
            for (var x = 0; x < _map[0].Length; x++)
            {
                var wide = Utils.ConvertToWide(_map[y][x]);

                wideMap[y][xInd] = wide[0];
                wideMap[y][xInd+1] = wide[1];
                xInd += 2;
            }
        }
        
        var robotPosition = Utils.GetRobotPosition(wideMap);
        
        if(print)
            Utils.PrintPlane(wideMap);
        
        foreach (var instruction in _instructions.Where(i => i != '\r' && i != '\n'))
        {
            var mapCheckpoint = wideMap.Select(row => (char[])row.Clone()).ToArray();
            
            var dirV = Utils.Directions[instruction];

            if (MoveWide(robotPosition, dirV))
                robotPosition = (robotPosition.y + dirV.dy, robotPosition.x + dirV.dx);
            else
                wideMap = mapCheckpoint;
            
            if (print)
            {
                Utils.PrintPlane(wideMap);
                Thread.Sleep(10);    
            }
        }

        return Utils.CalculateGPS(wideMap, Utils.BOX_LEFT);
        
        bool MoveWide((int y, int x) position, (int dy, int dx) direction)
        {
            while (true)
            {
                (int y, int x) nextPosition = (position.y + direction.dy, position.x + direction.dx);

                switch (wideMap[nextPosition.y][nextPosition.x])
                {
                    case Utils.WALL:
                        return false;

                    case Utils.BOX_LEFT:
                        (int y, int x) rightPart = (nextPosition.y, nextPosition.x + 1);
                        if (MoveWide(rightPart, direction) && MoveWide(nextPosition, direction))
                            continue;
                        return false;

                    case Utils.BOX_RIGHT:
                        (int y, int x) leftPart = (nextPosition.y, nextPosition.x - 1);
                        if (MoveWide(leftPart, direction) && MoveWide(nextPosition, direction))
                            continue;
                        return false;

                    case Utils.FREE:
                        wideMap[nextPosition.y][nextPosition.x] = wideMap[position.y][position.x];
                        wideMap[position.y][position.x] = Utils.FREE;
                        return true;
                }

                return false;
            }
        }
    }
    
    private int RunPartOne(bool print)
    {
        var robotPosition = Utils.GetRobotPosition(_map);
        
        if(print)
            Utils.PrintPlane(_map);

        foreach (var instruction in _instructions.Where(i => i != '\r' && i != '\n'))
        {
            var dirV = Utils.Directions[instruction];
            
            if(Move(robotPosition, dirV))
                robotPosition = (robotPosition.y + dirV.dy, robotPosition.x + dirV.dx);

            if (print)
            {
                Utils.PrintPlane(_map);
                Thread.Sleep(10);    
            }
        }

        return Utils.CalculateGPS(_map, Utils.BOX);
        
        bool Move((int y, int x) position, (int dy, int dx) direction)
        {
            while (true)
            {
                (int y, int x) nextPosition = (position.y + direction.dy, position.x + direction.dx);

                switch (_map[nextPosition.y][nextPosition.x])
                {
                    case Utils.WALL:
                        return false;
                    case Utils.BOX:
                        if (Move(nextPosition, direction)) continue;
                        return false;
                    case Utils.FREE:
                        _map[nextPosition.y][nextPosition.x] = _map[position.y][position.x];
                        _map[position.y][position.x] = Utils.FREE;
                        return true;
                }

                return false;
            }
        }
    }
}