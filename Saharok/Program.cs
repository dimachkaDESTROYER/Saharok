using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saharok
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var coins = new List<Rectangle>();
            for (var y = 200; y <= 250; y += 50)
                for (var x = 80; x <= 80 + 50 * 10; x += 50)
                    coins.Add(new Rectangle(x, y, 25, 25));
            coins.Add(new Rectangle(1000, 250, 25, 25));
            var player = new Player(new Rectangle(25, 200, 50, 50));
            var monster = new Monster[] { new Monster(320, 560, MovingDirection.Right, new Rectangle(380, 500, 50, 50)) };
            var finish = new Rectangle(80, 200, 100, 100);
            var l = new LevelBuilder(1024, 600, finish).AddPlayer(player).AddMonstr(monster)
                                               .AddWalls(new Rectangle(0, 550, 160, 50),
                                                         new Rectangle(320, 550, 320, 50),
                                                         new Rectangle(560, 500, 80, 50),
                                                         new Rectangle(640, 450, 160, 50),
                                                         new Rectangle(880, 400, 80, 50),
                                                         new Rectangle(960, 325, 80, 50),
                                                         new Rectangle(600, 325, 160, 50),
                                                         new Rectangle(320, 325, 160, 50),
                                                         new Rectangle(80, 300, 160, 50))
                                               .AddCoins(coins.ToArray())
                                               .AddWater(new Rectangle(160, 550, 160, 50),
                                                         new Rectangle(640, 550, 480, 50));
                                               
            Application.Run(new GameForm(l));
        }
    }
}