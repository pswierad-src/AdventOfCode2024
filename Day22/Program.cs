using Day22;

await new Processor().Run("files/test.txt");
Console.WriteLine();
await new Processor().Run("files/input.txt");