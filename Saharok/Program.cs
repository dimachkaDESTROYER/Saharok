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
            var player = new Player(new Rectangle(50, 500, 100, 100));
            var finish = new Rectangle(550, 500, 100, 100);
            var l = new LevelBuilder(1000, 800, finish).AddPlayer(player)
                                               .AddWalls(new Rectangle(0, 600, 200, 200),
                                                         new Rectangle(400, 600, 400, 200))
                                               //new Rectangle(600, 500, 100, 100),
                                               //new Rectangle(300, 300, 200, 100),
                                               //new Rectangle(100, 100, 100, 100),
                                               //new Rectangle(300, 100, 200, 100),
                                               //new Rectangle(700, 400, 100, 100))                                            
                                               .AddCoins(new Rectangle(500, 500, 50, 50), new Rectangle(155, 500, 50, 50))
                                               .AddWater(new Rectangle(200, 600, 100, 200))
                                               .ToLevel();
            Application.Run(new GameForm(l));
        }
    }
}