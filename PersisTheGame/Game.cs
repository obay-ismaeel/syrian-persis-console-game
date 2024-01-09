using System.Threading.Channels;

namespace PersisTheGame;

class Game
{
    private readonly Board board = new();
    private bool isUserTurn = true;

    public void Play()
    {
        while( ! board.IsTerminal() )
        {
            showPlayerTurnAndCurrentBoard();

            var shifts = Dice.GetPlayerShifts(true);
            
            if( isUserTurn )
            {
                while (shifts.Any())
                {
                    var moves = board.GetPossibleMoves(Player.USER, shifts);
                
                    showPossibleMoves(moves);

                    if (!moves.Any())
                    {
                        Console.WriteLine("NO VALID MOVES AVAILABLE!");
                        break;
                    }

                    Console.Write("Enter the number corresponding to the desired move: ");

                    int number = getUserMoveChoiceInput(moves);

                    board.MovePawn(moves[number].Pawn, moves[number].Shift);

                    shifts.Remove(moves[number].Shift);

                    ShowBoard();
                }

                isUserTurn = false;
            }
            else
            {
                while (shifts.Any())
                {
                    var moves = board.GetPossibleMoves(Player.COMPUTER, shifts);

                    showPossibleMoves(moves);

                    if (!moves.Any())
                    {
                        Console.WriteLine("NO VALID MOVES AVAILABLE!");
                        break;
                    }

                    Console.Write("Enter the number corresponding to the desired move: ");

                    int number = getUserMoveChoiceInput(moves);

                    board.MovePawn(moves[number].Pawn, moves[number].Shift);

                    shifts.Remove(moves[number].Shift);

                    ShowBoard();
                }

                isUserTurn = true;
            }
        }
    }

    private void ShowBoard() => Console.WriteLine(board);

    private void showPossibleMoves(List<PawnMovement> moves)
    {
        for (int i=0; i< moves.Count; i++)
        {
            Console.WriteLine($"[{i+1}] Move pawn {moves[i].Pawn} by {moves[i].Shift} steps");
        }
    }

    private void showPlayerTurnAndCurrentBoard()
    {
        Console.WriteLine("=================================================");
        if (isUserTurn) Console.WriteLine($"YOUR TURN");
        else Console.WriteLine($"COMPUTER TURN");
        Console.WriteLine("=================================================");
        Console.WriteLine(board + "\n");
    }

    private int getUserMoveChoiceInput(List<PawnMovement> moves)
    {
        int number = int.MaxValue;
        while (number > moves.Count || number < 1)
        {
            int.TryParse(Console.ReadLine(), out number);
            if (number > moves.Count || number < 1)
            {
                Console.WriteLine("INVALID INPUT!");
                Console.Write("Enter the number corresponding to the desired move: ");
            } 
        }
        return number-1;
    }
   

}
