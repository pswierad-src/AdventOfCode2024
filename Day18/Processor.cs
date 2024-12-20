namespace Day18;

public class Processor
{
    private List<(int x, int y)> _input;
    public async Task Run(string path, int size, int simulateTo)
    {
        _input = (await File.ReadAllLinesAsync(path))
            .Select(l => l.Split(',').Select(int.Parse).ToList()).Select(l => (l[0], l[^1])).ToList();

        PartOne(size, simulateTo);
        PartTwo(size, simulateTo);

    }
    public void PartOne(int size, int simulateTo)
    {
        var memory = new bool[size + 1, size + 1];

        var start = new Node(0, 0);
        var end = new Node(size, size);

        foreach (var step in _input[..simulateTo])
        {
            memory[step.x, step.y] = true;
        }

        var pathfinder = new AStar(size+1, size+1, memory);
        var currentPath = pathfinder.FindPath(start, end);

        Console.WriteLine($"Min steps: {currentPath.Count-1}");
    }

    private void PartTwo(int size, int simulateFirst)
    {
        var memory = new bool[size + 1, size + 1];

        var start = new Node(0, 0);
        var end = new Node(size, size);

        foreach (var presim in _input[..simulateFirst])
        {
            memory[presim.x, presim.y] = true;
        }

        foreach (var step in _input[(simulateFirst+1)..])
        {
            memory[step.x, step.y] = true;

            var pathfinder = new AStar(size+1, size+1, memory);
            var currentPath = pathfinder.FindPath(start, end);

            if (currentPath != null)
                continue;

            Console.WriteLine($"First failing: {step.x},{step.y}");
            break;
        }
    }

}
