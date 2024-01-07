using System.Threading.Channels;

namespace PersisTheGame;

class Game
{
    private readonly Board board = new Board();
    private bool isUserTurn = true;

    public void Play()
    {
        while( ! board.IsTerminal() )
        {
            ShowPlayerTurnAndCurrentBoard();

            var shifts = Dice.GetPlayerShifts(isUserTurn);
            
            if( isUserTurn )
            {
                while (shifts.Any())
                {
                    var moves = board.GetPossibleMoves(Player.USER, shifts);
                
                    ShowPossibleMoves(moves);

                    if (!moves.Any())
                    {
                        Console.WriteLine("NO VALID MOVES AVAILABLE!");
                        break;
                    }

                    Console.Write("Enter the number corresponding to the desired move: ");

                    int number = getUserShiftChoiceInput(moves);

                    board.MovePawn(moves[number].Pawn, moves[number].Shift);

                    shifts.Remove(moves[number].Shift);

                    ShowBoard();
                }

                isUserTurn = false;
            }
            else
            {
                var moves = board.GetPossibleMoves(Player.COMPUTER, shifts);

                ShowPossibleMoves(moves);

                isUserTurn = true;
            }
        }
    }

    private void ShowBoard() => Console.WriteLine(board);

    private void ShowPossibleMoves(List<PawnMovement> moves)
    {
        for (int i=0; i<moves.Count; i++)
        {
            Console.WriteLine($"[{i}] Move pawn {moves[i].Pawn} by {moves[i].Shift} steps");
        }
    }

    private void ShowPlayerTurnAndCurrentBoard()
    {
        Console.WriteLine("=================================================");
        if (isUserTurn) Console.WriteLine($"YOUR TURN");
        else Console.WriteLine($"COMPUTER TURN");
        Console.WriteLine("=================================================");
        Console.WriteLine(board + "\n");
    }

    private int getUserShiftChoiceInput(List<PawnMovement> moves)
    {
        int number = int.MaxValue;
        while (number >= moves.Count || number < 0)
        {
            Int32.TryParse(Console.ReadLine(), out number);
            if (number >= moves.Count || number < 0)
            {
                Console.WriteLine("INVALID INPUT!");
                Console.Write("Enter the number corresponding to the desired move: ");
            } 
        }
        return number;
    }
   

}
