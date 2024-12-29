namespace Day21;

public static class Utils
{
    public static long CalculateCost(string code, int robot, Dictionary<string, long>[] cache)
    {
        if (cache[robot].TryGetValue(code, out var c))
            return c;

        if (robot == 1) return code.Length;

        long cost = 0;
        var currentlyAt = 3;
        foreach (var t in code)
        {
            var moveTo = t switch
            {
                '^' => 4,
                'A' => 3,
                '<' => 2,
                'V' => 1,
                '>' => 0,
                _ => 5
            };

            var map = MovementMaps[currentlyAt][moveTo];
            
            var lowestCost = map.Select(possible => CalculateCost(possible, robot - 1, cache)).Prepend(long.MaxValue).Min();

            cost += lowestCost;
            currentlyAt = moveTo;
        }

        cache[robot][code] = cost;

        return cost;
    }
    
    public static string[][][] MovementMaps { get; } =
    [
        [["A"], ["<A"], ["<<A"], ["^A"], ["^<A", "<^A"]],
        [[">A"], ["A"], ["<A"], [">^A", "^>A"], ["^A"]],
        [[">>A"], [">A"], ["A"], [">>^A", ">^>A"], [">^A"]],
        [["VA"], ["V<A", "<VA"], ["<V<A", "V<<A"], ["A"], ["<A"]],
        [["V>A", ">VA"], ["VA"], ["V<A"], [">A"], ["A"]]
    ];

    public static string[][][] StepsToDigit { get; } =
    [
        // 7
        [
            ["A"], [">A"], [">>A"], 
            ["VA"], ["V>A", ">VA"], ["V>>A", ">V>A", ">>VA"], 
            ["VVA"], ["VV>A", "V>VA", ">VVA"], ["VV>>A", "V>V>A", "V>>VA", ">VV>A", ">V>VA", ">>VVA"], 
            [], ["VV>VA", "V>VVA", ">VVVA"], ["VV>V>A", "VV>>VA", "V>VV>A", "V>V>VA", "V>>VVA", ">VVV>A", ">VV>VA", ">V>VVA", ">>VVVA"] 
        ],
        // 8
        [
            ["<A"], ["A"], [">A"], 
            ["V<A", "<VA"], ["VA"], ["V>A", ">VA"], 
            ["VV<A", "V<VA", "<VVA"], ["VVA"], ["VV>A", "V>VA", ">VVA"], 
            [], ["VVVA"], ["VVV>A", "VV>VA", "V>VVA", ">VVVA"] 
        ],
        // 9
        [
            ["<<A"], ["<A"], ["A"], 
            ["V<<A", "<V<A", "<<VA"], ["V<A", "<VA"], ["VA"], 
            ["VV<<A", "V<V<A", "V<<VA", "<VV<A", "<V<VA", "<<VVA"], ["VV<A", "V<VA", "<VVA"], ["VVA"], 
            [], ["VVV<A", "VV<VA", "V<VVA", "<VVVA"], ["VVVA"] 
        ],
        // 4
        [
            ["^A"], ["^>A", ">^A"], ["^>>A", ">^>A", ">>^A"], 
            ["A"], [">A"], [">>A"], 
            ["VA"], ["V>A", ">VA"], ["V>>A", ">V>A", ">>VA"], 
            [], ["V>VA", ">VVA"], ["V>V>A", "V>>VA", ">VV>A", ">V>VA", ">>VVA"] 
        ],
        // 5
        [
            ["^<A", "<^A"], ["^A"], ["^>A", ">^A"], 
            ["<A"], ["A"], [">A"], 
            ["V<A", "<VA"], ["VA"], ["V>A", ">VA"], 
            [], ["VVA"], ["VV>A", "V>VA", ">VVA"] 
        ],
        // 6
        [
            ["^<<A", "<^<A", "<<^A"], ["^<A", "<^A"], ["^A"], 
            ["<<A"], ["<A"], ["A"],
            ["V<<A", "<V<A", "<<VA"], ["V<A", "<VA"], ["VA"], 
            [], ["VV<A", "V<VA", "<VVA"], ["VVA"] 
        ],
        // 1
        [
            ["^^A"], ["^^>A", "^>^A", ">^^A"], ["^^>>A", "^>^>A", "^>>^A", ">^^>A", ">^>^A", ">>^^A"], 
            ["^A"], ["^>A", ">^A"], ["^>>A", ">^>A", ">>^A"], 
            ["A"], [">A"], [">>A"], 
            [], [">VA"], [">V>A", ">>VA"] 
        ],
        // 2
        [
            ["^^<A", "^<^A", "<^^A"], ["^^A"], ["^^>A", "^>^A", ">^^A"], 
            ["^<A", "<^A"], ["^A"], ["^>A", ">^A"], 
            ["<A"], ["A"], [">A"],
            [], ["VA"], ["V>A", ">VA"] 
        ],
        // 3
        [
            ["^^<<A", "^<^<A", "^<<^A", "<^^<A", "<^<^A", "<<^^A"], ["^^<A", "^<^A", "<^^A"], ["^^A"], 
            ["^<<A", "<^<A", "<<^A"], ["^<A", "<^A"], ["^A"], 
            ["<<A"], ["<A"], ["A"], 
            [], ["V<A", "<VA"], ["VA"] 
        ],
        
        // Empty spot
        [[], [], [], [], [], [], [], [], [], [], [], []],

        // 0
        [
            ["^^^<A", "^^<^A", "^<^^A"], ["^^^A"], ["^^^>A", "^^>^A", "^>^^A", ">^^^A"], 
            ["^^<A", "^<^A"], ["^^A"], ["^^>A", "^>^A", ">^^A"], 
            ["^<A"], ["^A"], ["^>A", ">^A"], 
            [], ["A"], [">A"] 
        ],
        // A
        [
            ["^^^<<A", "^^<^<A", "^^<<^A", "^<^^<A", "^<^<^A", "^<<^^A", "<^^^<A", "<^^<^A", "<^<^^A"], ["^^^<A", "^^<^A", "^<^^A", "<^^^A"], ["^^^A"],
            ["^^<<A", "^<^<A", "^<<^A", "<^^<A", "<^<^A"], ["^^<A", "^<^A", "<^^A"], ["^^A"],
            ["^<<A", "<^<A"], ["^<A", "<^A"], ["^A"],
            [], ["<A"], ["A"]
        ]
    ];
}