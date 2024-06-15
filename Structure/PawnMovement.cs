namespace PersisTheGame.Structure;

class PawnMovement
{

    public Pawn Pawn { get; set; }
    public int Shift { get; set; }
    public PawnMovement(Pawn pawn, int shift)
    {
        Pawn = pawn;
        Shift = shift;
    }

}
