using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saharok
{
    public class Level
    {
        public bool IsOver { get; private set; }
        public bool IsWin { get; private set; }
        private int gravityForce;
        public readonly int LevelHeight;
        public readonly int LevelWidth;
        private GameCell[] Walls;
        private GameCell[] Water;
        private List<GameCell> Coins;
        public Player player;
        public Rectangle finish;
        public Keys KeyPressed;
        public Level(int LevelHeight, int LevelWidth,
                     IEnumerable<Rectangle> walls, IEnumerable<Rectangle> coins, IEnumerable<Rectangle> water,
                     int gForce, Player player, Rectangle finish)
        {
            IsOver = false;
            this.LevelHeight = LevelHeight;
            this.LevelWidth = LevelWidth;
            Walls = walls.Select(r => new GameCell(CellType.Wall, r)).ToArray();
            Coins = coins.Select(r => new GameCell(CellType.Money, r)).ToList();
            Water = water.Select(r => new GameCell(CellType.Water, r)).ToArray();
            gravityForce = gForce;
            this.player = player;
            this.finish = finish;
        }
        private void Move(Axis axis)
        {
            player.ChangePosition(axis);
            var dx = 0;
            var dy = 0;
            foreach (var wall in Walls.Select(w => w.Position))
                if (player.Position.IntersectsWith(wall))
                {
                    if (axis == Axis.Horisontal)
                    {
                        if (player.SpeedX > 0)
                            dx = wall.Left - player.Position.Right;
                        else if (player.SpeedX < 0)
                            dx = wall.Right - player.Position.Left;
                    }
                    else
                    {
                        if (player.SpeedY > 0)
                        {
                            player.onGround = true;
                            dy = wall.Top - player.Position.Bottom;
                        }
                        else if (player.SpeedY < 0)
                            dy = wall.Bottom - player.Position.Top;
                    }
                }
            player.SpeedX = 0;
            if (dy != 0)
                player.SpeedY = 0;
            player.ChangePosition(dx, dy);
        }

        private void Move()
        {
            if (player.SpeedX != 0)
                Move(Axis.Horisontal);
            if (player.SpeedY != 0)
                Move(Axis.Vertical);

        }

        public void GameTurn()
        {
            if (player.SpeedY < gravityForce)
                player.Down(gravityForce);
            Move();
            var removed = new List<GameCell>();
            foreach (var coin in Coins.Where(c => c.Position.IntersectsWith(player.Position)))
            {
                removed.Add(coin);
                player.AddCoin();
            }
            foreach (var coin in removed)
                Coins.Remove(coin);
            foreach (var water in Water.Where(c => c.Position.IntersectsWith(player.Position)))
            {
                player.Lifes -= 1;
            }
            foreach (var coin in removed)
                Coins.Remove(coin);
            if (player.Position.Bottom > LevelHeight)
                player.Lifes -= 1;
            if (player.Lifes <= 0)
                IsOver = true;
            if (player.Position.IntersectsWith(finish) && Coins.Count == 0)
                IsWin = true;
        }

        public IEnumerable<Rectangle> GetCoins()
        {
            return Coins.Select(c => c.Position);
        }
        public IEnumerable<GameCell> GetCells()
        {
            foreach (var wall in Walls)
                yield return wall;
            foreach (var water in Water)
                yield return water;
            foreach (var coin in Coins)
                yield return coin;
        }
    }
}