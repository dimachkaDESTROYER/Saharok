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
        public readonly Func<Level, bool> predicate;

        public Hint(Rectangle position, string hintText, Func<Level, bool> predicate = null)
        {
            this.predicate = predicate ?? (l => true);
            this.position = position;
            this.hintText = hintText;
        }
    }
}
