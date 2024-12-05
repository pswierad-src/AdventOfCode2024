namespace Day5;

internal class Processor
{
    private readonly Dictionary<int, List<int>> _ruleDictionary = new();
    private readonly List<List<int>> _updates = new();

    public async Task Run(string path)
    {
        var input = (await File.ReadAllTextAsync(path)).Split(Environment.NewLine + Environment.NewLine);

        _updates.AddRange(
            input[1].TrimEnd('\n').Split(Environment.NewLine)
                .Select(u => u.Split(',').Select(int.Parse).ToList()).ToList());

        var rules = input[0].Split(Environment.NewLine)
            .Select(r => r.Split("|").Select(int.Parse).ToArray()).ToArray();

        foreach (var rule in rules)
        {
            if (_ruleDictionary.TryGetValue(rule[0], out var ruleSet))
            {
                ruleSet.Add(rule[1]);
            }
            else
            {
                _ruleDictionary[rule[0]] = [rule[1]];
            }
        }

        RunTaskOne();
        RunTaskTwo();

    }

    public void RunTaskOne()
    {
        var valid = _updates.Where(UpdateValid).Select(x => x[x.Count / 2]).Sum();

        Console.WriteLine($"Running task 1. Sum of middle elements: {valid}");
    }

    public void RunTaskTwo()
    {
        var invalid = _updates.Where(x => !UpdateValid(x)).ToList();

        var sortedInvalids = new List<List<int>>();

        foreach (var invalidToSort in invalid.Select(update => new List<int>(update)))
        {
            for (var i = 0; i < invalidToSort.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (_ruleDictionary.TryGetValue(invalidToSort[i], out var ruleSet) && ruleSet.Contains(invalidToSort[j]))
                    {
                        (invalidToSort[i], invalidToSort[j]) = (invalidToSort[j], invalidToSort[i]);
                    }
                }
            }

            sortedInvalids.Add(invalidToSort);
        }

        var sum = sortedInvalids.Select(si => si[si.Count / 2]).Sum();

        Console.WriteLine($"Running task 2. Sum of middle elements: {sum}");
    }

    private bool UpdateValid(List<int> update)
    {
        var updateRev = new List<int>(update);
        updateRev.Reverse();

        foreach (var page in updateRev)
        {
            _ruleDictionary.TryGetValue(page, out var ruleSet);
            ruleSet ??= [];

            if (updateRev[updateRev.IndexOf(page)..].Exists(item => ruleSet.Contains(item)))
                return false;
        }

        return true;
    }
}
