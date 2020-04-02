using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    public enum CellType
    {
        Wall,
        Empty,
    }
    public class GameCell
    {
        public readonly CellType Type;
        public GameCell(CellType type)
        {
            Type = type;
        }
    }
}
