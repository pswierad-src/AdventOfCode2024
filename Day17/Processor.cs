
using System.Diagnostics;
using System.Numerics;

namespace Day17;

public class Processor
{
    private string _path;
    private long _regA, _regB, _regC;
    private List<int> _operations = new();
    
    public async Task Run(string path)
    {
        _path = path;
        await SetRegAndOper(_path);
        Console.WriteLine(string.Join(',', RunOperations(_operations.ToArray(), _regA, _regB, _regC)));
        Console.WriteLine();
        Console.WriteLine($"{FindStartingProgram(0, 1)}");
    }
    
    private long? FindStartingProgram(long potentialA, int iteration)
    {
        SetRegAndOper(_path).Wait();
        
        if (_operations.Count - iteration < 0)
        {
            return potentialA;
        }

        for (var i = 0; i < 8; i++)
        {
            var result = RunOperations(
                _operations.ToArray(),
                potentialA * 8 + i,
                _regB,
                _regC);

            if (result[0] != _operations[^iteration]) 
                continue;
            
            var copy = FindStartingProgram(potentialA * 8 + i, iteration + 1);
            
            if(copy == null)
                continue;
            
            return copy;
        }

        return null;
    }

    private List<int> RunOperations(int[] program, long a, long b, long c)
    {
        var regs = new [] { a, b, c };

        var buffer = new List<int>();
        
        for (var i = 0; i < program.Length;)
        {
            var operation = (Operations)program[i];
            var operand = program[i + 1];

            long numerator = 0, denominator = 0;
            
            switch (operation)
            {
                case Operations.Adv:
                    numerator = regs[0];
                    denominator = (long)Math.Pow(2, GetOperand(operand));
                    regs[0] = numerator / denominator;
                    i += 2;
                    break;
                case Operations.Bxl:
                    regs[1] = regs[1] ^ operand;
                    i += 2;
                    break;
                case Operations.Bst:
                    regs[1] = GetOperand(operand) % 8;
                    i += 2;
                    break;
                case Operations.Jnz:
                    if (regs[0] == 0)
                    {
                        i += 2;
                        break;
                    }
                    i = operand;
                    break;
                case Operations.Bxc:
                    regs[1] = regs[1] ^ regs[2];
                    i += 2;
                    break;
                case Operations.Out:
                    buffer.Add((int)(GetOperand(operand) % 8));
                    i += 2;
                    break;
                case Operations.Bdv:
                    numerator = regs[0];
                    denominator = (long)Math.Pow(2, GetOperand(operand));
                    regs[1] = numerator / denominator;
                    i += 2;
                    break;
                case Operations.Cdv:
                    numerator = regs[0];
                    denominator = (long)Math.Pow(2, GetOperand(operand));
                    regs[2] = numerator / denominator;
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
                4 => regs[0],
                5 => regs[1],
                6 => regs[2],
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