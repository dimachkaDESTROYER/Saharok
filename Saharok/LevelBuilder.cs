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
        private int width;
        private int height;
        private IEnumerable<GameCell> gameCells;
        private Player player;
        public LevelBuilder(int width, int height, IEnumerable<GameCell> cells)
        {
            this.width = width;
            this.height = height;
            gameCells = cells;
        }

        public LevelBuilder ChangePhyisics(int gravityForce)
        {
            this.gravityForce = gravityForce;
            return this;
        }

        public LevelBuilder AddPlayer(Player player)
        {
            this.player = player;
            return this;
        }

        public Level ToLevel() => new Level(height, width, gravityForce, gameCells,
                                            player);
    }
}
