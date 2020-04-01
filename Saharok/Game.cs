using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    class Game
    {
        public int gravityForce;
        public int playerForce;
        public int frictionForce;
        readonly int CellWidth;
        readonly int CellHeigth;
        public GameCell[,] gameCells;
        public Player player;

        private void CorrectMove()
        {
            player.ChangePositionBy(player.SpeedX, player.SpeedY);
            var top = player.Position.Top / gameCells.GetLength(1);
            var bottom = player.Position.Bottom / gameCells.GetLength(1);
            var left = player.Position.Left / gameCells.GetLength(0);
            var right = player.Position.Right / gameCells.GetLength(0);
            var dx = 0;
            var dy = 0;
            if (player.SpeedX > 0 && Enumerable.Range(top, bottom)
                                               .Select(i => gameCells[right, i])
                                               .Any(c => c.Type == CellType.Wall))
                dx = - player.Position.Right % CellWidth;
            if (player.SpeedX < 0 && Enumerable.Range(top, bottom)
                                               .Select(i => gameCells[left, i])
                                               .Any(c => c.Type == CellType.Wall))
                dx = CellWidth - player.Position.Left % CellWidth;
            if (player.Position.Y > 0)
                throw new NotImplementedException("Дима сделай");
            player.ChangePositionBy(dx, dy);
        }
        public void GameTurn()
        {
            CorrectMove();
            if(!player.onGround)
                player.ChangeSpeed(MovingDirection.Down, gravityForce);
            if (player.SpeedX > 0 && player.SpeedX > frictionForce)
                player.ChangeSpeed(MovingDirection.Left, frictionForce);
            else if (player.SpeedX + frictionForce < 0)
                player.ChangeSpeed(MovingDirection.Right, frictionForce);
        }
    }
}
