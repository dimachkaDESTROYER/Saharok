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
                                                         
                                               .AddCoins(new Rectangle(330, 600, 25, 25), new Rectangle(640, 425, 25, 25))
                                               .AddWater(new Rectangle(160, 650, 160, 50))
                                               .ToLevel();
            Application.Run(new GameForm(l));
        }
    }
}