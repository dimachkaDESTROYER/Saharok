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
        private int gravityForce;
        public readonly int LevelHeight;
        public readonly int LevelWidth;
        private GameCell[] Walls;
        private List<GameCell> Coins;
        public Player player;
        public Keys KeyPressed;
        public Level(int LevelHeight, int LevelWidth,
                     IEnumerable<Rectangle> walls, IEnumerable<Rectangle> coins,
                     int gForce, Player player)
        {
            this.LevelHeight = LevelHeight;
            this.LevelWidth = LevelWidth;
            Walls = walls.Select(r => new GameCell(CellType.Wall, r)).ToArray();
            Coins = coins.Select(r => new GameCell(CellType.Money, r)).ToList();
            gravityForce = gForce;
            this.player = player;
        }
        private bool TryMove(Axis axis)
        {
            if (!player.onGround)
                player.ChangeSpeedBy(MovingDirection.Down, gravityForce);
            var newPosition = player.GetChangedPosition(axis);
            if (newPosition.Left < 0 || newPosition.Right > LevelWidth ||
               newPosition.Top < 0 || newPosition.Bottom > LevelHeight)
                return false;
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
