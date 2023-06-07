using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishEatFish
{
    public partial class Form1 : Form
    {
        internal CustomSKControl skControl;
        internal UIManager uiManager;
        internal ResourceManager resourceManager;
        internal Gameplay gameplay;
        internal GameUtility gameUtility;
        internal ManagerNPC managerNPC;
        internal Sound soundManager;
        internal InputManager inputManager;
        // добавить экран смерти,
        // подправить размеры нпс (ошибочная попытка съесть нпс), добавить бомбы и поработать с логикой присвоения уровня,
        // , протестировать на других экранах,

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            this.Resize += Form1_Resize;
        }

        private void InitializeGame()
        {
            skControl = new CustomSKControl();
            skControl.Size = ClientSize;

            Controls.Add(skControl);

            resourceManager = new ResourceManager();
            uiManager = new UIManager(this);
            gameplay = new Gameplay(this);
            gameUtility = new GameUtility(this);
            managerNPC = new ManagerNPC(this);
            soundManager = new Sound(this);
            inputManager = new InputManager(this);

            soundManager.PlayBackgroundMusic();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                float ratio = (float)Screen.PrimaryScreen.Bounds.Width / Screen.PrimaryScreen.Bounds.Height;
                int newWidth = (int)(this.Height * ratio);
                int newHeight = (int)(this.Width / ratio);

                // Adjust the form's size based on the calculated values
                if (this.Width > newWidth)
                    this.Width = newWidth;
                else
                    this.Height = newHeight;
            }
            if (WindowState == FormWindowState.Minimized)
                uiManager.isMenuVisible = true;
            if (skControl != null)
                skControl.Size = ClientSize;
            managerNPC?.UpdateNPCSize();
            gameplay?.UpdatePlayersSize();
        }
    }
}
