namespace PersisTheGame;

static class Dice
{
    public static List<int> GetPlayerShifts(bool isUserTurn)
    {
        List<int> rolls = isUserTurn ? CollectTossesOutput() : CollectTossesOutputComputer();
        List<int> shifts = new();
        
        foreach(int roll in rolls)
        {
            switch(roll)
            {
                case 0:
                    shifts.Add(6);
                    break;
                case 1:
                    shifts.Add(1);
                    shifts.Add(10);
                    break;
                case 2:
                    shifts.Add(2);
                    break;
                case 3:
                    shifts.Add(3);
                    break;
                case 4:
                    shifts.Add(4);
                    break;
                case 5:
                    shifts.Add(1);
                    shifts.Add(24);
                    break;
                case 6:
                    shifts.Add(12);
                    break;
            }
        }

        return shifts;
    }

    public static List<int> GetComputerShifts()
    {
        List<int> rolls = CollectTossesOutputComputer();
        List<int> shifts = new();

        foreach (int roll in rolls)
        {
            switch (roll)
            {
                case 0:
                    shifts.Add(6);
                    break;
                case 1:
                    shifts.Add(1);
                    shifts.Add(10);
                    break;
                case 2:
                    shifts.Add(2);
                    break;
                case 3:
                    shifts.Add(3);
                    break;
                case 4:
                    shifts.Add(4);
                    break;
                case 5:
                    shifts.Add(1);
                    shifts.Add(24);
                    break;
                case 6:
                    shifts.Add(12);
                    break;
            }
        }

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

    private static bool PlayAgain(int rollResult) => rollResult == 0 || rollResult == 1 || rollResult == 5 || rollResult == 6;

    private static int TossOnce()
    {
        int result = 0;
        Random random = new Random();
        result += random.Next(0, 2);
        result += random.Next(0, 2);
        result += random.Next(0, 2);
        result += random.Next(0, 2);
        result += random.Next(0, 2);
        result += random.Next(0, 2);
        return result;
    }

    public static double GetShiftPossibility(int shift)
    {
        return shift switch
        {
            0 => (double)1 / 64,
            1 => (double)6 / 64,
            2 => (double)15 / 64,
            3 => (double)20 / 64,
            4 => (double)15 / 64,
            5 => (double)6 / 64,
            6 => (double)1 / 64,
            _ => throw new InvalidOperationException("The shift you asked for doesn't exist!"),
        };
    }
}