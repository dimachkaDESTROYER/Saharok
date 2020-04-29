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
    public partial class Shop : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Image ImageGun;
        private readonly Image imageHat;
        private readonly Image ImageStudent;
        private readonly Image ImageSugar;
        private readonly Image CoinImage;
        private readonly Level level;
        private ITool current;

        public Shop(Level level, bool inicialise, DirectoryInfo imagesDirectory = null)
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

            var buttonBuy = new Button()
            {
                Text = "Купить",
                Font = new Font("AlaskaC", 15),
                Dock = DockStyle.Fill
            };
            buttonBuy.Click += (sender, args) =>
            {
                if (current != null && level.player.TryRemoveCoins(current.GetPrise()))
                {
                    level.player.Tools.Add(current);
                }
            };

            var buttonBack = new Button()
            {
                Text = "Назад",
                Font = new Font("AlaskaC", 15),
                Dock = DockStyle.Fill,
                BackColor = GameColors.ButtonColor,
            };

            buttonBack.Click += (sender, args) =>
            {
                
                this.Hide();
                inicialise = false;
                
            };

            var textBox = new TextBox
            {
                Text = "Выберите покупку",
                Font = new Font("AlaskaC", 15),
                Dock = DockStyle.Fill,
                BackColor = GameColors.ButtonColor,
            };

            var gun = new Button()
            {
                Image = ImageGun,
                Dock = DockStyle.Fill,
                BackColor = GameColors.ButtonColor,
            };

            var hat = new Button()
            {
                Image = imageHat,
                Dock = DockStyle.Fill,
                BackColor = GameColors.ButtonColor,
            };

            hat.Click += (sender, args) =>
            {
                current = new Magnit();
                textBox.Text = current.GetName() + "; Цена - " + current.GetPrise().ToString();
                
            };

            var student = new Button()
            {
                Image = ImageStudent,
                Dock = DockStyle.Fill
            };

            

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

            table.SetColumnSpan(textBox, 3);
            table.Controls.Add(textBox, 1, 0);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);
            BackgroundImage = ImageSugar;

        }
    }
}