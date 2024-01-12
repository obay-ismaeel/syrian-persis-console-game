namespace PersisTheGame;

static class ExpectiMiniMax
{
    public static PawnMovement Solve(Node node, List<int> shifts)
    {
        int bestValue = int.MinValue;
        PawnMovement? bestMove = null;

        foreach(var move in node.GetPossibleMoves(Player.COMPUTER, shifts))
        {
            var child = new Node(node, move);
            List<int> newShifts = new List<int>(shifts);
            newShifts.Remove(move.Shift);
            var value = Value(child, newShifts, true, 6);
            if(value >= bestValue)
            {
                bestValue = value;
                bestMove = move;
            }
        }

        return bestMove;
    }

    public static int Value(Node node, List<int> shifts, bool isMaxPlayer, int depth)
    {
        if (node.IsTerminal() || depth is 0)
            return node.Evaluate();
        
        if ( shifts.Count is 0 || node.GetPossibleMoves(isMaxPlayer ? Player.COMPUTER : Player.USER ,shifts).Count is 0 )
            return ChanceValue(node, isMaxPlayer, depth);
        
        if (isMaxPlayer)
            return MaxValue(node, shifts, isMaxPlayer, depth);
        
        return MinValue(node, shifts, isMaxPlayer, depth);
    }

    public static int MaxValue(Node node, List<int> shifts, bool isMaxPlayer, int depth)
    {
        var value = int.MinValue;
        var moves = node.GetPossibleMoves(Player.COMPUTER, shifts);
        foreach (var move in moves)
        {
            var newShifts = new List<int>(shifts);
            newShifts.Remove(move.Shift);
            var childNode = new Node(node, move);
            node.Children.Add(childNode);
            value = Math.Max( value, Value( childNode, newShifts, isMaxPlayer, depth - 1) );
        }
        return value;
    }

    public static int MinValue(Node node, List<int> shifts, bool isMaxPlayer, int depth)
    {
        var value = int.MaxValue;
        var moves = node.GetPossibleMoves(Player.USER, shifts);
        foreach (var move in moves)
        {
            var newShifts = new List<int>(shifts);
            newShifts.Remove(move.Shift);
            var childNode = new Node(node, move);
            node.Children.Add(childNode);
            value = Math.Min(value, Value(new Node(node, move), newShifts, isMaxPlayer, depth - 1));
        }
        return value;
    }

    public static int ChanceValue(Node node, bool isMaxPlayer, int depth)
    {
        double avg = 0;
        for (int i = 0; i < 7; i++)
        {
            var possibilty = Dice.GetTossPossibility(i);
            var shifts = Dice.GetShifts(i);
            var childNode = new Node(node, new PawnMovement(node.userPawns[0], 0));
            node.Children.Add(childNode);
            var value = Value(childNode, shifts, !isMaxPlayer, depth - 1);
            avg += possibilty * value;
        }
        return (int)avg;
    }
}