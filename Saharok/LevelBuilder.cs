using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok
{
    public class LevelBuilder
    {
        private int gravityForce = 1;
        private int playerForce = 1;
        private double frictionC = 0.9;
        private int cellWidth;
        private int cellHeight;
        private GameCell[,] gameCells;
        private Player player;
        private Func<IEnumerable<MovingDirection>> movePlayer = () => new MovingDirection[0];
        public LevelBuilder(int cellWidth, int cellHeight, string[] map,
                            Dictionary<char, GameCell> dict)
        {
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            var w = map[0].Length;
            var h = map.Length;
            gameCells = new GameCell[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    gameCells[x, y] = dict[map[y][x]];
        }

        public LevelBuilder ChangePhyisics(int gravityForce, int playerForce, double frictionCoef)
        {
            this.gravityForce = gravityForce;
            this.playerForce = playerForce;
            this.frictionC = frictionCoef;
            return this;
        }

        public LevelBuilder AddPlayer(Player player, Func<IEnumerable<MovingDirection>> moving)
        {
            this.player = player;
            movePlayer = moving;
            return this;
        }

        public Level ToLevel() => new Level(gravityForce, playerForce, frictionC,
                                            cellWidth, cellHeight, gameCells,
                                            player, movePlayer);
    }
}
