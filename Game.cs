using System.Threading.Channels;
using PersisTheGame.Enums;
using PersisTheGame.Structure;

namespace PersisTheGame;

class Game
{
    private readonly Board board = new();
    private bool isUserTurn = true;

    public void Play()
    {
        while (!board.IsTerminal())
        {
            showPlayerTurnAndCurrentBoard();

            var shifts = Dice.GetPlayerShifts(true);

            if (isUserTurn)
            {
                UserTurnLogic(shifts);
            }
            else
            {
                ComputerTurnLogic(shifts);
            }
        }
        DeclareWinner();
    }

    private void DeclareWinner()
    {
        string winnerDeclaration;

        if (board.GetWinner() is Player.USER)
            winnerDeclaration = "CONGRATS YOU ARE THE WINNER!!";
        else
            winnerDeclaration = "OOPS the computer defeated you :)";

        Console.WriteLine("================================");
        Console.Write(winnerDeclaration);
        Console.WriteLine("================================");
    }

    private void ComputerTurnLogic(List<int> shifts)
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

            var move = ExpectiMiniMax.Solve(new Node(board), shifts);

            Console.WriteLine($"pawn:{move.Pawn}, shift:{move.Shift}");

            board.MovePawn(move.Pawn, move.Shift);

            shifts.Remove(move.Shift);

            ShowBoard();
        }

        isUserTurn = true;
    }

    private void TheOtherUserTurnLogic(List<int> shifts)
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

    private void UserTurnLogic(List<int> shifts)
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

    private void ShowBoard() => Console.WriteLine(board);

    private void showPossibleMoves(List<PawnMovement> moves)
    {
        for (int i = 0; i < moves.Count; i++)
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