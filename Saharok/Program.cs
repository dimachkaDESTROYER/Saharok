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
            var player = new Player(new Rectangle(100, 100, 100, 100));
            var cells = new[] { new GameCell(CellType.Wall, new Rectangle(0, 600, 1000, 200))};
            var l = new LevelBuilder(1000, 800, cells).AddPlayer(player)
                                                    .ToLevel();
            Application.Run(new GameForm(l));
        }
    }
}
