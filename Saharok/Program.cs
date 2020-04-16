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
            for (var y = 400; y <= 450; y += 50)
                for (var x = 80; x <= 80 + 50 * 10; x += 50)
                    coins.Add(new Rectangle(x, y, 25, 25));
            coins.Add(new Rectangle(1000, 450, 25, 25));
            var player = new Player(new Rectangle(25, 600, 50, 50));
            var monstr = new Monstr(new Rectangle(330, 700, 50, 50));
            var finish = new Rectangle(80, 400, 100, 100);
            var l = new LevelBuilder(1024, 800, finish).AddPlayer(player).AddMonstr(monstr)
                                               .AddWalls(new Rectangle(0, 750, 160, 50),
                                                         new Rectangle(320, 750, 320, 50),
                                                         new Rectangle(560, 700, 80, 50),
                                                         new Rectangle(640, 650, 160, 50),
                                                         new Rectangle(880, 600, 80, 50),
                                                         new Rectangle(960, 525, 80, 50),
                                                         new Rectangle(600, 525, 160, 50),
                                                         new Rectangle(320, 525, 160, 50),
                                                         new Rectangle(80, 500, 160, 50))
                                               .AddCoins(coins.ToArray())
                                               .AddWater(new Rectangle(160, 750, 160, 50),
                                                         new Rectangle(640, 750, 480, 50))
                                               .ToLevel();
            Application.Run(new GameForm(l));
        }
    }
}