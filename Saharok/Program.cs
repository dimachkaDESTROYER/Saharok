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
            for (var y = 250; y <= 350; y += 50)
                for (var x = 80; x <= 80 + 50 * 10; x += 50)
                    coins.Add(new Rectangle(x, y, 25, 25));
            coins.Add(new Rectangle(1000, 350, 25, 25));
            var player = new Player(new Rectangle(25, 500, 50, 50));
            var finish = new Rectangle(80, 300, 100, 100);
            var l = new LevelBuilder(1024, 800, finish).AddPlayer(player)
                                               .AddWalls(new Rectangle(0, 650, 160, 50),
                                                         new Rectangle(320, 650, 320, 50),
                                                         new Rectangle(500, 600, 80, 50),
                                                         new Rectangle(640, 550, 160, 50),
                                                         new Rectangle(880, 500, 80, 50),
                                                         new Rectangle(1000, 425, 80, 50),
                                                         new Rectangle(600, 450, 160, 50),
                                                         new Rectangle(320, 425, 160, 50),
                                                         new Rectangle(80, 400, 160, 50))
                                               .AddCoins(coins.ToArray())
                                               .AddWater(new Rectangle(160, 650, 160, 50))
                                               .ToLevel();
            Application.Run(new GameForm(l));
        }
    }
}