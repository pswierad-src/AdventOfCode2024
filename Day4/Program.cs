using Day4;

await Processor.RunTaskOne("files/test.txt");
await Processor.RunTaskOne("files/input.txt");
Console.WriteLine();
await Processor.RunTaskTwo("files/test.txt");
await Processor.RunTaskTwo("files/input.txt");
