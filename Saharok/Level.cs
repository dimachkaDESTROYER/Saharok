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
        public readonly IEnumerable<GameCell> NonEmptygameCells;
        public Player player;
        public Keys KeyPressed;
        public Level(int LevelHeight, int LevelWidth, int gForce,
                    IEnumerable<GameCell> gameCells, Player player)
        {
            this.LevelHeight = LevelHeight;
            this.LevelWidth = LevelWidth;
            gravityForce = gForce;
            NonEmptygameCells = gameCells;
            this.player = player;
        }
        private void CorrectMove(Axis axis)
        {
            if (!player.onGround)
                player.ChangeSpeedBy(MovingDirection.Down, gravityForce);
            var newPosition = player.GetChangedPosition(axis);
            if (newPosition.Left < 0 || newPosition.Right > LevelWidth ||
               newPosition.Top < 0 || newPosition.Bottom > LevelHeight)
                return;
            if (NonEmptygameCells.Any(c => c.Position.IntersectsWith(newPosition)))
                return;
            player.ChangePosition(axis);
            player.onGround = true;
        }

        public void GameTurn()
        {
            CorrectMove(Axis.Horisontal);
            CorrectMove(Axis.Vertical);
        }
    }
}
