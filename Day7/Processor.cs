namespace Day7;

public class Processor
{
    public async Task Run(string path)
    {
        var input = (await File.ReadAllLinesAsync(path)).Select(ParseEquation).ToList();

        var validEquations = input.Where(i => ValidateEquation(i.result, i.elements, 0, false));
        
        Console.WriteLine($"Valid equation result sum: {validEquations.Select(ve => ve.result).Sum()}");
        
        var validWithConcat = input.Where(i => ValidateEquation(i.result, i.elements, 0, true));
        
        Console.WriteLine($"Valid with concat equation result sum: {validWithConcat.Select(ve => ve.result).Sum()}");
    }

    private bool ValidateEquation(long expectedResult, List<long> elements, long currentResult, bool enableConcat)
    {
        if (elements.Count == 0)
            return expectedResult == currentResult;

        var add = ValidateEquation(expectedResult, elements.Skip(1).ToList(), currentResult + elements[0], enableConcat);
        var mult = ValidateEquation(
            expectedResult,
            elements.Skip(1).ToList(),
            currentResult != 0 ? currentResult * elements[0] : elements[0],
            enableConcat
        );
        
        if(!enableConcat)
            return currentResult <= expectedResult && (add || mult);
            
        var concat = ValidateEquation(
            expectedResult,
            elements.Skip(1).ToList(),
            long.Parse(string.Concat(currentResult, elements[0])),
            enableConcat);
        
        return currentResult <= expectedResult && (add || mult || concat);
    }       
    
    private (long result, List<long> elements) ParseEquation(string line)
    {
        var split = line.Split(":");

        var res = long.Parse(split[0]);
        var elem = split[1].Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(long.Parse).ToList();

        return (res, elem);
    }
}
