using Saharok.Interface;
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
        
        
        private readonly Level level;
        private ITool current;

        public Shop(Level level, DirectoryInfo imagesDirectory = null)
        {
            BackColor = GameColors.BackgroundColor;
            InitializeComponent();

            GameImages.ImagesForShop();
            
            this.level = level;

            var buttonBuy = new Button()
            {
                Text = "Купить",
                Font = new Font("Roboto", 15),
                Dock = DockStyle.Fill,
                BackColor = GameColors.ButtonColor,

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
                Font = new Font("Roboto", 15),
                Dock = DockStyle.Fill,
                BackColor = GameColors.ButtonColor,
            };

            buttonBack.Click += (sender, args) =>
            {
                
                this.Hide();
            };

            var textBox = new Label()
            {
                Text = "Выберите покупку",
                Font = new Font("Roboto", 25),
                Dock = DockStyle.Fill,
                BackColor = GameColors.TextShopBackColor,
            };

            var boots = new Button()
            {
                Image = GameImages.Boots,
                Dock = DockStyle.Fill,
            };
            boots.Click += (sender, args) =>
            {
                current = new Boots();
                textBox.Text = current.GetName() + "; Цена - " + current.GetPrise().ToString();
            };

            var hat = new Button()
            {
                Image = GameImages.CoinMagnet,
                Dock = DockStyle.Fill,
            };

            hat.Click += (sender, args) =>
            {
                current = new Magnit();
                textBox.Text = current.GetName() + "; Цена - " + current.GetPrise().ToString();
            };

            var student = new Button()
            {
                Image = GameImages.Student,
                Dock = DockStyle.Fill
            };
            student.Click += (sender, args) =>
            {
                current = new Student();
                textBox.Text = current.GetName() + "; Цена - " + current.GetPrise().ToString();
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
            table.Controls.Add(boots, 1, 1);
            table.Controls.Add(student, 2, 1);
            table.Controls.Add(hat, 3, 1);

            table.SetColumnSpan(textBox, 3);
            table.Controls.Add(textBox, 1, 0);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);
            

        }
    }
}