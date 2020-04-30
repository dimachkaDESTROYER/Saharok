using System.Drawing;
using System.IO;

namespace Saharok.Interface
{
    public static class GameImages
    {
        
        private static readonly DirectoryInfo imagesDirectory = new DirectoryInfo("Image");
        public static Bitmap Coin;
        public static Bitmap CoinMagnet;
        public static Bitmap Boots;
        public static Bitmap Student;


        public static void ImagesForShop()
        {
            foreach (var e in imagesDirectory.GetFiles("*.png"))
            {
                if (e.Name == "монетка.png")
                    Coin = (Bitmap)Image.FromFile(e.FullName);
                if (e.Name == "hat.png")
                    CoinMagnet = (Bitmap)Image.FromFile(e.FullName);
                if (e.Name == "boots.png")
                    Boots = (Bitmap)Image.FromFile(e.FullName);
                if (e.Name == "student.png")
                    Student = (Bitmap)Image.FromFile(e.FullName);
            }
        }
        public static class PlayerImages
        {
            public static Bitmap Simple;
            public static Bitmap WithMagnet;
            public static Bitmap WithBoots;
            public static Bitmap WithStudent;

            public static void ImagesForSugar()
            {
                foreach (var e in imagesDirectory.GetFiles("*.png"))
                {
                    if (e.Name == "withBoots.png")
                        WithBoots = (Bitmap)Image.FromFile(e.FullName);
                    if (e.Name == "withHat.png")
                        WithMagnet = (Bitmap)Image.FromFile(e.FullName);
                    if (e.Name == "withStudent.png")
                        WithStudent = (Bitmap)Image.FromFile(e.FullName);
                    if (e.Name == "simple.png")
                        Simple = (Bitmap)Image.FromFile(e.FullName);
                }
            }
            

            

        }

    }
}