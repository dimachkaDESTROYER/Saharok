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
    public class GameForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Dictionary<CellType, string> cells = new Dictionary<CellType, string>();
        private readonly Level level;
        private readonly LevelBuilder levelBuilder;
        private readonly string LifeImage;
        private readonly string finish;
        private readonly string CoinImage;
        private readonly string PlayerImage;
        private readonly string WaterImage;
        private readonly string MonstrImage;
        private bool inicialise;
        private bool firstTimeDrawing = true;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private CoinMagnet coinMagnet;
        public GameForm(LevelBuilder levelBuilder, DirectoryInfo imagesDirectory = null)
        {
            finish = "финиш.png";
            PlayerImage = "длинный.png";
            LifeImage = "жизнь.png";
            CoinImage = "монетка.png";
            WaterImage = "water.png";
            MonstrImage = "кофе.png";
            coinMagnet = new CoinMagnet(10, 10, 100);
            cells[CellType.Wall] = "platform1.png";
            cells[CellType.Money] = CoinImage;
            cells[CellType.Water] = WaterImage;
            this.levelBuilder = levelBuilder;
            this.level = levelBuilder.ToLevel();
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

        private void ReadPressedKeys()
        {
            if (pressedKeys.Contains(Keys.A))
                level.player.Left(20);
            if (pressedKeys.Contains(Keys.D))
                level.player.Right(20);
            if (pressedKeys.Contains(Keys.Space))
                level.player.Up(50);
            if (pressedKeys.Contains(Keys.M))
                coinMagnet.Magnetize(level);
        }
        private void TimerTick(object sender, EventArgs args)
        {
            foreach (var coin in level.GetCoins())
                Invalidate(coin);
            ReadPressedKeys();
            Invalidate(level.player.Position); 
            foreach (var monster in level.monsters)
                Invalidate(monster.Position);

            level.GameTurn();
            if ((level.IsOver || level.IsWin) && !inicialise)
            {
                if (level.IsOver)
                    Exit("Вы проиграли");
                else
                    Exit("Победа!");
               
            }
            foreach (var coin in level.GetCoins())
                Invalidate(coin);
            foreach (var monster in level.monsters)
                Invalidate(monster.Position);
            Invalidate(level.player.Position, true);
            Invalidate(new Rectangle(0, 10, level.LevelWidth, 35), true);
        }

        private void Exit(string text)
        {
            this.inicialise = true;
            var ex = new Exit(text, levelBuilder);
            this.Hide();
            ex.ShowDialog();
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