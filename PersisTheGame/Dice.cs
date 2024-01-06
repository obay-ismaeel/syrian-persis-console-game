namespace PersisTheGame;

static class Dice
{
    public static List<int> GetPlayerShifts()
    {
        List<int> rolls = CollectTossesOutput();
        List<int> moves = new();
        
        foreach(int roll in rolls)
        {
            switch(roll)
            {
                case 0:
                    moves.Add(6);
                    break;
                case 1:
                    moves.Add(1);
                    moves.Add(10);
                    break;
                case 2:
                    moves.Add(2);
                    break;
                case 3:
                    moves.Add(3);
                    break;
                case 4:
                    moves.Add(4);
                    break;
                case 5:
                    moves.Add(1);
                    moves.Add(24);
                    break;
                case 6:
                    moves.Add(12);
                    break;
            }
        }

        return moves;
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