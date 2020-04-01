using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    public enum MovingDirection
    {
        Up,
        Down,
        Left,
        Right,
    }
    public class Player
    {
        public bool onGround;
        public Rectangle Position { get; private set; }
        public int SpeedX { get; private set; }
        public int SpeedY { get; private set; }

        public void ChangeSpeed(MovingDirection d, int AbsSpeed)
        {
            if (d == MovingDirection.Up)
                SpeedY += AbsSpeed;
            if (d == MovingDirection.Down)
                SpeedY -= AbsSpeed;
            if (d == MovingDirection.Left)
                SpeedX -= AbsSpeed;
            if (d == MovingDirection.Right)
                SpeedX += AbsSpeed;
        }

        public void ChangePositionBy(int dx, int dy)
        {
            Position.Offset(dx, dy);
        }

        public Player(Rectangle position)
        {
            Position = position;
        }
        
    }
}
