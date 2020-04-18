using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saharok.Model
{
    public class CoinMagnet
    {
        private int DeltaX { get; }
        private int DeltaY { get; }
        private int MagnetRange { get; }
        private Rectangle previousPlayerPosition;
        private Dictionary<Rectangle, Stack<Rectangle>> coinPathByIndex;
        public CoinMagnet(int deltaX, int deltaY, int magnetRange)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            MagnetRange = magnetRange;
        }
        private IEnumerable<Rectangle> GetNeighbourPositions(Rectangle currentPos)
        {
            for (var dx = -1; dx <= 1; dx++)
                for (var dy = -1; dy <= 1; dy++)
                    if (dy == 0 ^ dx == 0)
                        yield return new Rectangle(new Point(currentPos.X + dx * DeltaX,
                                                             currentPos.Y + dy * DeltaY),
                                                    currentPos.Size);
        }

        private int GetSquaredDistance(Point first, Point second)
        {
            return (first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y);
        }

        private static Point GetCenter(Rectangle r) => new Point((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);

        private Stack<Rectangle> GetPathFromDict(Rectangle? currentPos, Dictionary<Rectangle, Rectangle?> stepBack)
        {
            var path = new Stack<Rectangle>();
            while (stepBack[(Rectangle)currentPos] != null)
            {
                path.Push((Rectangle)currentPos);
                currentPos = stepBack[(Rectangle)currentPos];
            }
            return path;
        }
        private Stack<Rectangle> GetOptimalPath(IEnumerable<Rectangle> walls, Rectangle player, Rectangle coin)
        {
            var queue = new Queue<Rectangle>();
            var stepBack = new Dictionary<Rectangle, Rectangle?>();
            queue.Enqueue(coin);
            stepBack[coin] = null;
            while (queue.Count > 0)
            {
                var currentPos = queue.Dequeue();
                if (currentPos.IntersectsWith(player))
                    return GetPathFromDict(currentPos, stepBack);
                foreach (var neighbour in GetNeighbourPositions(currentPos))
                {
                    if (stepBack.ContainsKey(neighbour) ||
                        walls.Any(w => w.IntersectsWith(neighbour)) ||
                        !IsNear(neighbour, player))
                        continue;
                    stepBack[neighbour] = currentPos;
                    queue.Enqueue(neighbour);
                }
            }
            return null;
        }
        private bool IsNear(Rectangle first, Rectangle second)
        {
            return GetSquaredDistance(GetCenter(first), GetCenter(second)) <= MagnetRange * MagnetRange;
        }

        public void Magnetize(Level level)
        {

            if (level.player.Position != previousPlayerPosition)
            {
                coinPathByIndex = new Dictionary<Rectangle, Stack<Rectangle>>();
                foreach (var coin in level.GetCoins())
                    if (IsNear(coin, level.player.Position))
                        coinPathByIndex[coin] = GetOptimalPath(level.Walls.Select(w => w.Position),
                                                                level.player.Position,
                                                                coin);
            }
            var removedCoins = new List<Rectangle>();
            var addedCoins = new Dictionary<Rectangle, Stack<Rectangle>>();
            foreach (var coin in coinPathByIndex.Keys)
            {
                if (coinPathByIndex[coin].Count > 0)
                {
                    var nextPos = coinPathByIndex[coin].Pop();
                    level.RemoveCoin(coin);
                    level.AddCoin(nextPos);
                    removedCoins.Add(coin);
                    addedCoins[nextPos] = coinPathByIndex[coin];
                }
            }
            foreach (var coin in removedCoins)
                coinPathByIndex.Remove(coin);
            foreach (var coinPathPair in addedCoins)
                coinPathByIndex[coinPathPair.Key] = coinPathPair.Value;
            previousPlayerPosition = level.player.Position;
        }
    }
}
