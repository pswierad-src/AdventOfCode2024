using MathNet.Numerics.LinearAlgebra;

namespace Day13;

public class Processor
{
    public async Task Run(string path)
    {
        var examples = (await File.ReadAllTextAsync(path)).Split(Environment.NewLine + Environment.NewLine)
            .Select(l => l.Split(Environment.NewLine)).ToArray();

        var machinesSpec = new List<EquationElements>();
        var tokens = new List<long>();
        
        foreach (var example in examples)
        {
            var (xa, ya) = GetValues(example[0], '+');
            var (xb, yb) = GetValues(example[1], '+');
            var (prizeX, prizeY) = GetValues(example[2], '=');
            
            machinesSpec.Add(new EquationElements(xa, xb, ya, yb, prizeX, prizeY));
        }

        foreach (var spec in machinesSpec)
        {
            var values = Vector<double>.Build.Dense([spec.X, spec.Y]);
            var equation = Matrix<double>.Build.DenseOfColumnArrays([spec.Xa, spec.Ya], [spec.Xb, spec.Yb]);
            
            var solve = equation.Solve(values).Select(x => Math.Round(x, 3)).ToList();
            
            if(solve.Any(i => i == 0 || i % 1 != 0 || i > 100))
                continue;
            
            tokens.Add((long)(solve[0] * 3 + solve[1] * 1));
        }

        Console.WriteLine($"Sum of tokens needed: {tokens.Sum()}");

        tokens = [];
        
        foreach (var spec in machinesSpec)
        {
            var values = Vector<double>.Build.Dense([spec.X + 10000000000000, spec.Y + 10000000000000]);
            var equation = Matrix<double>.Build.DenseOfColumnArrays([spec.Xa, spec.Ya], [spec.Xb, spec.Yb]);
            
            var solve = equation.Solve(values).Select(x => Math.Round(x, 3)).ToList();
            
            if(solve.Any(i => i == 0 || i % 1 != 0))
                continue;
            
            tokens.Add((long)(solve[0] * 3 + solve[1] * 1));
        }
        
        Console.WriteLine($"Sum of tokens needed for adjusted positions: {tokens.Sum()}");
    }

    private (long x, long y) GetValues(string input, char splitter)
    {
        var rawParams = input.Split(':')[1].TrimStart(' ').Split(',');

        return (
            long.Parse(rawParams[0].Split(splitter)[1]),
            long.Parse(rawParams[1].Split(splitter)[1])
        );
    }

    public record EquationElements(long Xa, long Xb, long Ya, long Yb, long X, long Y);
}