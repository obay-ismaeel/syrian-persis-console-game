using System.Numerics;
using System.Text;

namespace PersisTheGame;

class Board
{
    public List<Pawn> UserPawns = new();
    public List<Pawn> ComputerPawns = new();
    
    public List<Cell> userPath = new List<Cell>();
    public List<Cell> computerPath = new List<Cell>();
    
    public readonly List<int> ProtectedCellsIndexes = new() { 9, 22, 26, 39, 43, 56, 60, 73 };

    public Board(Board board, PawnMovement move)
    {
        UserPawns = board.UserPawns;
        ComputerPawns = board.ComputerPawns;
        userPath = board.userPath;
        computerPath = board.computerPath;
        MovePawn(move.Pawn, move.Shift);
    }

    public Board()
    {
        //intializing the users pawns
        for(int i=1; i<=4; i++)
        {
            UserPawns.Add(new Pawn(Player.USER,$"U{i}"));
            ComputerPawns.Add(new Pawn(Player.COMPUTER, $"C{i}"));
        }

        //intializing the user path 
        for (int i=0; i<7; i++)
            userPath.Add(new Cell(false, CellType.USER_KITCHEN));
        
        for(int i=7; i<75; i++)
        {
            if (ProtectedCellsIndexes.Contains(i))
                userPath.Add(new Cell(true, CellType.SHARED));
            else
                userPath.Add(new Cell());
        }

        for (int i = 7; i >= 0; i--)
            userPath.Add(userPath[i]);

        userPath.Add(new Cell( false, CellType.USER_KITCHEN ));

        //intializing the computer path 
        for (int i = 0; i < 7; i++)
            computerPath.Add(new Cell(false, CellType.COMPUTER_KITCHEN));

        for (int i = 41; i < 75; i++)
            computerPath.Add(userPath[i]);

        for (int i = 7; i <= 41; i++)
            computerPath.Add(userPath[i]);

        for (int i = 6; i >= 0; i--)
            computerPath.Add(computerPath[i]);
        
        computerPath.Add(new Cell( false, CellType.COMPUTER_KITCHEN ));
    }

    public List<PawnMovement> GetPossibleMoves(Player player, List<int> shifts)
    {
        List<PawnMovement> moves = new List<PawnMovement>();

        foreach(var shift in shifts)
        {
            var pawns = GetPlayerPawns(player);
            for (int i = 0;i < pawns.Count; i++)
            {
                if (NextPosition(pawns[i], shift) != -1)
                {
                    moves.Add(new PawnMovement(pawns[i], shift));
                }
            }
        }

        var uniqueMoves= moves
        .GroupBy(m => new { m.Pawn.Position, m.Shift })
        .Select(group => group.First())
        .ToList();

        return uniqueMoves;
    }

    private List<Pawn> GetPlayerPawns(Player player) => player is Player.USER ? UserPawns : ComputerPawns;

    public void MovePawn(Pawn pawn, int shift)
    {
        var newPosition = NextPosition(pawn, shift);

        if (newPosition is -1) throw new InvalidOperationException();
        
        var oldPosition = pawn.Position;
        
        pawn.Position = newPosition;
        
        if(pawn.Player is Player.USER)
        {
            if(oldPosition is not -1) userPath[oldPosition].Remove(pawn);

            if (userPath[newPosition].BelongsTo(Player.COMPUTER))
            {
                var removedPawns = userPath[newPosition].RemoveAll();
                UpdateIndexes(removedPawns, -1);
            }

            userPath[newPosition].Place(pawn);
            UpdateIndexes(pawn.Name, newPosition);
        }
        else
        {
            if (oldPosition is not -1) computerPath[oldPosition].Remove(pawn);

            if (computerPath[newPosition].BelongsTo(Player.USER))
            {
                var removedPawns = computerPath[newPosition].RemoveAll();
                UpdateIndexes(removedPawns, -1);
            }

            computerPath[newPosition].Place(pawn);
            UpdateIndexes(pawn.Name, newPosition);
        }
    }

    public void UpdateIndexes(List<string> pawns, int newIndex)
    {
        for (int i = 0; i < UserPawns.Count; i++)
        {
            if (pawns.Contains(UserPawns[i].Name)) UserPawns[i].Position = newIndex;
            if (pawns.Contains(ComputerPawns[i].Name)) ComputerPawns[i].Position = newIndex;
        }
    }

    public void UpdateIndexes(string pawn, int newIndex)
    {
        for (int i = 0; i < UserPawns.Count; i++)
        {
            if (pawn == UserPawns[i].Name) UserPawns[i].Position = newIndex;
            if (pawn == ComputerPawns[i].Name) ComputerPawns[i].Position = newIndex;
        }
    }

    public int NextPosition(Pawn pawn, int shift)
    {
        if (pawn.Position is -1)
        {
            return shift is 1 ? 0 : -1;
        }

        var nextPosition = pawn.Position + shift;
        
        if (nextPosition > 83) return -1;

        if (pawn.Player is Player.USER)
        {
            if (userPath[nextPosition].IsProtected && userPath[nextPosition].BelongsTo(Player.COMPUTER))
                return -1;
        }
        else
        {
            if (userPath[nextPosition].IsProtected && userPath[nextPosition].BelongsTo(Player.USER))
                return -1;
        }

        return nextPosition;
    }

