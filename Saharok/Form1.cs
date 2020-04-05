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
    public class GameForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Dictionary<CellType, string> cells = new Dictionary<CellType, string>();
        private readonly Level level;
        private readonly string LifeImage;
        private readonly string CoinImage;
        private readonly string PlayerImage;

        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        public GameForm(Level level, DirectoryInfo imagesDirectory = null)
        {
            PlayerImage = "грусть.png";
            LifeImage = "жизнь.png";
            CoinImage = "монетка.png";
            cells[CellType.Wall] = "математика 3.png";
            cells[CellType.Money] = CoinImage;
            this.level = level;
            ClientSize = new Size(
                this.level.LevelWidth,
                this.level.LevelHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Image");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            BackColor = Color.Aquamarine;
            var timer = new Timer();
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Sugar";
            DoubleBuffered = true;            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            if (e.KeyCode == Keys.W)
                level.player.ChangeSpeedBy(MovingDirection.Up, 7);
            else if (e.KeyCode == Keys.S)
                level.player.ChangeSpeedBy(MovingDirection.Down, 5);
            else if (e.KeyCode == Keys.D)
                level.player.ChangeSpeedBy(MovingDirection.Right, 5);
            else if (e.KeyCode == Keys.A)
                level.player.ChangeSpeedBy(MovingDirection.Left, 5);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            level.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TranslateTransform(0, GameState.ElementSize);
            //    e.Graphics.FillRectangle(
            //    Brushes.Red, 0, 0, currentLevel.LevelWidth,
            //    currentLevel.LevelHeight);
            foreach (var cell in level.GetCells())
                e.Graphics.DrawImage(bitmaps[cells[cell.Type]], cell.Position);
            e.Graphics.DrawImage(bitmaps[PlayerImage], level.player.Position);
            e.Graphics.DrawString(level.player.Coins.ToString(), new Font("Arial", 16), Brushes.Black, 120, 0);
            //e.Graphics.DrawString(level.Lifes.ToString(), new Font("Arial", 16), Brushes.Black, 100, 0);
            e.Graphics.DrawImage(bitmaps[CoinImage], new Point((int)(0.9 * level.LevelWidth), 0));
        }

        private void TimerTick(object sender, EventArgs args)
        {
                level.GameTurn();
                Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(884, 626);
            this.Name = "SugarWindow";
            this.ResumeLayout(false);
        }
    }
}