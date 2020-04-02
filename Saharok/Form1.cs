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
        private readonly Level currentLevel;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();


        public GameForm(Level level, DirectoryInfo imagesDirectory = null)
        {
            currentLevel = level;
            ClientSize = new Size(
                currentLevel.LevelWidth,
                currentLevel.LevelHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Image");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 15;
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
            currentLevel.KeyPressed = e.KeyCode;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            currentLevel.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TranslateTransform(0, GameState.ElementSize);
            e.Graphics.FillRectangle(
                Brushes.Red, 0, 0, currentLevel.LevelWidth,
                currentLevel.LevelHeight);
            foreach (var cell in currentLevel.GetCells())
                e.Graphics.DrawImage(bitmaps[cell.Item1.GetImageFileName()], cell.Item2);
            e.Graphics.DrawImage(bitmaps[currentLevel.player.GetImageFileName()], currentLevel.player.Position);
            e.Graphics.ResetTransform();
            //e.Graphics.DrawString(currentLevel.Scores.ToString(), new Font("Arial", 16), Brushes.Black, 120, 0);
            //e.Graphics.DrawString(currentLevel.Lifes.ToString(), new Font("Arial", 16), Brushes.Black, 100, 0);
        }

        private void TimerTick(object sender, EventArgs args)
        {
            currentLevel.GameTurn();
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