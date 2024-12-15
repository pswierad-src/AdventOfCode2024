using Day15;

Console.WriteLine("Day 15");
Console.WriteLine($"Part 1, test: {await new Processor().Run("files/test.txt", false, 1)}");
Console.WriteLine($"Part 1, input: {await new Processor().Run("files/input.txt", false, 1)}");
Console.WriteLine($"Part 2, input: {await new Processor().Run("files/test.txt", false, 2)}");
Console.WriteLine($"Part 2, input: {await new Processor().Run("files/input.txt", false, 2)}");