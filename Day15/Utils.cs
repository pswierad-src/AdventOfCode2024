namespace Day15;

public static class Utils
{
    public static int CalculateGPS(char[][] map, char boxIndicator)
    {
        var gps = 0;

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[0].Length; x++)
            {
                if (map[y][x] == boxIndicator)
                    gps += (100 * y) + x;
            }
        }

        return gps;
    }
    
    public static void PrintPlane(char[][] map)
    {
        Console.Clear();

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[0].Length; j++)
            {
                Console.Write(map[i][j]);
            }
            
            Console.Write(Environment.NewLine);
        }
    }
    
    public static (int y, int x) GetRobotPosition(char[][] map)
    {
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[0].Length; x++)
            {
                if (map[y][x] == '@')
                    return (y, x);
            }
        }
        
        throw new Exception("Robot not found");
    }
    
    public static char[] ConvertToWide(char i)
    {
        return i switch
        {
            ROBOT => WIDE_ROBOT,
            BOX => WIDE_BOX,
            WALL => WIDE_WALL,
            FREE => WIDE_FREE,
            _ => throw new NotImplementedException(),
        };
    }
    
    public static readonly Dictionary<char, (int dy, int dx)> Directions = new() 
    {
        ['^'] = ( -1, 0),
        ['>'] = ( 0,  1),
        ['v'] = ( 1,  0),
        ['<'] = (0,  -1)
    };
    
    public const char ROBOT = '@';
    public const char BOX = 'O';
    public const char WALL = '#';
    public const char FREE = '.';

    public const char BOX_LEFT = '[';
    public const char BOX_RIGHT = ']';

    public static char[] WIDE_ROBOT = [ROBOT, FREE];
    public static char[] WIDE_BOX = [BOX_LEFT, BOX_RIGHT];
    public static char[] WIDE_WALL = [WALL, WALL];
    public static char[] WIDE_FREE = [FREE, FREE];
}