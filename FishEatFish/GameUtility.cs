using System;
using SkiaSharp.Views.Desktop;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishEatFish
{
    internal class GameUtility
    {
        private readonly Form1 form;
        private readonly Random rnd = new Random();
        public Timer volumeTimer = new Timer();
        public MouseEventArgs mouseDown = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);


        public GameUtility(Form1 form)
        {
            this.form = form;
            ToggleFullScreen();
            volumeTimer.Interval = 200;
            volumeTimer.Tick += new EventHandler(VoulumeTimer_Tick);
        }

        public void ToggleFullScreen()
        {
            if (form.uiManager.isFullScreen)
            {
                form.FormBorderStyle = FormBorderStyle.Sizable;
                form.WindowState = FormWindowState.Maximized;

                form.skControl.Size = form.ClientSize;
            }
            else
            {
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Normal;
                form.WindowState = FormWindowState.Maximized;

                // Set the skControl size to match the screen size
                var screen = Screen.PrimaryScreen.Bounds;
                form.skControl.Size = new Size(screen.Width, screen.Height);
            }

            form.uiManager.isFullScreen = !form.uiManager.isFullScreen;
            form.skControl.Invalidate();
        }

        public List<string> FilterStringsByFirstNumber(List<string> arr, ref int number)
        {
            List<string> subarray = new List<string>();
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].StartsWith(number.ToString()) && !char.IsDigit(arr[i][number.ToString().Length]))
                    subarray.Add(arr[i]);
            }

            if (subarray.Count == 0)
            {
                int k = rnd.Next(2) == 0 ? -1 : 1;
                while (subarray.Count == 0)
                {
                    number = number <= 1000 ? number + k : 1;
                    subarray = new List<string>();
                    for (int i = 0; i < arr.Count; i++)
                        if (arr[i].StartsWith(number.ToString()) && !char.IsDigit(arr[i][number.ToString().Length]))
                            subarray.Add(arr[i]);
                }
            }
            return subarray;
        }

        private void VoulumeTimer_Tick(object sender, EventArgs e)
        {
            if (form.resourceManager.npeBounds["Options_plusSFX"].Contains(mouseDown.Location.ToSKPoint()))
                form.soundManager.SfxVolume += 0.01f;

            if (form.resourceManager.npeBounds["Options_minusSFX"].Contains(mouseDown.Location.ToSKPoint()))
                form.soundManager.SfxVolume -= 0.01f;

            if (form.resourceManager.npeBounds["Options_plusMusic"].Contains(mouseDown.Location.ToSKPoint()))
                form.soundManager.MusicVolume += 0.01f;

            if (form.resourceManager.npeBounds["Options_minusMusic"].Contains(mouseDown.Location.ToSKPoint()))
                form.soundManager.MusicVolume -= 0.01f;

            if (volumeTimer.Interval > 40)
                volumeTimer.Interval -= 20;
        }
    }
}
