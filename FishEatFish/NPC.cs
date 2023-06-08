using System;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishEatFish
{
    internal class NPC : Fish
    {
        private static int count = 0;
        private string direction;
        private static readonly Random rnd = new Random();
        private static readonly Dictionary<string, SKBitmap> flippedBitmapCache = new Dictionary<string, SKBitmap>();
        public static int Count => count;

        public NPC(Form1 form, SKBitmap bitmap, SKPoint location, int speed, double level, string direction) : base(form, bitmap, location, speed, level)
        {
            if (level < 99999)
                count++;
            this.direction = direction;
            if (direction == "right")
                this.image = GetFlippedBitmap(bitmap).Copy();

            float width = form.ClientSize.Width * Image.Width / 1536;
            float height = form.ClientSize.Height * Image.Height / 864;
            Bounds.Size = new SKSize(width, height);
        }

        public bool UpdateNPC()
        {
            if (direction == "left")
            {
                Left -= Speed;
                return Left < -Bounds.Width;
            }
            Left += Speed;
            return Left > form.ClientSize.Width;
        }

        public static SKBitmap FlipBitmapHorizontally(SKBitmap bitmap)
        {
            SKBitmap flippedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
            using (SKCanvas canvas = new SKCanvas(flippedBitmap))
            {
                canvas.Clear();
                canvas.Scale(-1, 1, bitmap.Width / 2f, bitmap.Height / 2f);
                canvas.DrawBitmap(bitmap, 0, 0);
            }
            return flippedBitmap;
        }

        private SKBitmap GetFlippedBitmap(SKBitmap originalBitmap)
        {
            string cacheKey = GenerateCacheKey(originalBitmap);
            if (flippedBitmapCache.ContainsKey(cacheKey))
                return flippedBitmapCache[cacheKey];
            else
            {
                SKBitmap flippedBitmap = FlipBitmapHorizontally(originalBitmap);
                flippedBitmapCache.Add(cacheKey, flippedBitmap);
                return flippedBitmap;
            }
        }

        private string GenerateCacheKey(SKBitmap bitmap) => $"{bitmap.Width}_{bitmap.Height}_{rnd.Next(10)}";

        public override void Dispose()
        {
            if (Level < 99999)
                count--;
            base.Dispose();
            direction = null;
        }
    }
}
