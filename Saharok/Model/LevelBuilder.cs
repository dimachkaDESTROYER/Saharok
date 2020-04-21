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
        private int GravityForce = 10;
        private int Width { get; }
        private int Height { get; }
        private List<Rectangle> coins;
        private List<Rectangle> walls;
        private List<Rectangle> water;
        private Player Player;
        private List<Monster> monsters;
        private Rectangle Finish;
        public LevelBuilder(int width, int height, Rectangle finish)
        {
            coins = new List<Rectangle>();
            walls = new List<Rectangle>();
            water = new List<Rectangle>();
            Width = width;
            Height = height;
            Finish = finish;
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
            GravityForce = gravityForce;
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
        public Level ToLevel() => new Level(Height, Width, walls, coins.ToArray(), water,
                                            GravityForce, Player.Copy(), monsters.Select(m => m.Copy()), Finish);
    }
}