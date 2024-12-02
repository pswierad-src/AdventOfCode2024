// See https://aka.ms/new-console-template for more information

using Day1;

await Processor.RunTaskOne("files/test.txt");
await Processor.RunTaskOne("files/input.txt");
Console.WriteLine("");
await Processor.RunTaskTwo("files/test.txt");
await Processor.RunTaskTwo("files/input.txt");

Console.ReadKey();
