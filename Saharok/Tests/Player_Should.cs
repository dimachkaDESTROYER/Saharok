﻿using NUnit.Framework;
using Saharok.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok.Tests
{

    [TestFixture]
    public class Player_Should
    {

        private List<Rectangle> walls = new List<Rectangle> { new Rectangle(0, 600, 200, 200), new Rectangle(400, 600, 200, 200) };
        private List<Rectangle> coins = new List<Rectangle> { new Rectangle(60, 500, 50, 50), new Rectangle(900, 500, 50, 50) };
        private List<Rectangle> water = new List<Rectangle> { new Rectangle(200, 600, 100, 200) };
        private List<Monster> monsters = new List<Monster> { new Monster(1000, 10001, MovingDirection.Right, new Rectangle(1000, 0, 1, 1)) };
        private Rectangle finish = new Rectangle(350, 500, 50, 50);
        private Rectangle shop = new Rectangle(0,0,0,0);

        [Test]
        public void TestMoveRight()
        {
            var player = new Player(new Rectangle(10, 10, 10, 10));
            player.Right(5);
            var expected = 5;
            var actual = player.SpeedX;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoveLeft()
        {
            var player = new Player(new Rectangle(10, 10, 10, 10));
            player.Left(5);
            var expected = -5;
            var actual = player.SpeedX;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoveUpWithoutGravity()
        {
            var player = new Player(new Rectangle(50, 500, 100, 100));
            var level = new Level(1000, 1000, walls, coins, water, 0, player, monsters, finish, shop, null);
            level.player.OnGround = true;
            level.player.Up(5);
            level.GameTurn();
            var expected = new Rectangle(50, 495, 100, 100);
            var actual = level.player.Position;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoveUpWithGravity()
        {
            var player = new Player(new Rectangle(50, 500, 100, 100));
            var level = new Level(1000, 1000, walls, coins, water, 5, player, monsters, finish, shop, null);
            level.player.OnGround = true;
            level.player.Up(10);
            level.GameTurn();
            var expected = new Rectangle(50, 495, 100, 100);
            var actual = level.player.Position;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestMoveUpNotOnGround()
        {
            var player = new Player(new Rectangle(50, 500, 100, 100));
            var level = new Level(1000, 1000, walls, coins, water, 5, player, monsters, finish, shop, null);
            level.player.OnGround = false;
            level.player.Up(70);
            level.GameTurn();
            var expected = new Rectangle(50, 500, 100, 100);
            var actual = level.player.Position;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestCoinAddToPlayer()
        {
            var player = new Player(new Rectangle(10, 10, 10, 10));
            player.AddCoins(1);
            var actual = player.Coins;
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCoinAddToPlayerInGame()
        {

            var player = new Player(new Rectangle(50, 500, 100, 100));
            var level = new Level(1000, 800, walls, coins, water, 0, player, monsters, finish, shop, null);
            level.player.Right(10);
            level.GameTurn();
            var actual = level.player.Coins;
            var expected = 1;
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void TestCoinDisappearAfterCollecting()
        {
            var player = new Player(new Rectangle(50, 500, 100, 100));
            var level = new Level(1000, 800, walls, coins, water, 0, player, monsters, finish, shop, null);
            level.player.Right(10);
            level.GameTurn();
            var actualCoins = level.GetCoins().ToList();
            Assert.AreEqual(1, actualCoins.Count);
        }

        [Test]
        public void TestInteractionWithWater()
        {
            var player = new Player(new Rectangle(50, 500, 100, 100));

            var level = new Level(1000, 1000, walls, coins, water, 1, player, monsters, finish, shop, null);
            level.player.OnGround = true;
            level.player.Right(200);
            level.GameTurn();
            var actual = level.IsOver;
            var actualLives = level.player.Lifes;
            Assert.AreEqual(true, actual);
            Assert.AreEqual(0, actualLives);
        }

        [Test]
        public void TestIsWin()
        {
            var player = new Player(new Rectangle(340, 500, 100, 100));
            
            var level = new Level(1000, 1000, walls, new List<Rectangle>(), water, 5, player, monsters, finish, shop, null);
            level.player.Right(15);
            level.GameTurn();
            var actual = level.IsWin;
            Assert.AreEqual(true, actual);
        }

    }
}
