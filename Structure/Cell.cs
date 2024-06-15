using System.Runtime.InteropServices;
using System.Text;
using PersisTheGame.Enums;

namespace PersisTheGame.Structure;

class Cell
{
    public List<Pawn> Content = new();
    public bool IsProtected = false;
    public CellType Type = CellType.SHARED;

    public Cell()
    {

    }

    public Cell(Cell cell)
    {
        cell.Content.ForEach(pawn =>
        {
            Content.Add(new Pawn(pawn));
        });
        IsProtected = cell.IsProtected;
        Type = cell.Type;
    }
    public Cell(bool isProtected, CellType type)
    {
        IsProtected = isProtected;
        Type = type;
    }

    public void Place(Pawn pawn)
    {
        if (BelongsTo(pawn.Player))
        {
            Content.Add(pawn);
            return;
        }

        Content.ForEach(item =>
        {
            item.Position = -1;
        });

        Content.Clear();

        Content.Add(pawn);
    }

    public void Remove(Pawn pawn)
    {
        if (!Content.Contains(pawn)) return;

        Content.Remove(pawn);
        pawn.Position = -1;
    }

    public List<string> RemoveAll()
    {
        List<string> list = new();

        Content.ForEach(item =>
        {
            item.Position = -1;
            list.Add(item.Name);
        });
        Content.Clear();

        return list;
    }

    public bool BelongsTo(Player player)
    {
        if (!Content.Any()) return false;
        return Content.First().Player == player;
    }

    public int Count() => Content.Count;

    public bool IsEmpty() => !Content.Any();

    public override string ToString()
    {
        if (!Content.Any()) return "  ";
        var sb = new StringBuilder();
        var count = 0;
        foreach (var item in Content)
        {
            if (count is not 0) sb.Append(",");
            if (count is 0) sb.Append(item.ToString());
            else sb.Append(item.ToString()[1]);
            count++;
        }
        return sb.ToString();
    }
}
