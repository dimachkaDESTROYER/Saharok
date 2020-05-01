﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Saharok.Model
{
    public class LevelBuilder
    {
        private int gravityForce = 10;
        public int Width { get; }
        public int Height { get; }
        private readonly List<Rectangle> coins;
        private readonly List<Rectangle> walls;
        private readonly List<Rectangle> lava;
        private Player Player;
        private List<Monster> monsters;
        private readonly Rectangle finish;
        public LevelBuilder(int width, int height, Rectangle finish)
        {
            coins = new List<Rectangle>();
            walls = new List<Rectangle>();
            lava = new List<Rectangle>();
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
        public LevelBuilder AddLava(params Rectangle[] lava)
        {
            foreach (var drop in lava)
                this.lava.Add(drop);
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
            foreach (var water in lava)
                yield return new GameCell(CellType.Lava, water);
        }

        public Level ToLevel() => new Level(Height, Width, walls, coins.ToArray(), lava,
                                            gravityForce, Player.Copy(), monsters.Select(m => m.Copy()), finish);
    }
}