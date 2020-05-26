using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok.Model
{
    public class Hint
    {
        public readonly Rectangle position;
        public readonly string hintText;

        public Hint(Rectangle position, string hintText)
        {
            this.position = position;
            this.hintText = hintText;
        }
    }
}
