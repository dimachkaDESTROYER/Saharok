using NUnit.Framework;
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
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }

        public Player Copy()
        {
            return new Player(Position);
        }
        public Player(Rectangle position)
        {
            Lifes = 2;
            Position = position;
        }

        public void AddCoin()
        {
            Coins++;
        }

        public void ChangeSpeedBy(MovingDirection d, int absSpeed)
        {
            if (d == MovingDirection.Up && onGround && SpeedY == 0)
            {
                onGround = false;
                SpeedY -= absSpeed;
            }
            if (d == MovingDirection.Down)
                SpeedY += absSpeed;
            if (d == MovingDirection.Left)
                SpeedX -= absSpeed;
            if (d == MovingDirection.Right)
                SpeedX += absSpeed;
        }
        public void ChangePosition(Axis axis)
        {
            var dx = 0;
            var dy = 0;
            if (axis == Axis.Horisontal)
                dx = SpeedX;
            else
                dy = SpeedY;
            ChangePosition(dx, dy);
        }

        public void ChangePosition(int dx, int dy)
        {
            Position = new Rectangle(new Point(Position.X + dx, Position.Y + dy), Position.Size);
        }

        public void Up(int absSpeed)
        {
            ChangeSpeedBy(MovingDirection.Up, absSpeed);
        }

        public void Left(int absSpeed)
        {
            ChangeSpeedBy(MovingDirection.Left, absSpeed);
        }

        public void Down(int absSpeed)
        {
            ChangeSpeedBy(MovingDirection.Down, absSpeed);
        }

        public void Right(int absSpeed)
        {
            ChangeSpeedBy(MovingDirection.Right, absSpeed);
        }
    }
}