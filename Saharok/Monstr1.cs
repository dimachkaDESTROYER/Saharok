using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    public class Monstr
    {
        public Rectangle Position { get; private set; }
        
        public Monstr(Rectangle position)
        {
            
            Position = position;
        }

        public void ChangePositionToRight()
        {
            Position = new Rectangle(new Point(Position.X + 5, Position.Y), Position.Size);
        }

        public void ChangePositionToLeft()
        {
            Position = new Rectangle(new Point(Position.X - 5, Position.Y), Position.Size);
        }

    }
}
