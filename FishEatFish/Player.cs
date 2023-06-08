using System;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishEatFish
{
    public class Player : Fish
    {
        private readonly string name; // name of player's fish
        private readonly Keys[] keys = new Keys[4];
        private bool moveLeft, moveRight, moveUp, moveDown;

        public static double MaxLevel = 1;
        public Keys[] ControlKeys => keys;
        public override double Level
        {
            get => base.Level;
            set
            {
                if (image == null || value <= 0)
                    return;

                float k = 1 + (float)Math.Log(value, 3);
                float width = form.ClientSize.Width * 63 / 1536;
                float height = form.ClientSize.Height * 43 / 864;
                Bounds.Size = new SKSize(width * k, height * k);

                base.Level = value;
                if (value > MaxLevel)
                    MaxLevel = value;
            }
        }

        public Player(Form1 form, SKBitmap bitmap, Keys[] keys, SKPoint location, int speed, SKSize size) : base(form, bitmap, location, speed, 1, size)
        {
            this.form.skControl.KeyDown += SkControl_KeyDown;
            this.form.skControl.KeyUp += SkControl_KeyUp;
            this.keys = keys;
            switch (keys[0])
            {
                case Keys.W:
                    name = "Violet";
                    break;
                case Keys.U:
                    name = "Green";
                    break;
                case Keys.Up:
                    name = "Blue";
                    break;
            }
        }

        public void UpdatePlayer()
        {
            if (moveLeft)
            {
                Left -= Speed;
                Image = form.resourceManager.playersImages[$"{name}_left"];
            }
            else if (moveRight)
            {
                Left += Speed;
                Image = form.resourceManager.playersImages[$"{name}_right"];
            }

            if (Bounds.Left < 0)
                Left = 0;
            else if (Bounds.Left > form.ClientSize.Width - Bounds.Width)
                Left = (int)(form.ClientSize.Width - Bounds.Width);

            if (moveUp)
                Top -= Speed;
            else if (moveDown)
                Top += Speed;

            if (Bounds.Top < 0)
                Top = 0;
            else if (Bounds.Top > form.ClientSize.Height - Bounds.Height)
                Top = (int)(form.ClientSize.Height - Bounds.Height);
        }

        private void SkControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == keys[0])
                moveUp = false;
            if (e.KeyCode == keys[1])
                moveLeft = false;
            if (e.KeyCode == keys[2])
                moveDown = false;
            if (e.KeyCode == keys[3])
                moveRight = false;
        }

        private void SkControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == keys[0])
                moveUp = true;
            if (e.KeyCode == keys[1])
                moveLeft = true;
            if (e.KeyCode == keys[2])
                moveDown = true;
            if (e.KeyCode == keys[3])
                moveRight = true;
        }

        public void Kill()
        {
            Bounds.Size = new SKSize(0, 0);
            Bounds.Location = new SKPoint(0, 0);
            Speed = 0;
            Level = 0.1;
        }

        public void UpdatePlayerSize()
        {
            float k = 1 + (float)Math.Log(Level, 3);
            float width = form.ClientSize.Width * 63 / 1536;
            float height = form.ClientSize.Height * 43 / 864;
            Bounds.Size = new SKSize(width * k, height * k);
        }

        public void Revive()
        {
            UpdatePlayerSize();
            form.soundManager.PlaySound("Spawn");
            Bounds.Location = new SKPoint(form.ClientSize.Width / 2, 5);
            Speed = 5;
            Level = 1;
        }
    }
}
