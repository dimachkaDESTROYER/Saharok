using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    public enum CellType
    {
        Wall,
        Money,
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
