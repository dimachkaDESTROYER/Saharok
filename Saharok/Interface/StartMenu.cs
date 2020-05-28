using Saharok.Interface;
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
        public LevelBuilder FirstLevel;
        public Menu(DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();
            GameImages.ImageBackGround();
            BackgroundImage = GameImages.backgroung;
            ClientSize = new Size(860, 600);

            var play = new Button()
            {
                BackColor = Color.Yellow,
                Text = "Играть",
                Font = new Font("Roboto", 15),
                Size = new Size(400, 50),
            };

            play.Click += (sender, args) =>
            {
                var coins3 = new List<Rectangle>();
                for (var y = 250; y <= 290; y += 40)
                    for (var x = 250; x <= 80 + 50 * 10; x += 50)
                        coins3.Add(new Rectangle(x, y, 25, 25));
                coins3.Add(new Rectangle(1000, 250, 25, 25));
                coins3.Add(new Rectangle(60, 30, 25, 25));
                coins3.Add(new Rectangle(95, 30, 25, 25));
                coins3.Add(new Rectangle(960, 165, 25, 25));

                var coins1 = new List<Rectangle>() { new Rectangle(450, 300, 50, 50), new Rectangle(800, 400, 50, 50), new Rectangle(800, 150, 50, 50) };
                var coins2 = new List<Rectangle>() { new Rectangle(200, 350, 50, 50), new Rectangle(550, 100, 50, 50)};
                for (var x = 0; x <= 150; x += 30)
                    coins2.Add(new Rectangle(x, 275, 50, 50));
                var player = new Player(new Rectangle(25, 500, 50,  50), 2);
                var monster3 = new Monster[] { new Monster(320, 560, MovingDirection.Right, new Rectangle(380, 500, 50, 50)),
                                              new Monster(320, 740, MovingDirection.Left, new Rectangle(380, 145, 50, 50)),
                                              new Monster(580, 850, MovingDirection.Right, new Rectangle(480, 25, 50, 50))};
                var monster2 = new Monster[] { new Monster(300, 550, MovingDirection.Right, new Rectangle(400, 400, 50, 50)),
                                               new Monster(250, 450, MovingDirection.Right, new Rectangle(300, 50, 50, 50))};
                var finish3 = new Rectangle(880, 5, 75, 75);
                var finish = new Rectangle(100, 75, 100, 75);
                var finish2 = new Rectangle(750, 175, 100, 75);
                var level3 = new LevelBuilder(1024, 600, finish3, new Rectangle(320, 265, 50, 80), null).AddPlayer(player).AddMonsters(monster3)
                                                   .AddWalls(new Rectangle(0, 550, 160, 50),
                                                             new Rectangle(320, 550, 320, 50),
                                                             new Rectangle(560, 500, 80, 50),
                                                             new Rectangle(640, 450, 160, 50),
                                                             new Rectangle(880, 400, 80, 50),
                                                             new Rectangle(960, 325, 80, 50),
                                                             new Rectangle(600, 350, 160, 25),
                                                             new Rectangle(320, 350, 160, 25),
                                                             new Rectangle(80, 300, 160, 25),
                                                             new Rectangle(20, 225, 120, 25),
                                                             new Rectangle(320, 195, 440, 25),
                                                             new Rectangle(880, 195, 120, 25),
                                                             new Rectangle(580, 75, 400, 25),
                                                             new Rectangle(280, 95, 160, 25),
                                                             new Rectangle(40, 70, 160, 25))
                                                   .AddCoins(coins3.ToArray())
                                                   .AddLava(new Rectangle(160, 550, 160, 50),
                                                             new Rectangle(640, 550, 480, 50));
                var level2 = new LevelBuilder(1024, 600, finish2, new Rectangle(0,275, 50, 70 ), level3).AddPlayer(player)
                                                                         .AddMonsters(monster2)
                                                                         .AddHints(new Hint(new Rectangle(0, 0, 1024, 600),
                                                                             "Остерегайтесь стаканчиков с кофе"))
                                                                         .AddWalls(new Rectangle(0, 550, 150, 50),
                                                                                   new Rectangle(150, 450, 400, 50),
                                                                                   new Rectangle(0, 350, 150, 50),
                                                                                   new Rectangle(0, 200, 150, 50),
                                                                                   new Rectangle(600, 350, 200, 50),
                                                                                   new Rectangle(250, 275, 200, 50),
                                                                                   new Rectangle(500, 200, 150, 50),
                                                                                   new Rectangle(750, 250, 100, 50),
                                                                                   new Rectangle(200, 100, 300, 50))
                                                                         .AddLava(new Rectangle(150, 550, 900, 50)).AddCoins(coins2.ToArray());                                                           

                var level1 = new LevelBuilder(1024, 600, finish, new Rectangle(0,0,0,0), level2).AddPlayer(player)
                                                                      .AddCoins(coins1.ToArray())
                                                                      .AddLava(new Rectangle(250, 550, 400, 100))
                                                                      .AddHints(new Hint(new Rectangle(0, 0, 1000, 1000),
                                                                          "Используйте A, D и пробел для перемещения "))
                                                                      .AddWalls(new Rectangle(0, 550, 250, 100),
                                                                                new Rectangle(650, 550, 350, 100),
                                                                                new Rectangle(300, 450, 100, 50),
                                                                                new Rectangle(450, 400, 100, 50),
                                                                                new Rectangle(600, 480, 100, 50),
                                                                                new Rectangle(600, 300, 200, 50),
                                                                                new Rectangle(300, 200, 200, 50),
                                                                                new Rectangle(100, 150, 100, 50));
                new GameForm(level1).Show();
                this.Hide();
            };
            Controls.Add(play);
            var table = new TableLayoutPanel();
            table.BackgroundImage = GameImages.backgroung;
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 41));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
           
            table.Controls.Add(play, 1, 1);
            table.SetColumnSpan(play, 2);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Menu";
            DoubleBuffered = true;
        }
    }
}