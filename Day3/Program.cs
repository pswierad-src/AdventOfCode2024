using Day3;

await Processor.RunTaskOne("files/test.txt");
await Processor.RunTaskOne("files/input.txt");
Console.WriteLine();
await Processor.RunTaskTwo("files/test2.txt");
await Processor.RunTaskTwo("files/input.txt");
