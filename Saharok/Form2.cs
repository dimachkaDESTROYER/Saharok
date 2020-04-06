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
            BackColor = Color.DarkTurquoise;
            var play = new Button();
            play.BackColor = Color.Gray;
            play.Text = "Играть";
            play.Click += (sender, args) =>
            {
                var player = new Player(new Rectangle(100, 100, 100, 100));
                var l = new LevelBuilder(1000, 800).AddPlayer(player)
                                                   .AddWalls(new Rectangle(0, 600, 200, 200), new Rectangle(600, 200, 50, 400))
                                                   .AddWater(new Rectangle(200, 600, 800, 200))
                                                   .AddCoins(new Rectangle(500, 500, 50, 50))
                                                   .ToLevel();
                var gameForm = new GameForm(l);
                gameForm.Show();
                this.Hide();
            };
            Controls.Add(play);
            play.Size = new Size(100, 100);
            play.Location = new Point(this.Width/3, 2 * this.Height/3);
            var settings = new Button();
            settings.Text = "Настройки";
            settings.Size = play.Size;
            settings.BackColor = Color.Gray;
            //settings.Font
            //settings.FlatStyle
            //settings.ForeColor = Color.Black;
            
            settings.Location = new Point((int)(1.5 * this.Width / 3), 2 * this.Height / 3);
            Controls.Add(settings);
        }
    }
}
