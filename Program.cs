public class Rarity
{
    public string Name { get; set; }
    public List<double> Chances { get; set; }
    public int Counter { get; set; }
};

public class Result
{
    private string Name { get; set; }
    private int Sum { get; set; }
    private double AvgChance { get; set; }
    private int MaxTreasure { get; set; }
    private int MaxAmount { get; set; }

    public Result(string name, int sum, double avgChance, int maxTresure, int maxAmount)
    {
        Name = name;
        Sum = sum;
        AvgChance = avgChance;
        MaxTreasure = maxTresure;
        MaxAmount = maxAmount;
    }

    public void Print()
    {
        Console.WriteLine
            (
                $"\n{this.Name}" +
                $"\nобщее кол-во {this.Sum}" +
                $"\nср.шанс {this.AvgChance:p3}" +
                $"\nкейс {this.MaxTreasure}" +
                $"\nмакс. кол-во {this.MaxAmount}"
            );
    }
}

public class Program
{
    private static readonly List<Rarity> AllRarities = new()
        {
            new()
            {
                Name = "Редкая",
                Chances = new()
                    {
                        2000, 583, 187, 88, 51, 33, 23, 17, 13.1, 10.4,
                        8.5, 7.1, 6, 5.2, 4.5, 4, 3.6, 3.2, 2.9, 2.6,
                        2.4, 2.2, 2.1, 1.9, 1.8, 1.7, 1.6, 1.5, 1.5,
                        1.4, 1.3, 1.3, 1.2, 1.2, 1.2, 1.1, 1.1, 1.1, 1.1, 1,
                        1,1,1,1,1,1,1,1,1,1
                    },
                Counter = 0
            },
            new()
            {
                Name = "Очень редкая",
                Chances = new()
                    {
                        20000, 3653, 1059, 485, 276, 178, 124, 92, 70, 56,
                        45, 38, 32, 27, 24, 21, 18, 16, 14.1, 12.7,
                        11.5, 10.5, 9.6, 8.8, 8.1, 7.5, 7, 6.5, 6, 5.7,
                        5.3, 5, 4.7, 4.5, 4.2, 4, 3.8, 3.6, 3.4, 3.3,
                        3.2, 3, 2.9, 2.8, 2.7, 2.6, 2.5, 2.4, 2.3, 2.2
                    },
                Counter = 0
            },
            new()
            {
                Name = "Невероятно редкая",
                Chances = new()
                    {
                        100000, 27380, 8614, 4021, 2303, 1486, 1037, 764, 586, 464,
                        376, 311, 262, 223, 193, 168, 148, 131, 117, 105,
                        95, 86, 79, 72, 66, 61, 57, 53, 49, 46,
                        43, 40, 38, 35, 33, 32, 30, 28, 27, 26,
                        24, 23, 22, 21, 20, 19, 18, 17,17,17
                    },
                Counter = 0
            }
        };

    public static void Main()
    {
        Console.Write("Количество итераций: ");
        int itearations = int.Parse(Console.ReadLine());

        List<Result> results = GetResult(itearations);

        foreach (var result in results)
            result.Print();
    }

    public static List<Result> GetResult(int itearations)
    {
        Dictionary<string, SortedDictionary<int, int>> rawResults = new();
        Random rand = new();

        foreach (var rarity in AllRarities)
            rawResults.Add(rarity.Name, new() );

        for (int i = 0; i < itearations; i++)
            foreach (var rarity in AllRarities)
                Simulate(rarity, rand, rawResults);

        return CultivateResult(rawResults, itearations);
    }

    public static void Simulate(Rarity rarity, Random rand, Dictionary<string, SortedDictionary<int, int>> rawResults)
    {

        int chance = rarity.Counter > rarity.Chances.Count - 1 ?
            rarity.Chances.Count - 1: rarity.Counter;

        if (rand.Next(0, (int)rarity.Chances[chance] * 10) < 10)
        {
            var dict = rawResults[rarity.Name];

            if (!dict.ContainsKey(rarity.Counter))
                dict.Add(rarity.Counter, 1);
            else
                dict[rarity.Counter]++;

            rarity.Counter = 0;
        }
        else
            rarity.Counter++;
    }

    public static List<Result> CultivateResult(Dictionary<string, SortedDictionary<int, int>> rawResults, int itearations)
    {
        List<Result> results = new();

        foreach (var name in rawResults.Keys)
        {
            int maxAmount = 0;
            int maxTresure = 0;
            int sum = 0;

            foreach (var tresure in rawResults[name].Keys)
            {
                if (maxAmount < rawResults[name][tresure])
                {
                    maxAmount = rawResults[name][tresure];
                    maxTresure = tresure;
                }
                sum += rawResults[name][tresure];
            }

            double avgChance = sum / (double)itearations;
            results.Add(new(name, sum, avgChance, maxTresure, maxAmount));
        }

        return results;
    }
}