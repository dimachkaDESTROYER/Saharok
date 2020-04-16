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
        public Exit(string text, LevelBuilder level)
        {
            this.Size = new Size(1024, 800);
            RichTextBox text_form = new RichTextBox();
            text_form.Text = text;
            text_form.Location = new Point(this.Width / 3, this.Height / 5);
            var exit = new Button();
            this.Controls.Add(text_form);
            exit.BackColor = Color.Gray;
            exit.Text = "Выйти";
            exit.Click += (sender, args) =>
            {
                Application.Exit();
                InitializeComponent();
            };
            Controls.Add(exit);
            exit.Size = new Size(100, 100);
            exit.Location = new Point((int)(0.5 * this.Width / 3), this.Height / 3);
            var again = new Button();
            again.BackColor = Color.Gray;
            again.Text = "Начать заново";
            again.Click += (sender, args) =>
            {
                this.Hide();
                var game = new GameForm(level);
                game.Show();
                InitializeComponent();
            };
            Controls.Add(again);
            again.Size = new Size(100, 100);
            again.Location = new Point(this.Width / 3, this.Height / 3);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Sugar";
            DoubleBuffered = true;
        }


    }
}
