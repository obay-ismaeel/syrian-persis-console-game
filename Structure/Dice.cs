namespace PersisTheGame.Structure;

static class Dice
{
    public static List<int> GetPlayerShifts(bool isUserTurn)
    {
        List<int> rolls = isUserTurn ? CollectTossesOutput() : CollectTossesOutputComputer();
        List<int> shifts = new();

        rolls.ForEach(roll =>
        {
            switch (roll)
            {
                case 0 or 6:
                    shifts.Add(roll + 6);
                    break;
                case 1:
                    shifts.Add(1);
                    shifts.Add(10);
                    break;
                case 2 or 3 or 4:
                    shifts.Add(roll);
                    break;
                case 5:
                    shifts.Add(1);
                    shifts.Add(25);
                    break;
            }
        });

        return shifts;
    }

    private static List<int> CollectTossesOutput()
    {
        List<int> tossResults = new();
        bool still = true;
        while (still)
        {
            Console.Write("Press any key to toss the dice: ");
            Console.ReadKey(true);
            int result = TossOnce();

            Console.WriteLine(result);
            tossResults.Add(result);
            still = PlayAgain(result);
        }
        return tossResults;
    }

    private static List<int> CollectTossesOutputComputer()
    {
        List<int> tossResults = new();
        bool still = true;
        while (still)
        {
            Console.Write("Press any key to toss the dice: ");
            Thread.Sleep(1500);
            int result = TossOnce();

            Console.WriteLine(result);
            tossResults.Add(result);
            still = PlayAgain(result);
        }
        return tossResults;
    }

    public static bool PlayAgain(int rollResult) => rollResult == 0 || rollResult == 1 || rollResult == 5 || rollResult == 6;

    private static int TossOnce()
    {
        int result = 0;
        Random random = new();
        result += random.Next(0, 10) < 4 ? 1 : 0;
        result += random.Next(0, 10) < 4 ? 1 : 0;
        result += random.Next(0, 10) < 4 ? 1 : 0;
        result += random.Next(0, 10) < 4 ? 1 : 0;
        result += random.Next(0, 10) < 4 ? 1 : 0;
        result += random.Next(0, 10) < 4 ? 1 : 0;
        return result;
    }

    public static double GetTossPossibility(int toss)
    {
        return toss switch
        {
            0 => (double)Math.Pow(0.6, 6),
            1 => (double)Math.Pow(0.6, 5) * 2.4,
            2 => (double)Math.Pow(0.4, 2) * Math.Pow(0.6, 4) * 15,
            3 => (double)Math.Pow(0.4, 3) * Math.Pow(0.6, 3) * 20,
            4 => (double)Math.Pow(0.4, 4) * Math.Pow(0.6, 2) * 15,
            5 => (double)Math.Pow(0.4, 5) * 3.6,
            6 => (double)Math.Pow(0.4, 6),
            _ => throw new InvalidOperationException("The shift you asked for doesn't exist!"),
        };
    }

    public static List<int> GetShifts(int toss)
    {
        return toss switch
        {
            0 => new List<int> { 6 },
            1 => new List<int> { 1, 10 },
            2 => new List<int> { 2 },
            3 => new List<int> { 3 },
            4 => new List<int> { 4 },
            5 => new List<int> { 1, 25 },
            6 => new List<int> { 12 },
            _ => throw new InvalidOperationException("The toss you entered doesn't exist!")
        };
    }
}