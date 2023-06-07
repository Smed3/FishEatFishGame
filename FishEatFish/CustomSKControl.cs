using System;
using SkiaSharp.Views.Desktop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishEatFish
{
    internal class CustomSKControl : SKControl
    {
        protected override bool IsInputKey(Keys keyData)
        {
            // Capture arrow keys as input keys
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
    }
}
