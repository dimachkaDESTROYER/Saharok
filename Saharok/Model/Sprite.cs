using System.Drawing;
using System.Windows.Forms;

namespace Saharok.Model
{
    public enum SpriteType
    {
        Monster, Coin, Player
    }

    public class Sprite
    {
        public SpriteType Type { get; }
        public Rectangle Position { get; }

        public Sprite(Rectangle position, SpriteType type)
        {
            Position = position;
            Type = type;
        }
    }
}