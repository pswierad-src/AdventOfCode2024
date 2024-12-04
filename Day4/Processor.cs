namespace Day4;

internal static class Processor
{
    public static async Task RunTaskOne(string path)
    {
        var input = (await File.ReadAllLinesAsync(path)).Select(l => l.ToCharArray()).ToArray();
        var xmasCount = 0;

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] != 'X')
                {
                    continue;
                }

                //Up
                Check(y - 1, x, y - 2, x, y - 3, x);
                //RightUp
                Check(y - 1, x + 1, y - 2, x + 2, y - 3, x + 3);
                //Right
                Check(y, x + 1, y, x + 2, y, x + 3);
                //RightDown
                Check(y + 1, x + 1, y + 2, x + 2, y + 3, x + 3);
                //Down
                Check(y + 1, x, y + 2, x, y + 3, x);
                //LeftDown
                Check(y+1,x-1,y+2,x-2,y+3,x-3);
                //Left
                Check(y, x - 1, y, x - 2, y, x - 3);
                //LeftUp
                Check(y - 1, x - 1, y - 2, x - 2, y - 3, x - 3);
            }
        }

        Console.WriteLine($"Running task 1 for: {path} Found XMAS: {xmasCount} times.");
        return;

        void Check(int y1, int x1, int y2, int x2, int y3, int x3)
        {
            try {
                if (input[y1][x1] == 'M' && input[y2][x2] == 'A' && input[y3][x3] == 'S')
                    xmasCount++;
            } catch { }
        }
    }

    public static async Task RunTaskTwo(string path)
    {
        var input = (await File.ReadAllLinesAsync(path)).Select(l => l.ToCharArray()).ToArray();
        var xmasCount = 0;

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] != 'M')
                {
                    continue;
                }

                //Top
                Check(y, x + 2, y + 1, x + 1, y + 2, x, y + 2, x + 2);
                //Right
                Check(y + 2, x, y + 1, x - 1, y + 2, x - 2, y, x - 2);
                //Bottom
                Check(y, x - 2, y - 1, x - 1, y - 2, x - 2, y - 2, x);
                //Left
                Check(y - 2, x, y - 1, x + 1, y - 2,x + 2, y, x + 2);
            }
        }

        Console.WriteLine($"Running task 2 for: {path} Found XMAS: {xmasCount} times.");
        return;

        void Check(int y1, int x1, int y2, int x2, int y3, int x3, int y4, int x4)
        {
            try
            {
                if (input[y1][x1] == 'M' && input[y2][x2] == 'A' && input[y3][x3] == 'S' && input[y4][x4] == 'S')
                    xmasCount++;
            }
            catch
            {
            }
        }
    }
}
