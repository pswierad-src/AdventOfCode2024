using System.Drawing;
using System.Drawing.Imaging;

namespace Day14;

public class Processor
{
    private int planeY;
    private int planeX;
    private int[,] _plane;
    private List<Robot> _robots = new ();
    
    public async Task Run(string path, int tilesWide, int tilesTall)
    {
        planeY = tilesTall;
        planeX = tilesWide;
        _plane = new int[planeY, planeX];
        
        await SetupRobots(path);
        //PrintPlane(0);

        //var iterations = 100;
        var iterations = int.MaxValue;
        
        for (var t = 0; t < iterations; t++)
        {
            for (var i = 0; i < _robots.Count; i++)
            {
                _robots[i] = MoveRobot(_robots[i]);
            }
            
            //PrintPlane(t+1);
            
            Console.WriteLine($"Iteration {t+1}");
            CreateBitmap(t+1);
        }

        var topLeft = CalculateQuadrant(0, 0, planeX / 2, planeY / 2);
        var topRight = CalculateQuadrant((planeX/2)+1,0, planeX, planeY/2);
        var bottomLeft = CalculateQuadrant(0,(planeY/2)+1, planeX/2, planeY);
        var bottomRight = CalculateQuadrant((planeX/2)+1,(planeY/2)+1, planeX, planeY);
        
        var mult = topLeft * topRight * bottomLeft * bottomRight;
        
        Console.WriteLine(mult);
    }

    private void CreateBitmap(int iteration)
    {
        using var bitmap = new Bitmap(planeX, planeY);
        
        var bmpData = bitmap.LockBits(
            new Rectangle(0, 0, planeX, planeY), 
            ImageLockMode.WriteOnly, 
            PixelFormat.Format1bppIndexed
        );
            
        try
        {
            var stride = Math.Abs(bmpData.Stride);
            var rgbValues = new byte[stride * planeY];

            for (var y = 0; y < planeY; y++)
            {
                for (var x = 0; x < planeX; x++)
                {
                    var byteIndex = y * stride + (x / 8);
                    var bitIndex = 7 - (x % 8);

                    if (_plane[y, x] > 0)
                    {
                        rgbValues[byteIndex] |= (byte)(1 << bitIndex);
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
        }
        finally
        {
            bitmap.UnlockBits(bmpData);
        }

        bitmap.Save($"C:\\renders\\{iteration}.jpg", ImageFormat.Jpeg);
    }

    private int CalculateQuadrant(int fromX, int fromY, int toX, int toY)
    {
        var sum = 0;
        
        for (var i = fromY; i < toY; i++)
        {
            for (var j = fromX; j < toX; j++)
            {
                sum += _plane[i, j];
            }
        }
        
        return sum;
    }

    private Robot MoveRobot(Robot robot)
    {
        _plane[robot.Y, robot.X]--;
        
        robot.Y += robot.YVel;
        robot.X += robot.XVel;
        
        if(robot.Y >= planeY)
            robot.Y -= planeY;
        
        if(robot.Y < 0)
            robot.Y = planeY + robot.Y;
        
        if(robot.X >= planeX)
            robot.X -= planeX;
        
        if(robot.X < 0)
            robot.X = planeX + robot.X;

        _plane[robot.Y, robot.X]++;
        
        return robot;
    }

    private async Task SetupRobots(string path)
    {
        _robots = new List<Robot>();
        
        var lines = (await File.ReadAllLinesAsync(path)).Select(l => l.Split(" "));

        foreach (var line in lines.Select((value, i) => new { value, i}))
        {
            var position = line.value[0].Split(",");
            var velocity = line.value[1].Split(",");

            var robotToAdd = new Robot
            {
                Number = line.i,
                X = int.Parse(position[0].TrimStart('p').TrimStart('=')),
                Y = int.Parse(position[1]),
                XVel = int.Parse(velocity[0].TrimStart('v').TrimStart('=')),
                YVel = int.Parse(velocity[1])
            };

            _robots.Add(robotToAdd);

            _plane[robotToAdd.Y, robotToAdd.X]++;
        }
    }
    
    private void PrintPlane(int iteration)
    {
        Console.Clear();
        Console.WriteLine($"Iteration {iteration}");
        Console.WriteLine();

        for (var i = 0; i < planeY; i++)
        {
            for (var j = 0; j < planeX; j++)
            {
                if(_plane[i,j] == 0)
                    Console.Write('.');
                else
                    Console.Write(_plane[i,j]);
            }
            
            Console.Write(Environment.NewLine);
        }
    }
}