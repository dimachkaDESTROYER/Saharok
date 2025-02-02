﻿using System.Drawing;

namespace Saharok.Model
{
    public class Monster
    {

        public Rectangle Position { get; private set; }
        private int LeftBorder { get; }
        private int RightBorder { get; }
        private MovingDirection Direction { get; set; }
        private static int Speed = 10;


        public Monster(int leftBorder, int rightBorder, MovingDirection direction, Rectangle position)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Direction = direction;
            Position = position;
        }

        public Monster Copy()
        {
            return new Monster(LeftBorder, RightBorder, Direction, Position);
        }
        public void ChangePosition()
        {
            if (Direction == MovingDirection.Left)
            {
                if (Position.Left - Speed < LeftBorder)
                    Direction = MovingDirection.Right;
                else
                    Position = new Rectangle(new Point(Position.X - Speed, Position.Y), Position.Size);
            }
            else
            {
                if (Position.Right + Speed > RightBorder)
                    Direction = MovingDirection.Left;
                else
                    Position = new Rectangle(new Point(Position.X + Speed, Position.Y), Position.Size);
            }
        }
   

    }
}
