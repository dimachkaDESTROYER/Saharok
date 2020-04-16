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
        private readonly string finish;
        private readonly string CoinImage;
        private readonly string PlayerImage;
        private readonly string WaterImage;
        private readonly string MonstrImage;
        private bool firstTimeDrawing = true;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        public GameForm(Level level, DirectoryInfo imagesDirectory = null)
        {
            finish = "финиш.png";
            PlayerImage = "длинный.png";
            LifeImage = "жизнь.png";
            CoinImage = "монетка.png";
            WaterImage = "water.png";
            MonstrImage = "кофе.png";
            
            cells[CellType.Wall] = "platform1.png";
            cells[CellType.Money] = CoinImage;
            cells[CellType.Water] = WaterImage;
            this.level = level;
            ClientSize = new Size(
                this.level.LevelWidth,
                this.level.LevelHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Image");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            BackgroundImage = bitmaps["фон.png"];
            var timer = new Timer();
            timer.Interval = 10;
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
        }
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    pressedKeys.Add(e.KeyCode);
        //    if (e.KeyCode == Keys.W)
        //        level.player.ChangeSpeedBy(MovingDirection.Up, 100);
        //    else if (e.KeyCode == Keys.S)
        //        level.player.ChangeSpeedBy(MovingDirection.Down, 20);
        //    else if (e.KeyCode == Keys.D)
        //        level.player.ChangeSpeedBy(MovingDirection.Right, 20);
        //    else if (e.KeyCode == Keys.A)
        //        level.player.ChangeSpeedBy(MovingDirection.Left, 20);
        //}

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            //level.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TranslateTransform(0, GameState.ElementSize);
            //    e.Graphics.FillRectangle(
            //    Brushes.Red, 0, 0, currentLevel.LevelWidth,
            //    currentLevel.LevelHeight);
            if (firstTimeDrawing)
            {

                foreach (var cell in level.GetCells())
                    e.Graphics.DrawImage(bitmaps[cells[cell.Type]], cell.Position);
                firstTimeDrawing = false;
            }
            foreach (var coin in level.GetCoins())
                e.Graphics.DrawImage(bitmaps[CoinImage], coin);
            foreach (var monster in level.monsters)
                e.Graphics.DrawImage(bitmaps[MonstrImage], monster.Position);
            e.Graphics.DrawImage(bitmaps[finish], level.finish);
            e.Graphics.DrawImage(bitmaps[PlayerImage], level.player.Position);
            e.Graphics.DrawString(level.player.Coins.ToString(), new Font("Arial", 30), Brushes.Black, (float)(0.86 * level.LevelWidth), 5);
            e.Graphics.DrawString(level.player.Lifes.ToString(), new Font("Arial", 30), Brushes.Black, (float)(0.76 * level.LevelWidth), 5);
            e.Graphics.DrawImage(bitmaps[CoinImage], new Point((int)(0.92 * level.LevelWidth), 0));
            e.Graphics.DrawImage(bitmaps[LifeImage], new Point((int)(0.8 * level.LevelWidth), 0));
        }

        private void AddMovement()
        {
            if (pressedKeys.Contains(Keys.A))
                level.player.Left(20);
            if (pressedKeys.Contains(Keys.D))
                level.player.Right(20);
            if (pressedKeys.Contains(Keys.Space))
                level.player.Up(50);
        }
        private void TimerTick(object sender, EventArgs args)
        {
            AddMovement();
            var prevPos = level.player.Position;
            foreach (var coin in level.GetCoins())
                Invalidate(coin);
            foreach (var monster in level.monsters)
                Invalidate(monster.Position);
            level.GameTurn();
            if (level.IsOver || level.IsWin)
            {
                this.Hide();
            }
            Invalidate(prevPos, true);
            foreach (var monster in level.monsters)
                Invalidate(monster.Position);
            Invalidate(level.player.Position, true);
            Invalidate(new Rectangle(0, 10, level.LevelWidth, 35), true);
        }


        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(884, 626);
            this.Name = "SugarWindow";
            this.ResumeLayout(false);

        }
    }
}