namespace Saharok.Model
{
    public enum TypeTool
    {
        Magnet,
        Boot,
        Student,
    }

    public interface ITool
    {
        string GetName();
        int GetPrice();
        void DoAction(Level level);
        TypeTool GetToolType();
    }
    class Magnet : ITool
    {

        private CoinMagnet coinMagnet;

        public Magnet()
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
            return TypeTool.Magnet;
        }

        public int GetPrice()
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

        public int GetPrice()
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

        public int GetPrice()
        {
            return 5;
        }

        public TypeTool GetToolType()
        {
            return TypeTool.Boot;
        }
    }
}
