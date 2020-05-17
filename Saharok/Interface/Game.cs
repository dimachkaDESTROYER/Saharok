
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
        private Timer timer;
        private Dictionary<Keys, Dictionary<TypeTool, Bitmap>> keyWithTool;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private List<Tuple<Bitmap, Rectangle>> toDraw = new List<Tuple<Bitmap, Rectangle>>();
        private Dictionary<CellType, SolidBrush> brushesOfCells = new Dictionary<CellType, SolidBrush>();
        private int yForCoinsAndHearths;
        private int xForCoinsAndHearths;
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

            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Image");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            yForCoinsAndHearths = bitmaps[LifeImage].Height + 10;
            //TranslateTransform
            xForCoinsAndHearths = bitmaps[LifeImage].Width;
            ClientSize = new Size(
                this.level.LevelWidth,
                yForCoinsAndHearths + this.level.LevelHeight);

            keyWithTool = new Dictionary<Keys, Dictionary<TypeTool, Bitmap>>();
            keyWithTool[Keys.M] = new Dictionary<TypeTool, Bitmap>() { { TypeTool.Magnet, GameImages.PlayerImages.WithMagnet } };
            keyWithTool[Keys.S] = new Dictionary<TypeTool, Bitmap>() { { TypeTool.Student, GameImages.PlayerImages.WithStudent } };
            keyWithTool[Keys.B] = new Dictionary<TypeTool, Bitmap>() { { TypeTool.Boot, GameImages.PlayerImages.WithBoots } };

            brushesOfCells[CellType.Lava] = new SolidBrush(GameColors.LavaColor);
            brushesOfCells[CellType.Wall] = new SolidBrush(GameColors.WallColor);

            BackgroundImage = GetBackgroundImage(levelBuilder);

            timer = new Timer();
            timer.Interval = 40;
            timer.Tick += TimerTick;
            timer.Start();
        }

        private Rectangle GetIncrementedByY(Rectangle Position, int value) =>
        new Rectangle(new Point(Position.X, Position.Y + value), Position.Size);
        private Rectangle GetDowned(Rectangle Position) => GetIncrementedByY(Position, yForCoinsAndHearths);
        private Bitmap GetBackgroundImage(LevelBuilder builder)
        {
            var bitmap = new Bitmap(builder.Width, builder.Height + yForCoinsAndHearths);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(GameColors.BackgroundColor), new Rectangle(0, 0,
                                                                builder.Width, builder.Height + yForCoinsAndHearths));
            foreach (var gameCell in builder.GetCells())
                g.FillRectangle(brushesOfCells[gameCell.Type],
                            GetDowned(gameCell.Position));
            g.DrawImage(bitmaps[finish], GetDowned(level.finish));
            g.DrawImage(bitmaps[CoinImage], level.LevelWidth - xForCoinsAndHearths * 4, 5);
            g.DrawImage(bitmaps[LifeImage], level.LevelWidth - xForCoinsAndHearths * 2, 5);
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
            
            //for (var i = 0; i < level.player.Tools.Count; i++)
            //{
            //    var stro = level.player.Tools[i].GetFileName();
            //    e.Graphics.DrawImage(bitmaps[stro], i * 20, 5);
            //    e.Graphics.FillRectangle(new SolidBrush(Color.Red), new Rectangle(10, 10, 300, 300));
            //}
            
            e.Graphics.DrawString(level.player.Lifes.ToString(), fontForMoneyAndLifes, Brushes.Black, (float)(level.LevelWidth - xForCoinsAndHearths), 5);
            e.Graphics.DrawString(level.player.Coins.ToString(), fontForMoneyAndLifes, Brushes.Black, (float)(level.LevelWidth - 3 * xForCoinsAndHearths), 5);
            foreach (var imageRectPair in toDraw)
                e.Graphics.DrawImage(imageRectPair.Item1, GetDowned(imageRectPair.Item2));
            
            
        }

        private void ReadPressedKeys()
        {

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
            toDraw = new List<Tuple<Bitmap, Rectangle>>();//GetSprites вместо этого 

            foreach (var coin in level.GetCoins())
                Invalidate(GetDowned(coin));

            ReadPressedKeys();
            Invalidate(GetDowned(level.player.Position));
            foreach (var monster in level.monsters)
                Invalidate(GetDowned(monster.Position));
            level.GameTurn();
            foreach (var coin in level.GetCoins())
                toDraw.Add(Tuple.Create(bitmaps[CoinImage], coin));
            if ((level.IsOver || level.IsWin))
            {
                timer.Stop();
                if (level.isFinal && level.IsWin)
                    Exit(level.IsWin);
                if (level.IsOver)
                    Exit(level.IsWin);
                else
                {
                    
                    var player = new Player(new Rectangle(25, 450, 50, 50), 2);
                    var monster = new Monster[] { new Monster(320, 560, MovingDirection.Right, new Rectangle(380, 500, 50, 50)) };
                                              
                    var fin = new Rectangle(150, 480, 100, 75);
                    var l = new LevelBuilder(1024, 600, fin, true).AddPlayer(player).AddMonsters(monster)
                                                   .AddWalls(new Rectangle(0, 550, 160, 50),
                                                             new Rectangle(40, 70, 160, 25))
                                                   .AddLava(new Rectangle(180, 550, 160, 50),
                                                             new Rectangle(640, 550, 480, 50));
                    this.Hide();
                    new GameForm(l).Show();
                }
            }
            foreach (var coin in level.GetCoins())
                toDraw.Add(Tuple.Create(bitmaps[CoinImage], coin));
            foreach (var monster in level.monsters)
                Invalidate(GetDowned(monster.Position));
            foreach (var monster in level.monsters)
                toDraw.Add(Tuple.Create(bitmaps[MonsterImage], monster.Position));

            toDraw.Add(Tuple.Create(PlayerImage, level.player.Position));

            Invalidate(GetDowned(level.player.Position), true);
        }

        private void Exit(bool isWin)
        {
            var exit = new Exit(isWin, levelBuilder);
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