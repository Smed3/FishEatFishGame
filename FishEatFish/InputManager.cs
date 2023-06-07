using System;
using SkiaSharp.Views.Desktop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishEatFish
{
    internal class InputManager
    {
        private Form1 form;
        public InputManager(Form1 form)
        {
            this.form = form;
            this.form.skControl.KeyUp += SkControl_KeyUp;
            this.form.skControl.MouseClick += SkControl_MouseClick;
            this.form.skControl.MouseDown += SkControl_MouseDown;
            this.form.skControl.MouseUp += SkControl_MouseUp;
        }

        private void SkControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                    form.gameUtility.ToggleFullScreen();
                    break;
                case Keys.Escape:
                    if (form.uiManager.isOptionsVisible)
                    {
                        form.uiManager.isOptionsVisible = false;
                        form.uiManager.isMenuVisible = true;
                    }
                    else
                        form.uiManager.isMenuVisible = !form.uiManager.isMenuVisible;
                    break;
            }
        }

        private void SkControl_MouseDown(object sender, MouseEventArgs e)
        {
            form.gameUtility.mouseDown = e;
            form.gameUtility.volumeTimer.Start();
        }

        private void SkControl_MouseUp(object sender, MouseEventArgs e)
        {
            form.gameUtility.volumeTimer.Interval = 200;
            form.gameUtility.volumeTimer.Stop();
            form.gameUtility.mouseDown = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);

            if (form.resourceManager.npeBounds["Options_plusSFX"].Contains(e.Location.ToSKPoint()))
                form.soundManager.SfxVolume += 0.01f;

            if (form.resourceManager.npeBounds["Options_minusSFX"].Contains(e.Location.ToSKPoint()))
                form.soundManager.SfxVolume -= 0.01f;

            if (form.resourceManager.npeBounds["Options_plusMusic"].Contains(e.Location.ToSKPoint()))
                form.soundManager.MusicVolume += 0.01f;

            if (form.resourceManager.npeBounds["Options_minusMusic"].Contains(e.Location.ToSKPoint()))
                form.soundManager.MusicVolume -= 0.01f;
        }

        private void SkControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (form.resourceManager.npeBounds["pause"].Contains(e.Location.ToSKPoint()))
            {
                form.uiManager.isMenuVisible = true;
            }

            if (form.uiManager.isMenuVisible)
            {
                if (form.resourceManager.npeBounds["Menu_play"].Contains(e.Location.ToSKPoint()))
                    form.uiManager.isMenuVisible = false;

                else if (form.resourceManager.npeBounds["Menu_restart"].Contains(e.Location.ToSKPoint()))
                {
                    form.gameplay.RestartGame();
                    form.uiManager.isMenuVisible = false;
                }

                else if (form.resourceManager.npeBounds["Menu_options"].Contains(e.Location.ToSKPoint()))
                {
                    form.uiManager.isOptionsVisible = true;
                    form.uiManager.isMenuVisible = false;
                }

                else if (form.resourceManager.npeBounds["Menu_quit"].Contains(e.Location.ToSKPoint()))
                {
                    form.Close();
                }
            }
            else if (form.uiManager.isOptionsVisible)
            {
                if (form.resourceManager.npeBounds["Options_back"].Contains(e.Location.ToSKPoint()))
                {
                    form.uiManager.isOptionsVisible = false;
                    form.uiManager.isMenuVisible = true;
                }

                if (form.resourceManager.npeBounds["Options_off"].Contains(e.Location.ToSKPoint()))
                {
                    form.uiManager.isFullScreen = false;
                    form.gameUtility.ToggleFullScreen();
                }

                if (form.resourceManager.npeBounds["Options_on"].Contains(e.Location.ToSKPoint()))
                {
                    form.uiManager.isFullScreen = true;
                    form.gameUtility.ToggleFullScreen();
                }
            }
        }
    }
}
