using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok.Model
{
    public enum TypeTool
    {
        Magnit,
        Boot,
        Student,
    }

    public interface ITool
    {
        string GetName();
        int GetPrise();
        void DoAction(Level level);
        TypeTool GetToolType();
    }
    class Magnit : ITool
    {

        private CoinMagnet coinMagnet;

        public Magnit()
        {
            coinMagnet = new CoinMagnet(10, 10, 200);
        }
        public void DoAction(Level level)
        {
            coinMagnet.Magnetize(level);
        }

        public string GetName()
        {
            return "Шапка - магнит";
        }

        public TypeTool GetToolType()
        {
            return TypeTool.Magnit;
        }

        public int GetPrise()
        {
            return 5;
        }
    }

    class Student : ITool
    {
        public void DoAction(Level level)
        {
            level.player.IsStudent = true;
        }

        public string GetName()
        {
            return "Студенческий - спасение от монстров";
        }

        public int GetPrise()
        {
            return 10;
        }

        public TypeTool GetToolType()
        {
            return TypeTool.Student;
        }
    }

    class Boots : ITool
    {
        public void DoAction(Level level)
        {
            level.player.Up(80);
        }

        public string GetName()
        {
            return "Ботинки - прыгунки";
        }

        public int GetPrise()
        {
            return 5;
        }

        public TypeTool GetToolType()
        {
            return TypeTool.Boot;
        }
    }
}
