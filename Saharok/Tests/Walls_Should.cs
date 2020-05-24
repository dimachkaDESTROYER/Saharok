using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Saharok.Model;

namespace Saharok.Tests
{
    [TestFixture]
    class Walls_Should
    {
        private LevelBuilder builder;
        private void GenerateLevelBuilder()
        {
            builder = new LevelBuilder(100, 100, new Rectangle(80, 80, 10, 10), null);
            builder.AddWalls(new Rectangle(0, 0, 90, 10),
                             new Rectangle(0, 10, 10, 90),
                             new Rectangle(10, 90, 90, 10),
                             new Rectangle(90, 0, 10, 90))
                   .ChangePhyisics(20)
                   .AddPlayer(new Player(new Rectangle(20, 30, 60, 60)));
        }

        private Level GetLevel()
        {
            if (builder == null)
                GenerateLevelBuilder();
            return builder.ToLevel();
        }

        [Test]
        public void TestUpperWall()
        {
            var level = GetLevel();
            level.player.ChangeSpeedBy(MovingDirection.Up, 40);
            level.GameTurn();
            Assert.AreEqual(new Point(20, 10), level.player.Position.Location);
        }

        [Test]
        public void TestRightWall()
        {
            var level = GetLevel();
            level.player.ChangeSpeedBy(MovingDirection.Right, 40);
            level.GameTurn();
            Assert.AreEqual(new Point(30, 30), level.player.Position.Location);
        }

        [Test]
        public void TestLeftWall()
        {
            var level = GetLevel();
            level.player.ChangeSpeedBy(MovingDirection.Left, 20);
            level.GameTurn();
            Assert.AreEqual(new Point(10, 30), level.player.Position.Location);
        }

        [Test]
        public void TestOnFloor()
        {
            var level = GetLevel();
            level.GameTurn();
            Assert.AreEqual(true, level.player.OnGround);
        }
    }
}
