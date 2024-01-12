namespace PersisTheGame;

internal class Node : Board
{
    public Node? Parent;
    public PawnMovement? Move;
    public double Possibilty;
    public List<Node> Children = new();
    public bool IsChance = false;
    public List<int> Shifts = new();
    //public bool PlayAgain = false;
    //public bool DidPlayAgain = false;
    public Player PlayerTurn = Player.COMPUTER;

    public Node(Board board, PawnMovement move) : base (board, move)
    {

    } 

    public Node(Board current, List<int> shifts) : base(current)
    {
        Shifts = shifts;
    }

    public Node(Node parent, PawnMovement move, List<int> shifts) : base (parent, move)
    {
        Move = move;
        Parent = parent;
        Shifts = shifts;
        if (Shifts.Count is 0)
        {
            IsChance = true;
            PlayerTurn = ( PlayerTurn is Player.COMPUTER ? Player.USER : Player.COMPUTER );
        }
    }

    public Node(Node parent, double possibility, List<int> shifts) : base(parent)
    {
        Possibilty = possibility;
        Parent = parent;
        Shifts = shifts;
        if (Shifts.Count is 0 || GetPossibleMoves(PlayerTurn, shifts).Count is 0)
        {
            IsChance = true;
            PlayerTurn = (PlayerTurn is Player.COMPUTER ? Player.USER : Player.COMPUTER);
        }
    }

    public void GetChildren()
    {
        if(Children.Count > 0)
        {
            return;
        } 
        else if (IsChance)
        {
            for (int i = 0; i < 7; i++) 
            {
                var possiblity = Dice.GetTossPossibility(i);
                var shifts = Dice.GetShifts(i);
                Children.Add(new Node(this, possiblity, shifts));
            }
        }      
        else
        {
            var moves = GetPossibleMoves(PlayerTurn, Shifts);
            foreach (var move in moves)
            {
                var newShifts = new List<int>(Shifts);
                newShifts.Remove(move.Shift);
                Children.Add(new Node(this, move, newShifts));    
            }
            if(!moves.Any())
            {
                Children.Add(new Node(this, new PawnMovement(userPawns[0],0), new List<int>()));
            }
        }
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