    public int OutPawns(Player player)
    {
        int count = 0;
        foreach(var pawn in PlayerPawns(player))
        {
            if(pawn.Position is -1) count++;
        }
        return count;
    }

    public int FinishedPawns(Player player)
    {
        int count = 0;
        foreach (var pawn in PlayerPawns(player))
        {
            if (pawn.Position is 83) count++;
        }
        return count;
    }

    public List<Pawn> PlayerPawns(Player player) => player is Player.USER ? UserPawns : ComputerPawns;

    public bool IsTerminal()
    {
        if (userPath[83].Count() is 4) return true;
        if (computerPath[83].Count() is 4) return true;
        return false;
    }

    //MOVE TO A SEPARATE CLASS
    public int Evaluate()
    {
        var MaxInPawns = InPawns(Player.COMPUTER) * 10;
        var MinInPawns = InPawns(Player.USER) * -10;
        var MaxProtectedPawns = ProtectedPawns(Player.COMPUTER) * 5;
        var MinProtectedPawns = ProtectedPawns(Player.USER) * -5;
        var MaxFinalKitchenPawns = FinalKitchenPawns(Player.COMPUTER) * 5;
        var MinFinalKitchenPawns = FinalKitchenPawns(Player.USER) * -5;
        
        var value = MaxInPawns + MinInPawns + MaxProtectedPawns 
            + MinFinalKitchenPawns + MaxFinalKitchenPawns + MinFinalKitchenPawns;
        
        return value;
    }

    public int FinalKitchenPawns(Player player)
    {
        int count = 0;
        if(player is Player.USER)
        {
            for(int i = 76; i < 84; i++)
                count += userPath[i].Count();
        }
        else
        {
            for (int i = 76; i < 84; i++)
                    count += computerPath[i].Count();
        }
        return count;
    }

    public int ProtectedPawns(Player player) 
    {
        int count = 0;
        foreach(int index in ProtectedCellsIndexes)
        {
           if(userPath[index].BelongsTo(player)) count += userPath[index].Count();
        }
        return count;
    }

    public int InPawns(Player player) => 4 - OutPawns(player);

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"\t\t\t|{userPath[74]}|{userPath[7]}|{userPath[8]}|\n");
        sb.Append($"\t\t\t[{userPath[73]}]{userPath[6]}[{userPath[9]}]\n");
        sb.Append($"\t\t\t|{userPath[72]}|{userPath[5]}|{userPath[10]}|\n");
        sb.Append($"\t\t\t|{userPath[71]}|{userPath[4]}|{userPath[11]}|\n");
        sb.Append($"\t\t\t|{userPath[70]}|{userPath[3]}|{userPath[12]}|\n");
        sb.Append($"\t\t\t|{userPath[69]}|{userPath[2]}|{userPath[13]}|\n");
        sb.Append($"\t\t\t|{userPath[68]}|{userPath[1]}|{userPath[14]}|\n");
        sb.Append($"\t\t\t|{userPath[67]}|{userPath[0]}|{userPath[15]}|\n");
        
        sb.Append($"|{userPath[59]}[{userPath[60]}]{userPath[61]}|{userPath[62]}|{userPath[63]}|{userPath[64]}|{userPath[65]}|{userPath[66]}|\t " +
            $"|{userPath[16]}|{userPath[17]}|{userPath[18]}|{userPath[19]}|{userPath[20]}|{userPath[21]}[{userPath[22]}]{userPath[23]}|\n");

        sb.Append($"|{userPath[58]}|  |  |  |  |  |  |  |\t " +
            $"|  |  |  |  |  |  |  |{userPath[24]}|\n");

        sb.Append($"|{userPath[57]}[{userPath[56]}]{userPath[55]}|{userPath[54]}|{userPath[53]}|{userPath[52]}|{userPath[51]}|{userPath[50]}|\t " +
            $"|{userPath[32]}|{userPath[31]}|{userPath[30]}|{userPath[29]}|{userPath[28]}|{userPath[27]}[{userPath[26]}]{userPath[25]}|\n");

        sb.Append($"\t\t\t|{userPath[49]}|{computerPath[0]}|{userPath[33]}|\n");
        sb.Append($"\t\t\t|{userPath[48]}|{computerPath[1]}|{userPath[34]}|\n");
        sb.Append($"\t\t\t|{userPath[47]}|{computerPath[2]}|{userPath[35]}|\n");
        sb.Append($"\t\t\t|{userPath[46]}|{computerPath[3]}|{userPath[36]}|\n");
        sb.Append($"\t\t\t|{userPath[45]}|{computerPath[4]}|{userPath[37]}|\n");
        sb.Append($"\t\t\t|{userPath[44]}|{computerPath[5]}|{userPath[38]}|\n");
        sb.Append($"\t\t\t[{userPath[43]}]{computerPath[6]}[{userPath[39]}]");
        sb.Append($"\tOUT [USER:{OutPawns(Player.USER)} COM:{OutPawns(Player.COMPUTER)}]\n");
        sb.Append($"\t\t\t|{userPath[42]}|{userPath[41]}|{userPath[40]}|");
        sb.Append($"\tFINISHED [USER:{FinishedPawns(Player.USER)} COM:{FinishedPawns(Player.COMPUTER)}]\n");
        sb.Append("\n");

        return sb.ToString();
    }

}
