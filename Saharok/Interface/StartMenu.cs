using Saharok.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saharok
{
    public partial class Menu : Form
    {
        public Menu(DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();

            //var file = imagesDirectory.GetFiles("*.jpg")[0];
            //BackgroundImage = (Bitmap)Image.FromFile(file.FullName);
            BackColor = GameColors.BackgroundColor;

            var play = new Button()
            {
                BackColor = Color.Yellow,
                Text = "Играть",
                Font = new Font("Roboto", 15),
                Size = new Size(200, 200),
                Location = new Point((int)(this.Width / 4), (int)(1.5 * this.Height / 3))

            };

            play.Click += (sender, args) =>
            {
                var coins = new List<Rectangle>();
                for (var y = 200; y <= 250; y += 50)
                    for (var x = 80; x <= 80 + 50 * 10; x += 50)
                        coins.Add(new Rectangle(x, y, 25, 25));
                coins.Add(new Rectangle(1000, 250, 25, 25));
                var player = new Player(new Rectangle(25, 200, 50, 50), 2);
                var monster = new Monster[] { new Monster(320, 560, MovingDirection.Right, new Rectangle(380, 500, 50, 50)) };
                var finish = new Rectangle(80, 200, 100, 100);
                var l = new LevelBuilder(1024, 600, finish).AddPlayer(player).AddMonsters(monster)
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

                new GameForm(l).Show();
                this.Hide();
            };
            Controls.Add(play);

            var settings = new Button()
            {
                Text = "Настройки",
                Size = play.Size,
                BackColor = Color.Yellow,
                Font = new Font("Roboto", 15),
                Location = new Point((int)(1.6 * this.Width / 3), (int)(1.5 * this.Height / 3))
            };

            //settings.Font
            //settings.FlatStyle
            //settings.ForeColor = Color.Black;
            Controls.Add(settings);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Menu";
            DoubleBuffered = true;
        }
    }
}