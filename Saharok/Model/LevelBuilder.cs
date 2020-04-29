using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok.Model
{
    public class LevelBuilder
    {
        private int gravityForce = 10;
        public int Width { get; }
        public int Height { get; }
        private List<Rectangle> coins;
        private List<Rectangle> walls;
        private List<Rectangle> water;
        private Player Player;
        private List<Monster> monsters;
        private readonly Rectangle finish;
        public LevelBuilder(int width, int height, Rectangle finish)
        {
            coins = new List<Rectangle>();
            walls = new List<Rectangle>();
            water = new List<Rectangle>();
            Width = width;
            Height = height;
            this.finish = finish;
            monsters = new List<Monster>();

        }

        public LevelBuilder AddWalls(params Rectangle[] walls)
        {
            foreach (var wall in walls)
                this.walls.Add(wall);
            return this;
        }
        public LevelBuilder AddWater(params Rectangle[] water)
        {
            foreach (var drop in water)
                this.water.Add(drop);
            return this;
        }

        public LevelBuilder AddCoins(params Rectangle[] coins)
        {
            foreach (var coin in coins)
                this.coins.Add(coin);
            return this;
        }

        public LevelBuilder ChangePhyisics(int gravityForce)
        {
            this.gravityForce = gravityForce;
            return this;
        }

        public LevelBuilder AddPlayer(Player player)
        {
            Player = player;
            return this;
        }

        public LevelBuilder AddMonsters(params Monster[] monsters)
        {
            this.monsters = monsters.ToList();
            return this;
        }

        public IEnumerable<GameCell> GetCells()
        {
            foreach (var wall in walls)
                yield return new GameCell(CellType.Wall, wall);
            foreach (var water in water)
                yield return new GameCell(CellType.Water, water);
        }

        public Level ToLevel() => new Level(Height, Width, walls, coins.ToArray(), water,
                                            gravityForce, Player.Copy(), monsters.Select(m => m.Copy()), finish);
    }
}