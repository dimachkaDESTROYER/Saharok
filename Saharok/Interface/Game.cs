﻿using Saharok.Model;
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
        private string PlayerImage;
        private readonly string MonsterImage;
        private bool inicialise;
        private bool ShopInicialise;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public GameForm(LevelBuilder levelBuilder, DirectoryInfo imagesDirectory = null)
        {
            finish = "финиш.png";
            PlayerImage = "длинный.png";
            LifeImage = "жизнь.png";
            CoinImage = "монетка.png";
            MonsterImage = "монстр.png";
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
            var timer = new Timer();
            timer.Interval = 30;
            timer.Tick += TimerTick;
            timer.Start();
            
        }

        private static Bitmap GetBackgroundImage(LevelBuilder builder)
        {
            var bitmap = new Bitmap(builder.Width, builder.Height);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(GameColors.BackgroundColor), new Rectangle(0,0, 
                                                                builder.Width, builder.Height));
            var wallPen = new SolidBrush(GameColors.WallColor);
            var waterPen = new SolidBrush(Color.FromArgb(100,4,70));
            foreach (var gameCell in builder.GetCells())
            {
                SolidBrush currentBrush;
                if(gameCell.Type == CellType.Wall)
                    currentBrush = wallPen;
                else if (gameCell.Type == CellType.Water)
                    currentBrush = waterPen;
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
            {
                foreach (var tool in level.player.Tools)
                    if (tool.GetToolType() == TypeTool.Magnit)
                    {
                        PlayerImage = "крутой.png";
                        tool.DoAction(level);
                    }
                        
            }
            if (pressedKeys.Contains(Keys.C) && !ShopInicialise)
            {
                ShopInicialise = true;
                new Shop(level, ShopInicialise).Show();
            }
                
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