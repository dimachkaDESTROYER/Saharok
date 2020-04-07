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

    public enum Axis
    {
        Vertical,
        Horisontal,
    }
    public class Player
    {
        public int Coins { get; private set; }
        public int Lifes { get; set; }
        public bool onGround = false;
        public Rectangle Position { get; private set; }
        public int SpeedX;
        public int SpeedY;

        public void AddCoin()
        {
            Coins++;
        }

        public void ChangeSpeedBy(MovingDirection d, int AbsSpeed)
        {
            if (d == MovingDirection.Up && onGround)
            {
                onGround = false;
                SpeedY -= AbsSpeed;
            }
            if (d == MovingDirection.Down)
                SpeedY += AbsSpeed;
            if (d == MovingDirection.Left)
                SpeedX -= AbsSpeed;
            if (d == MovingDirection.Right)
                SpeedX += AbsSpeed;
        }
        public void ChangePosition(Axis axis)
        {
            Position = GetChangedPosition(axis);
            SpeedX /= 2;
        }

        public Rectangle GetChangedPosition(Axis axis)
        {
            var dx = 0;
            var dy = 0;
            if (axis == Axis.Horisontal)
                dx = SpeedX;
            else
                dy = SpeedY;
            return new Rectangle(new Point(Position.X + dx, Position.Y + dy), Position.Size);
        }

        public Player(Rectangle position)
        {
            Lifes = 1;
            Position = position;
        }
    }
}