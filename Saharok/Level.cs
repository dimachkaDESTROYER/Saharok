using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    class Level
    {
        private int gravityForce;
        private int playerForce;
        private int frictionForce;
        public readonly int CellWidth;
        public readonly int CellHeigth;
        public GameCell[,] gameCells;
        public Player player;
        public readonly Func<IEnumerable<MovingDirection>> MovePlayer;

        public Level(int gForce, int pForce, int fForce,
                    int cellWidth, int cellHeigth,
                    GameCell[,] gameCells, Player player,
                    Func<IEnumerable<MovingDirection>> movePlayer)
        {
            gravityForce = gForce;
            playerForce = pForce;
            frictionForce = fForce;
            CellWidth = cellWidth;
            CellHeigth = cellHeigth;
            this.gameCells = gameCells;
            this.player = player;
        }

        private IEnumerable<GameCell> GetSquareRange(int left, int right, int top, int bottom)
        {
            foreach (var x in Enumerable.Range(left, right))
                foreach (var y in Enumerable.Range(top, bottom))
                    yield return gameCells[x, y];
        }

        private void CorrectMove()
        {
            player.ChangePositionBy(player.SpeedX, player.SpeedY);
            var top = player.Position.Top / gameCells.GetLength(1);
            var bottom = player.Position.Bottom / gameCells.GetLength(1);
            var left = player.Position.Left / gameCells.GetLength(0);
            var right = player.Position.Right / gameCells.GetLength(0);
            var dx = 0;
            var dy = 0;
            if (player.SpeedX > 0 && GetSquareRange(right, right, top, bottom).Any(c => c.Type == CellType.Wall))
                dx = -player.Position.Right % CellWidth;
            if (player.SpeedX < 0 && GetSquareRange(left, left, top, bottom).Any(c => c.Type == CellType.Wall))
                dx = CellWidth - player.Position.Left % CellWidth;
            if (player.SpeedY > 0 && GetSquareRange(left, right, top, top).Any(c => c.Type == CellType.Wall))
                dy = CellHeigth - player.Position.Top % CellHeigth;
            if (player.SpeedY > 0 && GetSquareRange(left, right, bottom, bottom).Any(c => c.Type == CellType.Wall))
            {
                dy = -player.Position.Bottom % CellHeigth;
                player.onGround = true;
            }
            player.ChangePositionBy(dx, dy);
        }

        public void GameTurn()
        {
            foreach (var direction in MovePlayer())
                player.ChangeSpeed(direction, playerForce);
            CorrectMove();
            if (!player.onGround)
                player.ChangeSpeed(MovingDirection.Down, gravityForce);
            if (player.SpeedX > 0 && player.SpeedX > frictionForce)
                player.ChangeSpeed(MovingDirection.Left, frictionForce);
            else if (player.SpeedX + frictionForce < 0)
                player.ChangeSpeed(MovingDirection.Right, frictionForce);
        }
    }
}
