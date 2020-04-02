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
        private int playerForce;
        private double frictionCoef;
        private readonly int CellWidth;
        private readonly int CellHeigth;
        public readonly int LevelHeight;
        public readonly int LevelWidth;
        public GameCell[,] gameCells;
        public Player player;
        public readonly Func<IEnumerable<MovingDirection>> MovePlayer;

        public Level(int gForce, int pForce, double fCoef,
                    int cellWidth, int cellHeigth,
                    GameCell[,] gameCells, Player player,
                    Func<IEnumerable<MovingDirection>> movePlayer)
        {
            gravityForce = gForce;
            playerForce = pForce;
            frictionCoef = fCoef;
            CellWidth = cellWidth;
            CellHeigth = cellHeigth;
            this.gameCells = gameCells;
            this.player = player;
            LevelHeight = cellHeigth * gameCells.GetLength(1);
            LevelWidth = cellWidth * gameCells.GetLength(0);
            MovePlayer = movePlayer;
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
            var top = player.Position.Top / CellHeigth;
            var bottom = player.Position.Bottom / CellHeigth;
            var left = player.Position.Left / CellWidth;
            var right = player.Position.Right / CellWidth;

        }

        public void GameTurn()
        {
            foreach (var direction in MovePlayer())
                player.ChangeSpeedBy(direction, playerForce);
            CorrectMove();
            if (!player.onGround)
                player.ChangeSpeedBy(MovingDirection.Down, gravityForce);
            player.SpeedX = (int)(player.SpeedX * (1 - frictionCoef));
        }

        public IEnumerable<Tuple<GameCell, Rectangle>> GetCells()
        {
            for (var x = 0; x < gameCells.GetLength(0); x++)
                for (var y = 0; y < gameCells.GetLength(1); y++)
                    if (gameCells[x, y].Type != CellType.Empty)
                        yield return Tuple.Create(gameCells[x, y], new Rectangle(x * CellWidth, y * CellHeigth,
                                                                                CellWidth, CellHeigth));
        }

        public Keys KeyPressed;
    }
}
