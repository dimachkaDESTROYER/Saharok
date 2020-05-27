using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Saharok.Model
{
    public class Level
    {
        public bool IsOver { get; private set; }
        public bool IsWin { get; private set; }
        public readonly LevelBuilder nextLevel; 
        private int gravityForce;
        public readonly int LevelHeight;
        public readonly int LevelWidth;
        public readonly GameCell[] Walls;
        private GameCell[] Lava;
        private Dictionary<Rectangle, int> Coins;
        public Player player;
        public List<Monster> monsters;
        public Rectangle finish;
        public Rectangle shop;
        private List<Hint> hints;
        public string CurrentHintText = "";
        public bool NeedToChangeHint { get; set; }
        public bool IsEnterShop { get; private set; }

        public Level(int LevelHeight, int LevelWidth,
            IEnumerable<Rectangle> walls, IEnumerable<Rectangle> coins, IEnumerable<Rectangle> lava,
            int gForce, Player player, IEnumerable<Monster> monsters, Rectangle finish, Rectangle shop, LevelBuilder nextLevel, List<Hint> hints = null)
        {
            this.hints = hints ?? new List<Hint>();
            IsOver = false;
            NeedToChangeHint = false;
            this.LevelHeight = LevelHeight;
            this.LevelWidth = LevelWidth;
            Walls = walls.Select(r => new GameCell(CellType.Wall, r)).ToArray();
            Coins = new Dictionary<Rectangle, int>();
            foreach (var coin in coins)
                Coins[coin] = 1;
            Lava = lava.Select(r => new GameCell(CellType.Lava, r)).ToArray();
            gravityForce = gForce;
            this.player = player;
            this.monsters = monsters.ToList();
            this.finish = finish;
            this.shop = shop;
            this.nextLevel = nextLevel;
        }

        private void Move(Axis axis)
        {
            player.ChangePosition(axis);
            var dx = 0;
            var dy = 0;
            foreach (var wall in Walls.Select(w => w.Position))
            {
                if (!player.Position.IntersectsWith(wall)) continue;
                if (axis == Axis.Horisontal)
                {
                    if (player.SpeedX > 0)
                        dx = wall.Left - player.Position.Right;
                    else if (player.SpeedX < 0)
                        dx = wall.Right - player.Position.Left;
                }
                else
                {
                    if (player.SpeedY > 0)
                    {
                        player.OnGround = true;
                        dy = wall.Top - player.Position.Bottom;
                    }
                    else if (player.SpeedY < 0)
                        dy = wall.Bottom - player.Position.Top;
                }
            }

            player.SpeedX = 0;
            if (dy != 0)
                player.SpeedY = 0;
            player.ChangePosition(dx, dy);
        }

        private void Move()
        {
            if (player.SpeedX != 0)
                Move(Axis.Horisontal);
            if (player.SpeedY != 0)
                Move(Axis.Vertical);
            foreach (var monster in monsters)
                monster.ChangePosition();
        }

        public void GameTurn()
        {
            player.CurrentTool?.DoAction(this);
            if (player.SpeedY < gravityForce)
                player.Down(gravityForce);
            Move();
            var removed = new List<Rectangle>();
            foreach (var coin in GetCoins().Where(c => c.IntersectsWith(player.Position)).ToList())
            {
                removed.Add(coin);
                if (Coins.ContainsKey(coin))
                    player.AddCoins(Coins[coin]);
                Coins.Remove(coin);
            }

            foreach (var coin in removed)
                Coins.Remove(coin);
            foreach (var water in Lava.Where(c => c.Position.IntersectsWith(player.Position)))
                player.Conflict();
            foreach (var monster in monsters.Where(m => m.Position.IntersectsWith(player.Position)))
            {
                if (!player.IsStudent)
                    player.Conflict();
            }
            foreach (var coin in removed)
                Coins.Remove(coin);
            if (player.Position.Bottom > LevelHeight)
                player.Conflict();
            if (player.Lifes <= 0)
                IsOver = true;

            foreach (var hint in hints.Where(hint => hint.position.IntersectsWith(player.Position)))
            {
                if (hint.hintText != CurrentHintText)
                {
                    CurrentHintText = hint.hintText;
                    NeedToChangeHint = true;
                }

                break;
            }

            IsEnterShop = player.Position.IntersectsWith(shop);
            if (!player.Position.IntersectsWith(finish)) return;
            IsOver = true;
            IsWin = true;
            
        }

        public IEnumerable<Rectangle> GetCoins()
        {
            return Coins.Keys;
        }

        public void AddCoin(Rectangle coin)
        {
            if (!Coins.ContainsKey(coin))
                Coins[coin] = 0;
            Coins[coin]++;
        }

        public void RemoveCoin(Rectangle coin)
        {
            Coins[coin]--;
            if (Coins[coin] <= 0)
                Coins.Remove(coin);
        }

        public IEnumerable<Sprite> GetSprites()
        {
            yield return new Sprite(player.Position, SpriteType.Player);
            foreach (var monster in monsters)
                yield return new Sprite(monster.Position, SpriteType.Monster);

            foreach (var coin in Coins)
                yield return new Sprite(coin.Key, SpriteType.Coin);
        }
    }
}