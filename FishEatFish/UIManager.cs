using System;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishEatFish
{
    internal class UIManager
    {
        private readonly Form1 form;
        private SKCanvas canvas;
        public bool isFullScreen;
        public bool isMenuVisible;
        public bool isOptionsVisible;

        public UIManager(Form1 form)
        {
            this.form = form;
            form.skControl.PaintSurface += SkControl_PaintSurface;
        }

        private void SkControl_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();
            form.uiManager.canvas = canvas;

            form.uiManager.DrawBackground();
            form.uiManager.DrawPlayerBars(form.gameplay.players);

            foreach (NPC fish in form.managerNPC.CreatedFishes)
                canvas.DrawBitmap(fish.Image, fish.Bounds);

            foreach (Player player in form.gameplay.players)
                if (player.Level >= 1)
                    canvas.DrawBitmap(player.Image, player.Bounds);

            form.uiManager.DrawMenu();
        }

        public void DrawPlayerBars(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                SKBitmap bar = form.resourceManager.npe[$"{i}Bar"];
                SKRect bounds = bar.Info.Rect;
                bounds.Inflate(-bounds.Width / 3, -bounds.Height / 3);
                bounds.Location = new SKPoint(5 + i * form.ClientSize.Width / 3, 5);
                canvas.DrawBitmap(bar, bounds);
                SKPaint textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 24f,
                    TextAlign = SKTextAlign.Left,
                    IsAntialias = true
                };
                float x = bounds.Left + (float)(bounds.Width / 2.35);
                float y = bounds.Top + (float)(bounds.Height / 1.56);

                if (players[i].Level >= 1)
                    canvas.DrawText($"{Math.Round(players[i].Level, 2) * 100 - 100}", x, y, textPaint);
                else
                {
                    textPaint.TextSize = 20f;
                    y -= 2;
                    canvas.DrawText($"Press {players[i].ControlKeys[0]}", x, y, textPaint);
                }
            }
        }

        public void DrawBackground()
        {
            SKRect backBounds = new SKRect(0, 0, form.ClientSize.Width, form.ClientSize.Height);
            canvas.DrawBitmap(form.resourceManager.npe["landscape"], backBounds);
        }

        public void DrawMenu()
        {
            // common temp rectangle
            SKRect bounds = new SKRect();
            // pause icon in top right corner
            SKBitmap icon = form.resourceManager.npe["pause"];
            bounds = icon.Info.Rect;
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.035), (float)(form.ClientSize.Width * 0.035));
            bounds.Location = new SKPoint(form.ClientSize.Width - bounds.Width - 5, 5);
            canvas.DrawBitmap(icon, bounds);
            form.resourceManager.npeBounds["pause"] = bounds;
            // If menu isn`t visible you don`t need to draw it
            if (!isMenuVisible && !isOptionsVisible)
                return;
            // make non-menu elements darker
            SKPaint overlayPaint = new SKPaint
            {
                Color = new SKColor(0, 0, 0, 150),
                BlendMode = SKBlendMode.Multiply
            };
            canvas.DrawRect(new SKRect(0, 0, form.ClientSize.Width, form.ClientSize.Height), overlayPaint);
            canvas.Restore();
            // If options are visible then they are need to be drawn and other elements aren't
            if (isOptionsVisible)
            {
                DrawOptions();
                return;
            }
            // Title
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.46), (float)(form.ClientSize.Height * 0.2));
            bounds.Location = new SKPoint(form.Right / 2 - bounds.Width / 2, 5);
            form.resourceManager.npeBounds["Menu_Paused"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Menu_Paused"], bounds);
            // Start of buttons range from title bounds plus a little space to end of canvas minus 5 px
            float start = bounds.Top + 40;
            float distance = ((float)(form.ClientSize.Height - form.ClientSize.Height * 0.15) - start - 5) / 4;
            // Play
            bounds.Location = new SKPoint(5, start + distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.3), (float)(form.ClientSize.Height * 0.15));
            form.resourceManager.npeBounds["Menu_play"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Menu_play"], bounds);
            // Restart
            bounds.Location = new SKPoint(5, start + 2 * distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.53), (float)(form.ClientSize.Height * 0.15));
            form.resourceManager.npeBounds["Menu_restart"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Menu_restart"], bounds);
            // Options
            bounds.Location = new SKPoint(5, start + 3 * distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.51), (float)(form.ClientSize.Height * 0.15));
            form.resourceManager.npeBounds["Menu_options"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Menu_options"], bounds);
            // Quit
            bounds.Location = new SKPoint(5, start + 4 * distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.27), (float)(form.ClientSize.Height * 0.15));
            form.resourceManager.npeBounds["Menu_quit"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Menu_quit"], bounds);
        }

        private void DrawOptions()
        {
            // common rectangle for option buttons
            SKRect bounds = new SKRect();
            // arrow back
            bounds.Location = new SKPoint(5, 15);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.135), (float)(form.ClientSize.Height * 0.12));
            form.resourceManager.npeBounds["Options_back"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Options_back"], bounds);
            // title
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.51), (float)(form.ClientSize.Height * 0.15));
            bounds.Location = new SKPoint(form.Right / 2 - bounds.Width / 2, 5);
            canvas.DrawBitmap(form.resourceManager.npe["Options_options"], bounds);
            // Start of buttons range from title bounds plus a little space to end of canvas minus 5 px
            float start = bounds.Top + 40;
            float distance = ((float)(form.ClientSize.Height - form.ClientSize.Height * 0.15) - start - 5) / 3;
            // Sfx
            bounds.Location = new SKPoint(5, start + distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.21), (float)(form.ClientSize.Height * 0.15));
            canvas.DrawBitmap(form.resourceManager.npe["Options_sfx"], bounds);

            bounds.Location = new SKPoint(bounds.Right + (float)(form.ClientSize.Width * 0.1), bounds.Top);//////////
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.09), (float)(form.ClientSize.Height * 0.15));
            form.resourceManager.npeBounds["Options_plusSFX"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Options_plusSFX"], bounds);

            DrawNumber((int)(form.soundManager.SfxVolume * 100), bounds.Right, bounds.Right + (float)(form.ClientSize.Width * 0.19), bounds.Top);

            bounds.Location = new SKPoint(bounds.Right + (float)(form.ClientSize.Width * 0.19), bounds.MidY - (float)(form.ClientSize.Height * 0.027));
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.086), (float)(form.ClientSize.Height * 0.055));
            form.resourceManager.npeBounds["Options_minusSFX"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Options_minusSFX"], bounds);
            // Music
            bounds.Location = new SKPoint(5, start + 2 * distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.35), (float)(form.ClientSize.Height * 0.15));
            canvas.DrawBitmap(form.resourceManager.npe["Options_music"], bounds);

            bounds.Location = new SKPoint(bounds.Right + (float)(form.ClientSize.Width * 0.1), bounds.Top);////////
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.09), (float)(form.ClientSize.Height * 0.15));
            form.resourceManager.npeBounds["Options_plusMusic"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Options_plusMusic"], bounds);

            DrawNumber((int)(form.soundManager.MusicVolume * 100), bounds.Right, bounds.Right + (float)(form.ClientSize.Width * 0.19), bounds.Top);

            bounds.Location = new SKPoint(bounds.Right + (float)(form.ClientSize.Width * 0.19), bounds.MidY - (float)(form.ClientSize.Height * 0.027));
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.086), (float)(form.ClientSize.Height * 0.055));
            form.resourceManager.npeBounds["Options_minusMusic"] = bounds;
            canvas.DrawBitmap(form.resourceManager.npe["Options_minusMusic"], bounds);
            // Full Screen
            bounds.Location = new SKPoint(5, start + 3 * distance);
            bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.71), (float)(form.ClientSize.Height * 0.15));
            canvas.DrawBitmap(form.resourceManager.npe["Options_fullScreen"], bounds);

            if (isFullScreen)
            {
                bounds.Location = new SKPoint(bounds.Right + (float)(form.ClientSize.Width * 0.08), bounds.Top);
                bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.16), (float)(form.ClientSize.Height * 0.15));
                form.resourceManager.npeBounds["Options_on"] = bounds;
                form.resourceManager.npeBounds["Options_off"] = new SKRect();
                canvas.DrawBitmap(form.resourceManager.npe["Options_on"], bounds);
            }
            else
            {
                bounds.Location = new SKPoint(bounds.Right + (float)(form.ClientSize.Width * 0.08), bounds.Top);
                bounds.Size = new SKSize((float)(form.ClientSize.Width * 0.2), (float)(form.ClientSize.Height * 0.15));
                form.resourceManager.npeBounds["Options_off"] = bounds;
                form.resourceManager.npeBounds["Options_on"] = new SKRect();
                canvas.DrawBitmap(form.resourceManager.npe["Options_off"], bounds);
            }
        }

        private void DrawNumber(int number, float x1, float x2, float y)
        {
            float areaWidth = x2 - x1;

            float totalDigitsWidth = 0;
            foreach (char digitChar in number.ToString())
            {
                int digit = int.Parse(digitChar.ToString());

                if (digit >= 0 && digit < form.resourceManager.digitImages.Length)
                {
                    SKBitmap digitImage = form.resourceManager.digitImages[digit];
                    totalDigitsWidth += form.ClientSize.Width * digitImage.Width / 1536;
                }
            }

            float startX = x1 + (areaWidth - totalDigitsWidth) / 2;

            foreach (char digitChar in number.ToString())
            {
                if (char.IsDigit(digitChar))
                {
                    int digit = int.Parse(digitChar.ToString());

                    if (digit >= 0 && digit < form.resourceManager.digitImages.Length)
                    {
                        SKBitmap digitImage = form.resourceManager.digitImages[digit];
                        SKRect destRect = SKRect.Create(startX, y, form.ClientSize.Width * digitImage.Width / 1536, form.ClientSize.Height * digitImage.Height / 864);
                        canvas.DrawBitmap(digitImage, destRect);

                        startX += form.ClientSize.Width * digitImage.Width / 1536;
                    }
                }
            }
        }
    }
}
