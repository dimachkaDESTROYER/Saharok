using Saharok.Interface;
using Saharok.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Saharok
{
    public class GameForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Level level;
        private readonly LevelBuilder levelBuilder;
        private readonly string LifeImage;
        private readonly string finish;
        private readonly string CoinImage;
        private Bitmap PlayerImage;
        private Font fontForMoneyAndLifes;
        private readonly string MonsterImage;
        private Form shop;
        private Timer timer;
        private Dictionary<Keys, Dictionary<TypeTool, Bitmap>> keyWithTool;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public GameForm(LevelBuilder levelBuilder, DirectoryInfo imagesDirectory = null)
        {
            GameImages.PlayerImages.ImagesForSugar();
            finish = "финиш.png";
            PlayerImage = GameImages.PlayerImages.Simple;
            LifeImage = "жизнь.png";
            CoinImage = "монетка.png";
            MonsterImage = "монстр.png";
            
            fontForMoneyAndLifes = new Font("Arial", 30);
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
            BackgroundImage = GetBackgroundImage(levelBuilder);
            keyWithTool = new Dictionary<Keys, Dictionary<TypeTool, Bitmap>>();      
            keyWithTool[Keys.M] = new Dictionary<TypeTool, Bitmap>() { { TypeTool.Magnet, GameImages.PlayerImages.WithMagnet } };
            keyWithTool[Keys.S] = new Dictionary<TypeTool, Bitmap>() { { TypeTool.Student, GameImages.PlayerImages.WithStudent } };
            keyWithTool[Keys.B] = new Dictionary<TypeTool, Bitmap>() { { TypeTool.Boot, GameImages.PlayerImages.WithBoots } };


            timer = new Timer();
            timer.Interval = 40;
            timer.Tick += TimerTick;
            timer.Start();
            
        }

        private static Bitmap GetBackgroundImage(LevelBuilder builder)
        {
            var bitmap = new Bitmap(builder.Width, builder.Height);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(GameColors.BackgroundColor), new Rectangle(0,0, 
                                                                builder.Width, builder.Height));
            var wallBrush = new SolidBrush(GameColors.WallColor);
            var lavaBrush = new SolidBrush(GameColors.LavaColor);
            foreach (var gameCell in builder.GetCells())
            {
                SolidBrush currentBrush;
                if(gameCell.Type == CellType.Wall)
                    currentBrush = wallBrush;
                else if (gameCell.Type == CellType.Lava)
                    currentBrush = lavaBrush;
                else
                    throw new Exception("unexpected CellType");
                g.FillRectangle(currentBrush, gameCell.Position);
            }

            return bitmap;
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

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var coin in level.GetCoins())
                e.Graphics.DrawImage(bitmaps[CoinImage], coin);
            foreach (var monster in level.monsters)
                e.Graphics.DrawImage(bitmaps[MonsterImage], monster.Position);
            e.Graphics.DrawImage(bitmaps[finish], level.finish);
            e.Graphics.DrawImage(PlayerImage, level.player.Position);
            e.Graphics.DrawString(level.player.Coins.ToString(), fontForMoneyAndLifes, Brushes.Black, (float)(0.86 * level.LevelWidth), 5);
            e.Graphics.DrawString(level.player.Lifes.ToString(), fontForMoneyAndLifes, Brushes.Black, (float)(0.76 * level.LevelWidth), 5);
            e.Graphics.DrawImage(bitmaps[CoinImage], new Point((int)(0.8 * level.LevelWidth - bitmaps[LifeImage].Width), 0)); //!!! поменять на арифметику
            e.Graphics.DrawImage(bitmaps[LifeImage], new Point((int)(0.8 * level.LevelWidth), 0)); //непонятно
            //рисовать список обьектов по типу 
        }

        private void ReadPressedKeys()
        {
            // к словарям к кнопке => картинка, tool
            if (pressedKeys.Contains(Keys.A))
                level.player.Left(20);
            if (pressedKeys.Contains(Keys.D))
                level.player.Right(20);
            if (pressedKeys.Contains(Keys.Space))
                level.player.Up(50);
            foreach (var e in keyWithTool.Keys)
                foreach (var k in keyWithTool[e].Keys)
                    if (pressedKeys.Contains(e) && level.player.TryChangeTool(k))
                        PlayerImage = keyWithTool[e][k];
            if (pressedKeys.Contains(Keys.C))
            {
                pressedKeys.Remove(Keys.C);
                var shop = new Shop(level);
                shop.Show();
            }
            //непонятно как менять tools   
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
            if ((level.IsOver || level.IsWin))
            {
                timer.Stop();
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
            var exit = new Exit(text, levelBuilder);
            this.Hide();
            exit.ShowDialog();
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