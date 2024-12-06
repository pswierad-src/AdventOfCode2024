namespace Day6;

public class Processor
{
    public async Task Run(string path)
    {
        var seen = new HashSet<(int y, int x)>();
        var direction = 0;

        var map = (await File.ReadAllLinesAsync(path)).Select(l => l.ToCharArray()).ToArray();
        (int y, int x) currentPosition = map.GetStartingGuardPos();

        Task1();
        Task2();

        void Task1()
        {
            seen.Add(currentPosition);

            while (true)
            {
                var nextPos = currentPosition.Move(direction % 4);

                if (!map.Contains(nextPos))
                    break;

                if(map[nextPos.y][nextPos.x] == '#')
                    direction++;
                else
                {
                    currentPosition = nextPos;
                    seen.Add(currentPosition);
                }
            }

            Console.WriteLine($"Seen: {seen.Count}");
        }

        void Task2()
        {
            var possibleObstructionCount = 0;

            foreach (var position in seen.Skip(1))
            {
                currentPosition = map.GetStartingGuardPos();
                direction = 0;

                var prevChar = map[position.y][position.x];
                map[position.y][position.x] = '#';

                var possibleLoop = new HashSet<((int y, int x), int direction)>();

                while (true)
                {
                    var nextPos = currentPosition.Move(direction % 4);

                    if (!map.Contains(nextPos))
                        break;

                    if(map[nextPos.y][nextPos.x] == '#')
                        direction++;
                    else
                    {
                        if (!possibleLoop.Add((currentPosition, direction % 4)))
                        {
                            possibleObstructionCount++;
                            break;
                        }

                        currentPosition = nextPos;
                    }
                }

                map[position.y][position.x] = prevChar;
            }

            Console.WriteLine($"Possible obstruction count: {possibleObstructionCount}");
        }
    }
}

public static class Utils
{
    public static (int y, int x) GetStartingGuardPos(this char[][] map)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '^')
                {
                    return (y, x);
                }
            }
        }

        return (-1, -1);
    }

    public static (int y, int x) Move(this (int y, int x) currentPos, int direction)
    {
        return direction switch
        {
            0 => (currentPos.y - 1, currentPos.x), //^
            1 => (currentPos.y, currentPos.x + 1), //>
            2 => (currentPos.y + 1, currentPos.x), //v
            3 => (currentPos.y, currentPos.x - 1), //<
            _ => throw new Exception()
        };
    }

    public static bool Contains(this char[][] map, (int y, int x) position)
        => Enumerable.Range(0, map.Length).Contains(position.y)
           && Enumerable.Range(0, map[0].Length).Contains(position.x);
}
