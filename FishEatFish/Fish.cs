using System;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishEatFish
{
    public class Fish
    {
        protected readonly Form1 form;
        protected SKBitmap image;

        public SKBitmap Image
        {
            get { return image; }
            set { image = value; }
        }
        public SKRect Bounds;
        public virtual double Level { get; set; }
        public int Speed { get; set; }
        public float Left
        {
            get { return Bounds.Location.X; }
            set { Bounds.Location = new SKPoint(value, Bounds.Location.Y); }
        }
        public float Top
        {
            get { return Bounds.Location.Y; }
            set { Bounds.Location = new SKPoint(Bounds.Location.X, value); }
        }

        public Fish(Form1 form, SKBitmap image, SKPoint location, int speed, double level)
        {
            this.form = form;
            this.image = image;
            Speed = speed;
            Level = level;
            Bounds = this.image.Info.Rect;
            Left = location.X; Top = location.Y;
        }
        public Fish(Form1 form, SKBitmap image, SKPoint location, int speed, double level, SKSize size) : this(form, image, location, speed, level)
        {
            Bounds = this.image.Info.Rect;
            Bounds.Size = size;
            Left = location.X; Top = location.Y;
        }

        public virtual void Dispose()
        {
            image?.Dispose();
            Speed = 0;
            Level = 0.1;
            Left = 0; Top = 0;
        }
    }
}
