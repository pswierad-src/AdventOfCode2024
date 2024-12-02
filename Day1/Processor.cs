namespace Day1;

internal static class Processor
{
    public static async Task RunTaskOne(string path)
    {
        var leftNums = new List<int>();
        var rightNums = new List<int>();

        foreach (var line in await File.ReadAllLinesAsync(path))
        {
            var numbers = line.Split("   ");

            leftNums.Add(int.Parse(numbers[0]));
            rightNums.Add(int.Parse(numbers[^1]));
        }

        leftNums.Sort();
        rightNums.Sort();

        var distance = new List<int>();

        for (var i = 0; i < leftNums.Count; i++)
        {
            var l = leftNums[i];
            var r = rightNums[i];

            if(l > r)
                distance.Add(l-r);
            else
                distance.Add(r-l);
        }

        Console.WriteLine($"Running task 1 for: {path} Total distance: {distance.Sum()}");
    }

    public static async Task RunTaskTwo(string path)
    {
        var leftNums = new List<int>();
        var rightNums = new List<int>();

        foreach (var line in await File.ReadAllLinesAsync(path))
        {
            var numbers = line.Split("   ");

            leftNums.Add(int.Parse(numbers[0]));
            rightNums.Add(int.Parse(numbers[^1]));
        }

        leftNums.Sort();
        rightNums.Sort();

        Console.WriteLine($"Running task 2 for: {path} Total distance: {leftNums.Select(num => num * rightNums.Count(n => n == num)).Sum()}");
    }
}
