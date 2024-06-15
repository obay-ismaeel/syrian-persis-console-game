using PersisTheGame.Enums;

namespace PersisTheGame.Structure;

class Pawn
{
    public string Name;
    public Player Player;
    public int Position = -1;

    public Pawn(Player player, string name)
    {
        Name = name;
        Player = player;
    }

    public Pawn(Pawn pawn)
    {
        Name = pawn.Name;
        Player = pawn.Player;
        Position = pawn.Position;
    }

    public override string ToString()
    {
        return Name;
    }
}
