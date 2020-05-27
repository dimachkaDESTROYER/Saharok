
using Saharok.Interface;
using Saharok.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Media;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using FontFamily = System.Drawing.FontFamily;

namespace Saharok
{
    public class GameForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Level level;
        private readonly LevelBuilder levelBuilder;
        private readonly string LifeImage;
        private readonly string finish;
        private readonly string shop;
        private readonly string CoinImage;
        private Bitmap PlayerImage;
        private Font fontForMoneyAndLifes;
        private readonly string MonsterImage;
        private Timer timer;
        private Dictionary<TypeTool, Bitmap> keyWithTool;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private Dictionary<SpriteType, Bitmap> spritesImages;
        private Dictionary<CellType, SolidBrush> brushesOfCells = new Dictionary<CellType, SolidBrush>();
        private int yForCoinsAndHearths;
        private int xForCoinsAndHearths;
        private int yForHints;
        private int xForTool;
        private Label hintLabel;
        private PictureBox picture;
        private PictureBox picture1;
        private PictureBox picture2;
        private Dictionary<TypeTool, Bitmap> toolImage;

        public GameForm(LevelBuilder levelBuilder, DirectoryInfo imagesDirectory = null, int coins = -1, int lifes = -1)
        {
            GameImages.PlayerImages.ImagesForSugar();

            finish = "финиш.png";
            shop = "shop.png";
            PlayerImage = GameImages.PlayerImages.Simple;
            LifeImage = "жизнь.png";
            CoinImage = "монетка.png";
            MonsterImage = "монстр.png";
            GameImages.ImagesForShop();
            toolImage = new Dictionary<TypeTool, Bitmap>();
            toolImage[TypeTool.Boot] = GameImages.Boots;
            toolImage[TypeTool.Magnet] = GameImages.CoinMagnet;
            toolImage[TypeTool.Student] = GameImages.Student;
            Controls.Add(hintLabel);
            yForHints = 60;
            xForTool = 100;
            fontForMoneyAndLifes = new Font("Arial", 30);
            this.levelBuilder = levelBuilder;
            this.level = levelBuilder.ToLevel();
            if (coins != -1)
                level.player.Coins = coins;
            if (lifes != -1)
                level.player.Lifes = lifes;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Image");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            yForCoinsAndHearths = bitmaps[LifeImage].Height + 10;
            xForCoinsAndHearths = bitmaps[LifeImage].Width;
            ClientSize = new Size(
                this.level.LevelWidth,
                yForCoinsAndHearths + yForHints + this.level.LevelHeight);

            keyWithTool = new Dictionary<TypeTool, Bitmap>();
            keyWithTool[TypeTool.Magnet] = GameImages.PlayerImages.WithMagnet;
            keyWithTool[TypeTool.Student] = GameImages.PlayerImages.WithStudent;
            keyWithTool[TypeTool.Boot] = GameImages.PlayerImages.WithBoots;

            brushesOfCells[CellType.Lava] = new SolidBrush(GameColors.LavaColor);
            brushesOfCells[CellType.Wall] = new SolidBrush(GameColors.WallColor);

            BackgroundImage = GetBackgroundImage(levelBuilder);

            spritesImages = new Dictionary<SpriteType, Bitmap>();
            spritesImages[SpriteType.Player] = PlayerImage;
            spritesImages[SpriteType.Coin] = bitmaps[CoinImage];
            spritesImages[SpriteType.Monster] = bitmaps[MonsterImage];

            picture = new PictureBox()
            {
                BackColor = GameColors.ButtonColor,
                BorderStyle = BorderStyle.FixedSingle,  
                Location = new Point(level.LevelWidth - 3 * xForTool, ClientSize.Height - yForHints),
                Size = new Size(xForTool, yForHints),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Controls.Add(picture);

            picture1 = new PictureBox()
            {
                BackColor = GameColors.ButtonColor,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(level.LevelWidth - 2 * xForTool, ClientSize.Height - yForHints),
                Size = new Size(xForTool, yForHints),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Controls.Add(picture1);

            picture2 = new PictureBox()
            {
                BackColor = GameColors.ButtonColor,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(level.LevelWidth - xForTool, ClientSize.Height - yForHints),
                Size = new Size(xForTool, yForHints),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Controls.Add(picture2);

            hintLabel = new Label()
            {
                BackColor = GameColors.ButtonColor,
                Text = "",
                Font = new Font("Roboto", 15),
                Size = new Size(level.LevelWidth - 3 * xForTool, yForHints),
                Location = new Point(0, level.LevelHeight + yForCoinsAndHearths),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            Controls.Add(hintLabel);
            timer = new Timer();
            timer.Interval = 40;
            timer.Tick += TimerTick;
            timer.Start();
        }
        private Bitmap GetBackgroundImage(LevelBuilder builder)
        {
            var bitmap = new Bitmap(builder.Width, builder.Height + yForCoinsAndHearths);
            var g = Graphics.FromImage(bitmap);
            g.DrawImage(bitmaps[CoinImage], level.LevelWidth - xForCoinsAndHearths * 4, 5);
            g.DrawImage(bitmaps[LifeImage], level.LevelWidth - xForCoinsAndHearths * 2, 5);
            g.FillRectangle(new SolidBrush(GameColors.BackgroundColor), new Rectangle(0, 0,
                builder.Width, builder.Height));
            g.FillRectangle(new SolidBrush(GameColors.BackgroundColor), new Rectangle(0, 0,
                                                                builder.Width, builder.Height + yForCoinsAndHearths));
            g.DrawImage(bitmaps[CoinImage], level.LevelWidth - xForCoinsAndHearths * 4, 5);
            g.DrawImage(bitmaps[LifeImage], level.LevelWidth - xForCoinsAndHearths * 2, 5);
            g.TranslateTransform(0, yForCoinsAndHearths);
            foreach (var gameCell in builder.GetCells())
                g.FillRectangle(brushesOfCells[gameCell.Type],
                            gameCell.Position);
            g.DrawImage(bitmaps[finish], level.finish);
            g.DrawImage(bitmaps[shop], level.shop);
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
            e.Graphics.DrawString(level.player.Lifes.ToString(), fontForMoneyAndLifes, Brushes.Black, (float)(level.LevelWidth - xForCoinsAndHearths), 5);
            e.Graphics.DrawString(level.player.Coins.ToString(), fontForMoneyAndLifes, Brushes.Black, (float)(level.LevelWidth - 3 * xForCoinsAndHearths), 5);
            e.Graphics.TranslateTransform(0, yForCoinsAndHearths);
            foreach (var sprite in level.GetSprites())
            {
                if (spritesImages.TryGetValue(sprite.Type, out var image))
                    e.Graphics.DrawImage(image, sprite.Position);
                else
                    e.Graphics.DrawRectangle(Pens.Violet, sprite.Position);
            }
        }

        private void ReadPressedKeys()
        {
            if (pressedKeys.Contains(Keys.A))
                level.player.Left(20);
            if (pressedKeys.Contains(Keys.D))
                level.player.Right(20);
            if (pressedKeys.Contains(Keys.Space))
                level.player.Up(50);
            if (pressedKeys.Contains(Keys.D1) && level.player.TryChangeTool(level.player.Tools[0].GetToolType()))
                spritesImages[SpriteType.Player] = keyWithTool[level.player.Tools[0].GetToolType()];
            if (pressedKeys.Contains(Keys.D2) && level.player.TryChangeTool(level.player.Tools[1].GetToolType()))
                spritesImages[SpriteType.Player] = keyWithTool[level.player.Tools[1].GetToolType()];
            if (pressedKeys.Contains(Keys.D3) && level.player.TryChangeTool(level.player.Tools[2].GetToolType()))
                spritesImages[SpriteType.Player] = keyWithTool[level.player.Tools[2].GetToolType()];
            if (pressedKeys.Contains(Keys.C) && level.IsEnterShop)
            {
                pressedKeys.Remove(Keys.C);
                var shop = new Shop(level);
                shop.Show();
            }
        }

        private Rectangle GetIncrementedByY(Rectangle position, int value) =>
            new Rectangle(new Point(position.X, position.Y + value), position.Size);

        private Rectangle GetDowned(Rectangle position) => GetIncrementedByY(position, yForCoinsAndHearths);
        private void TimerTick(object sender, EventArgs args)
        {
            hintLabel.Text = level.CurrentHintText;
            if (level.player.Tools.Count > 0)
                picture.Image = toolImage[level.player.Tools[0].GetToolType()];
            if (level.player.Tools.Count > 1)
                picture1.Image = toolImage[level.player.Tools[1].GetToolType()];
            if (level.player.Tools.Count > 2)
                picture2.Image = toolImage[level.player.Tools[2].GetToolType()];
            ReadPressedKeys();
            Invalidate(GetDowned(level.player.Position));
            foreach (var coin in level.GetCoins())
                Invalidate(GetDowned(coin));
            foreach (var monster in level.monsters)
                Invalidate(GetDowned(monster.Position));
            level.GameTurn();
            foreach (var monster in level.monsters)
                Invalidate(GetDowned(monster.Position));
            if ((level.IsOver || level.IsWin))
            {
                timer.Stop();
                if (level.IsWin)
                {
                    if (levelBuilder.nextLevel != null)
                    {
                        this.Hide();
                        new GameForm(levelBuilder.nextLevel, null, level.player.Coins, level.player.Lifes).Show();
                    }
                    else
                        Exit(true);
                }
                else
                    Exit(false);
            }
            Invalidate(GetDowned(level.player.Position), true);
            foreach (var monster in level.monsters)
                Invalidate(monster.Position);
            foreach (var coin in level.GetCoins())
                Invalidate(GetDowned(coin));
            Invalidate(level.player.Position, true);
            Invalidate(new Rectangle(0, 10, level.LevelWidth, 35), true);
        }

        private void Exit(bool isWin)
        {
            var exit = new Exit(isWin, levelBuilder, isWin);
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