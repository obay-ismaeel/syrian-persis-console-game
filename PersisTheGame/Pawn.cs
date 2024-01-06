namespace PersisTheGame;

class Pawn
{
    public string Name;
    public Player Player;
    public int Position = -1;

    public Pawn(Player player, string name)
    {
        this.Name = name;
        this.Player = player;
    }

    public int StartIndex() => this.Player == Player.USER ? 0 : 42;

    public override string ToString()
    {
        return Name;
    }
}
