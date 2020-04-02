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
            var player = new Player(new Rectangle(80, 80, 100, 100));
            var d = new Dictionary<char, GameCell>();
            d['.'] = new GameCell(CellType.Empty);
            d['W'] = new GameCell(CellType.Wall);
            var map = new[]{"WWWWWW",
                            "....WW",
                            ".....W",
                            "WWWWWW" };
            var l = new LevelBuilder(100, 100, map, d).AddPlayer(player, () => new[] { MovingDirection.Right})
                                                    .ToLevel();
            Application.Run(new GameForm(l));
        }
    }
}
