using System.Drawing;
using System.IO;

namespace Saharok.Interface
{
    public static class GameImages
    {
        
        private static readonly DirectoryInfo ImagesDirectory = new DirectoryInfo("Image");
        public static Bitmap Coin;
        public static Bitmap CoinMagnet;
        public static Bitmap Boots;
        public static Bitmap Student;
        public static Bitmap backgroung;

        public static void ImagesForShop()
        {
            foreach (var e in ImagesDirectory.GetFiles("*.png"))
            {
                switch (e.Name)
                {
                    case "монетка.png":
                        Coin = (Bitmap)Image.FromFile(e.FullName);
                        break;
                    case "hat.png":
                        CoinMagnet = (Bitmap)Image.FromFile(e.FullName);
                        break;
                    case "boots.png":
                        Boots = (Bitmap)Image.FromFile(e.FullName);
                        break;
                    case "student.png":
                        Student = (Bitmap)Image.FromFile(e.FullName);
                        break;
                }
            }
        }

        public static void ImageBackGround()
        {
            foreach (var e in ImagesDirectory.GetFiles("*.png"))
            {
                switch (e.Name)
                {
                    case "start.png":
                        backgroung = (Bitmap)Image.FromFile(e.FullName);
                        break;
                }
            }
        }

        public static class PlayerImages
        {
            public static Bitmap Simple;
            public static Bitmap Simplefon;
            public static Bitmap WithMagnet;
            public static Bitmap WithBoots;
            public static Bitmap WithStudent;

            public static void ImagesForSugar()
            {
                foreach (var e in ImagesDirectory.GetFiles("*.png"))
                {
                    switch (e.Name)
                    {
                        case "withBoots.png":
                            WithBoots = (Bitmap)Image.FromFile(e.FullName);
                            break;
                        case "withHat.png":
                            WithMagnet = (Bitmap)Image.FromFile(e.FullName);
                            break;
                        case "withStudent.png":
                            WithStudent = (Bitmap)Image.FromFile(e.FullName);
                            break;
                        case "simple.png":
                            Simple = (Bitmap)Image.FromFile(e.FullName);
                            break;
                        case "simplefon.png":
                            Simplefon = (Bitmap)Image.FromFile(e.FullName);
                            break;
                    }
                }
            }     
        }

    }
}