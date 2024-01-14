namespace PersisTheGame;

static class ExpectiMiniMax
{
    public static PawnMovement Solve(Node node, List<int> shifts)
    {
        int bestValue = int.MinValue;
        PawnMovement? bestMove = null;

        int alpha = int.MinValue;
        int beta = int.MaxValue;

        foreach (var move in node.GetPossibleMoves(Player.COMPUTER, shifts))
        {
            var child = new Node(node, move);
            List<int> newShifts = new List<int>(shifts);
            newShifts.Remove(move.Shift);
            var value = Value(child, newShifts, true, 7, alpha, beta);
            if (value >= bestValue)
            {
                bestValue = value;
                bestMove = move;
            }
        }

        return bestMove!;
    }

    public static int Value(Node node, List<int> shifts, bool maximizingPlayer, int depth, int alpha, int beta)
    {
        if (node.IsTerminal() || depth is 0)
            return node.Evaluate();

        if (shifts.Count is 0 || node.GetPossibleMoves(maximizingPlayer ? Player.COMPUTER : Player.USER, shifts).Count is 0)
            return ChanceValue(node, maximizingPlayer, depth, alpha, beta);

        if (maximizingPlayer)
            return MaxValue(node, shifts, maximizingPlayer, depth, alpha, beta);

        return MinValue(node, shifts, maximizingPlayer, depth, alpha, beta);
    }

    public static int MaxValue(Node node, List<int> shifts, bool maximizingPlayer, int depth, int alpha, int beta)
    {
        var value = int.MinValue;

        var moves = node.GetPossibleMoves(Player.COMPUTER, shifts);
        foreach (var move in moves)
        {
            var newShifts = new List<int>(shifts);
            newShifts.Remove(move.Shift);
            var childNode = new Node(node, move);
            node.Children.Add(childNode);
            value = Math.Max(value, Value(childNode, newShifts, maximizingPlayer, depth - 1, alpha, beta));

            alpha = Math.Max(alpha, value);
            if (beta <= alpha)
                break;
        }

        return value;
    }

    public static int MinValue(Node node, List<int> shifts, bool maximizingPlayer, int depth, int alpha, int beta)
    {
        var value = int.MaxValue;
        var moves = node.GetPossibleMoves(Player.USER, shifts);
        foreach (var move in moves)
        {
            var newShifts = new List<int>(shifts);
            newShifts.Remove(move.Shift);
            var childNode = new Node(node, move);
            node.Children.Add(childNode);
            value = Math.Min(value, Value(new Node(node, move), newShifts, maximizingPlayer, depth - 1, alpha, beta));

            beta = Math.Min(beta, value);
            if (beta <= alpha)
                break;
        }
        return value;
    }

    public static int ChanceValue(Node node, bool maximizingPlayer, int depth, int alpha, int beta)
    {
        double avg = 0;
        double noValidMovesNodesPossiblities = 0;
        double noValidMovesNodesValue = 0;
        bool noValidMovesChildCalculated = false;
        for (int i = 0; i < 7; i++)
        {
            var possibilty = Dice.GetTossPossibility(i);
            var shifts = Dice.GetShifts(i);
            var childNode = new Node(node, new PawnMovement(node.userPawns[0], 0));
            node.Children.Add(childNode);

            var ChildPossibleMovesCount = childNode.GetPossibleMoves(maximizingPlayer ? Player.COMPUTER : Player.USER, shifts).Count;

            if (ChildPossibleMovesCount > 0 || !noValidMovesChildCalculated)
            {
                var value = Value(childNode, shifts, !maximizingPlayer, depth - 1, alpha, beta);

                if (ChildPossibleMovesCount > 0)
                    avg += possibilty * value;

                if (!noValidMovesChildCalculated)
                {
                    noValidMovesNodesValue = value;
                    noValidMovesChildCalculated = true;
                }

                if (maximizingPlayer)
                {
                    alpha = Math.Max(alpha, value);
                    if (beta <= alpha)
                        break;
                }
                else
                {
                    beta = Math.Min(beta, value);
                    if (beta <= alpha)
                        break;
                }
            }
            if (ChildPossibleMovesCount is 0) noValidMovesNodesPossiblities += possibilty;
        }
        avg += noValidMovesNodesPossiblities * noValidMovesNodesValue;
        return (int)avg;
    }
}
