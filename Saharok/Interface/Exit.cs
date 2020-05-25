using Saharok.Model;
using Saharok.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saharok
{
    public partial class Exit : Form
    {
        public Exit(bool isWin, LevelBuilder level)
        {
            this.Size = new Size(1024, 600);
            BackColor = GameColors.BackgroundColor;
            var text_form = new Label()
            {
                Text = isWin? "Вы выйграли": "Вы проиграли",
                Font = new Font("Roboto", 30),
                Size = new Size(300, 100),
                Location = new Point(this.Width / 3, this.Height / 5)
            };
            this.Controls.Add(text_form);

            var buttonExit = new Button()
            {
                Text = "Выйти",
                Font = new Font("Roboto", 15),
                BackColor = GameColors.ButtonColor,
                Size = new Size(200, 200),
                Location = new Point((int)(1.6 * this.Width / 3), (int)(1.5 * this.Height / 3))

            };
            buttonExit.Click += (sender, args) =>
            {
                Application.Exit();
                InitializeComponent();
            };

            var buttonAgain = new Button()
            {

                BackColor = GameColors.ButtonColor,
                Text = "Вернуться в меню",
                Font = new Font("Roboto", 15),
                Size = new Size(200, 200),
                Location = new Point((int)(this.Width / 4), (int)(1.5 * this.Height / 3))

            };
            Controls.Add(buttonAgain);
            buttonAgain.Click += (sender, args) =>
            {
                this.Hide();
                if (!isWin)
                {
                    var game = new GameForm(level);
                    game.Show();
                }
                else
                    new Menu().Show();
                InitializeComponent();
            };


            Controls.Add(buttonExit);
            Controls.Add(buttonAgain);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Menu";
            DoubleBuffered = true;
        }
    }
}