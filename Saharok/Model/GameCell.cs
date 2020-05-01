using System.Drawing;

namespace Saharok.Model
{
    public enum CellType
    {
        Wall,
        Lava,
    }

    public class GameCell
    {
        public readonly Rectangle Position;
        public readonly CellType Type;
        public GameCell(CellType type, Rectangle position)
        {
            Type = type;
            Position = position;
        }
    }
}