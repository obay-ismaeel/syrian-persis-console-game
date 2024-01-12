﻿namespace PersisTheGame;

static class ExpectiMiniMax
{
    public static int Value(Node node, List<int> shifts, bool isMaxPlayer, int depth)
    {
        if (node.IsTerminal() || depth is 0)
            return node.Evaluate();
        else if ( shifts.Count is 0 || node.GetPossibleMoves(isMaxPlayer ? Player.COMPUTER : Player.USER ,shifts).Count is 0 )
            return ChanceValue(node, isMaxPlayer, depth);
        else if (isMaxPlayer)
            return MaxValue(node, shifts, isMaxPlayer, depth);
        else
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