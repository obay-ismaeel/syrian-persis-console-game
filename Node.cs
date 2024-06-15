using PersisTheGame.Enums;
using PersisTheGame.Structure;

namespace PersisTheGame;

internal class Node : Board
{
    public Node? Parent;
    public PawnMovement? Move;
    public List<Node> Children = new();
    public List<int> Shifts = new();
    public Player PlayerTurn = Player.COMPUTER;

    public Node(Board board): base(board) { }

    public Node(Board board, PawnMovement move) : base (board, move)
    {

    } 

    public override string ToString()
    {
        return base.ToString();
    }
}
