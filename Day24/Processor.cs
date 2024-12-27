namespace Day24;

public class Processor
{
    public void Run(string path)
    {
        RunPart1(path);
        RunPart2(path);
    }

    private void RunPart2(string path)
    {
        var (gates, _) = GetInput(path);
        
        var invalidGates = new HashSet<Gate>();
        var lastZGate = gates.Where(g => g.Output.StartsWith('z'))
            .OrderByDescending(g => g.Output)
            .First();
    
        foreach (var gate in gates)
        {
            var isInvalid = false;
        
            if (gate.Output.StartsWith('z') && gate.Output != lastZGate.Output)
            {
                isInvalid = gate.Operation != Operation.XOR;
            }
            else if (!gate.Output.StartsWith('z') && !IsInput(gate.A) && !IsInput(gate.B))
            {
                isInvalid = gate.Operation == Operation.XOR;
            }
            else if (IsInput(gate.A) && IsInput(gate.B) && !(gate.A.EndsWith("00") && gate.B.EndsWith("00")))
            {
                var isValidForNextGate = gates.Exists(other => 
                    other != gate && 
                    (other.A == gate.Output || other.B == gate.Output) && 
                    other.Operation == (gate.Operation == Operation.XOR ? Operation.XOR : Operation.OR));
                
                isInvalid = !isValidForNextGate;
            }
        
            if (isInvalid)
                invalidGates.Add(gate);
        }

        Console.WriteLine($"Part 2: {string.Join(",", invalidGates.Select(g => g.Output).OrderBy(w => w))}");
        return;
        
        bool IsInput(string wire) => wire.StartsWith('x') || wire.StartsWith('y');
    }

    private void RunPart1(string path)
    {
        Dictionary<string, bool> wireValues = new();
        var (gates, signals) = GetInput(path);
        
        foreach (var signal in signals)
            wireValues.Add(signal.Item1, signal.Item2);
        
        while (gates.Count != 0)
        {
            gates = RunGates(gates);
        }

        foreach (var signal in signals)
        {
            wireValues.Remove(signal.Item1);
        }

        var wires = wireValues.OrderBy(x => x.Key)
            .Select(x => (x.Key, x.Value ? 1 : 0)).ToList();

        var num = string.Join(
            "",
            wires.Where(w => w.Key.StartsWith("z")).Select(w => w.Item2.ToString()).Reverse()
        );
        
        Console.WriteLine($"Part 1: {Convert.ToInt64(num, 2)}");
        return;
        
        List<Gate> RunGates(List<Gate> gates)
        {
            var gatesToRerun = new List<Gate>();
        
            foreach (var gate in gates)
            {
                if (!wireValues.TryGetValue(gate.A, out var left) || !wireValues.TryGetValue(gate.B, out var right))
                {
                    gatesToRerun.Add(gate);
                    continue;
                }

                wireValues[gate.Output] = gate.Operation switch
                {
                    Operation.AND => left && right,
                    Operation.OR => left || right,
                    Operation.XOR => left ^ right,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return gatesToRerun;
        }
    }

    private (List<Gate> gates, List<(string, bool)> inputSignals) GetInput(string path)
    {
        var input = File.ReadAllText(path).Split(Environment.NewLine + Environment.NewLine);
        var signals = input[0].Split(Environment.NewLine).Select(l =>
        {
            var values = l.Split(": ");
            return (values[0], int.Parse(values[1]) != 0);
        }).ToList();

        var gates = input[1].Split(Environment.NewLine).Select(l =>
        {
            var values = l.Split(" -> ");
            var left = values[0].Split(" ");

            return new Gate
            {
                A = left[0],
                B = left[2],
                Operation = Enum.Parse<Operation>(left[1]),
                Output = values[1]
            };
        }).ToList();
        
        return (gates, signals);
    }
}

public class Gate
{
    public string A { get; set; }
    public string B { get; set; }
    public Operation Operation { get; set; }
    public string Output { get; set; }
}

public enum Operation
{
    AND = 0,
    OR = 1,
    XOR = 2
}