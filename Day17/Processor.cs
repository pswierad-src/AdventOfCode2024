
namespace Day17;

public class Processor
{
    private long _regA, _regB, _regC;
    private List<int> _operations = new();
    
    public async Task Run(string path)
    {
        await SetRegAndOper(path);
        //FindSelfCopy2(_operations.ToArray());
        // Console.WriteLine(string.Join(',', RunOperations()));
        // Console.WriteLine();
        //Console.WriteLine($"Registry A value: {FindSelfCopy()}");
  
        var y = FindSelfCopy();
    }

    
    private long FindSelfCopy()
    {
        long potentialA;
        
        for (potentialA = 2; potentialA < (long)Math.Pow(8, _operations.Count); potentialA++)
        {
            _regA = potentialA;
            var result = RunOperations();

            Console.Clear();
            Console.WriteLine(string.Join(',', _operations));
            Console.WriteLine(string.Join(',', result));

            if (_operations[^result.Count..].Select((op, i) => result[i] == op).Any(d => d == false)) 
                continue;

            if (_operations.Count == result.Count) 
                break;
            
            potentialA = (potentialA * 8) - 1;
        }

        return potentialA;
    }

    private List<int> RunOperations()
    {
        var localA = _regA;
        var localB = _regB;
        var localC = _regC;

        var buffer = new List<int>();
        
        for (var i = 0; i < _operations.Count;)
        {
            var operation = (Operations)_operations[i];
            var operand = _operations[i + 1];

            long numerator = 0, denominator = 0;
            
            switch (operation)
            {
                case Operations.Adv:
                    numerator = localA;
                    denominator = (long)Math.Pow(2, GetOperand(operand));
                    localA = numerator / denominator;
                    i += 2;
                    break;
                case Operations.Bxl:
                    localB = localB ^ operand;
                    i += 2;
                    break;
                case Operations.Bst:
                    localB = GetOperand(operand) % 8;
                    i += 2;
                    break;
                case Operations.Jnz:
                    if (localA == 0)
                    {
                        i += 2;
                        break;
                    }
                    i = operand;
                    break;
                case Operations.Bxc:
                    localB = localB ^ localC;
                    i += 2;
                    break;
                case Operations.Out:
                    buffer.Add((int)(GetOperand(operand) % 8));
                    i += 2;
                    break;
                case Operations.Bdv:
                    numerator = localA;
                    denominator = (long)Math.Pow(2, GetOperand(operand));
                    localB = numerator / denominator;
                    i += 2;
                    break;
                case Operations.Cdv:
                    numerator = localA;
                    denominator = (long)Math.Pow(2, GetOperand(operand));
                    localC = numerator / denominator;
                    i += 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operand), operand, null);
            }
        }

        return buffer;
        
        long GetOperand(int operand) =>
            operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => localA,
                5 => localB,
                6 => localC,
                7 => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    private async Task SetRegAndOper(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);
        
        _regA = int.Parse(lines[0].Split(':')[1].TrimStart(' '));
        _regB = int.Parse(lines[1].Split(':')[1].TrimStart(' '));
        _regC = int.Parse(lines[2].Split(':')[1].TrimStart(' '));
        _operations = lines[^1].Split(':')[1].TrimStart(' ').Split(',').Select(int.Parse).ToList();
    }
}

public enum Operations
{
    Adv = 0, // A/B^2, trunc to int, write to A
    Bxl = 1, // bitwise XOR of B and literal
    Bst = 2, // literal % 8 -> keep lowerst 3 bits -> write to B
    Jnz = 3, // A==0 -> nothing, other jump value of operand
    Bxc = 4, // bitwise XOR of B and C -> store in B !!READS OPERAND BUT IGNORES!!
    Out = 5, // literal % 8 -> output
    Bdv = 6, // A/B^2, trunc to int write to B
    Cdv = 7  // A/B^2, trunc to int write to C
}