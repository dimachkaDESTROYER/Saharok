using NUnit.Framework;
using Saharok.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok.Model
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
        public int Lifes { get; private set; }
        public bool OnGround = false;
        public Rectangle Position { get; private set; }
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        public List<ITool> Tools = new List<ITool>();

        public Player Copy()
        {
            return new Player(Position, Lifes);
        }
        public Player(Rectangle position, int lifes = 1)
        {
            Lifes = lifes;
            Position = position;
        }

        public void AddCoins(int count)
        {
            Coins+= count;
        }

        public bool TryRemoveCoins(int count)
        {
            if (Coins >= count)
            {
                Coins -= count;
                return true;
            }
            return false;
                
        }

        public void ChangeSpeedBy(MovingDirection d, int absSpeed)
        {
            if (d == MovingDirection.Up)
                SpeedY -= absSpeed;
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

        public void Conflict()
        {
            Lifes--;
            ChangeSpeedBy(MovingDirection.Up,70);
        }
        public void ChangePosition(int dx, int dy)
        {
            Position = new Rectangle(new Point(Position.X + dx, Position.Y + dy), Position.Size);
        }

        public void Up(int absSpeed)
        {
            if(OnGround)
                ChangeSpeedBy(MovingDirection.Up, absSpeed);
            OnGround = false;
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