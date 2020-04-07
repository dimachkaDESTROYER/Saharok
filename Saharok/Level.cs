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
        private int gravityForce;
        public readonly int LevelHeight;
        public readonly int LevelWidth;
        private GameCell[] Walls;
        private GameCell[] Water;
        private List<GameCell> Coins;
        public Player player;
        public Keys KeyPressed;
        public Level(int LevelHeight, int LevelWidth,
                     IEnumerable<Rectangle> walls, IEnumerable<Rectangle> coins, IEnumerable<Rectangle> water,
                     int gForce, Player player)
        {
            IsOver = false;
            this.LevelHeight = LevelHeight;
            this.LevelWidth = LevelWidth;
            Walls = walls.Select(r => new GameCell(CellType.Wall, r)).ToArray();
            Coins = coins.Select(r => new GameCell(CellType.Money, r)).ToList();
            Water = water.Select(r => new GameCell(CellType.Water, r)).ToArray();
            gravityForce = gForce;
            this.player = player;
        }
        private bool TryMove(Axis axis)
        {
            if (!player.onGround)
                player.ChangeSpeedBy(MovingDirection.Down, gravityForce);
            var newPosition = player.GetChangedPosition(axis);
            if (newPosition.Left < 0 || newPosition.Right > LevelWidth)
            {
                player.SpeedX = 0;
                return false;
            }
            if (newPosition.Top < 0 || newPosition.Bottom > LevelHeight)
            {
                player.SpeedY = 0;
                return false;
            }
            if (Walls.Any(c => c.Position.IntersectsWith(newPosition)))
                return false;
            player.ChangePosition(axis);
            return true;
        }

        public void GameTurn()
        {
            TryMove(Axis.Horisontal);
            if (TryMove(Axis.Vertical) && player.SpeedY > 0)
                player.onGround = true;
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
            
            if (player.Lifes <= 0)
                IsOver = true;
        }

        public IEnumerable<GameCell> GetCells()
        {
            foreach(var wall in Walls)
                yield return wall;
            foreach (var coin in Coins)
                yield return coin;
        }
    }
}