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
            return "Magnit";
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
}
