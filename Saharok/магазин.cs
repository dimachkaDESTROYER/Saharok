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
    public partial class Shop : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Image ImageGun;
        private readonly Image imageHat;
        private readonly Image ImageStudent;
        private readonly Image ImageSugar;
        private readonly Image CoinImage;
        private readonly Level level;

        public Shop(Level level, DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();

            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Image");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            ImageGun = bitmaps["gun.png"];
            imageHat = bitmaps["Shapka.png"];
            ImageStudent = bitmaps["студик.png"];
            ImageSugar = bitmaps["улыбка.png"];
            CoinImage = bitmaps["монетка.png"];
            this.level = level;
            //var timer = new Timer();
            //timer.Interval = 10;
            //timer.Tick += TimerTick;
            //timer.Start();

            var buttonBuy = new Button()
            {
                Text = "Купить",
                Font = new Font("AlaskaC", 15),
                Dock = DockStyle.Fill
            };

            var buttonBack = new Button()
            {
                Text = "Назад",
                Font = new Font("AlaskaC", 15),
                Dock = DockStyle.Fill
            };

            var gun = new Button()
            {

                Image = ImageGun,
                Dock = DockStyle.Fill
            };

            var hat = new Button()
            {
                Image = imageHat,
                Dock = DockStyle.Fill
            };

            var student = new Button()
            {
                Image = ImageStudent,
                Dock = DockStyle.Fill
            };

            //var textBox = new TextBox
            //{
            //    Text = "Выберите покупку",
            //    Dock = DockStyle.Fill

            //};




            var table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));


            table.Controls.Add(buttonBuy, 1, 3);
            table.Controls.Add(buttonBack, 3, 3);
            table.Controls.Add(gun, 1, 1);
            table.Controls.Add(student, 2, 1);
            table.Controls.Add(hat, 3, 1);

            //table.SetColumnSpan(textBox, 3);
            //table.Controls.Add(textBox, 1, 0);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);
            BackgroundImage = ImageSugar;

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(ImageSugar, 0, 0, level.LevelWidth, level.LevelHeight);
            //e.Graphics.DrawImage(ImageSugar, 0, 0, 100, 100);
            e.Graphics.DrawString(level.player.Coins.ToString(), new Font("Arial", 30), Brushes.Black, (float)(0.46 * level.LevelWidth), 5);
            e.Graphics.DrawImage(CoinImage, new Point((int)(0.3 * level.LevelWidth), 0));
            e.Graphics.DrawString("Выберите покупку", new Font("Arial", 30), Brushes.Black, (float)(0.51 * this.ClientSize.Width), 5);

        }
        //private void TimerTick(object sender, EventArgs args)
        //{
        //    level.GameTurn();
        //    if (level.IsOver)
        //    {
        //        this.Hide();
        //    }

        //    Invalidate();
        //}
    }
}