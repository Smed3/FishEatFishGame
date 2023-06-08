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
    internal class Gameplay
    {
        private readonly Form1 form;
        private readonly Timer timer = new Timer();
        public Player[] players = new Player[3];

        public Gameplay(Form1 form)
        {
            this.form = form;
            this.form.skControl.KeyUp += SkControl_KeyUp;

            float width = form.ClientSize.Width * 63 / 1536;
            float height = form.ClientSize.Height * 43 / 864;
            players = new Player[]
            {
                new Player(form, form.resourceManager.playersImages["Violet_right"], new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D }, new SKPoint(form.Width / 2, 50), 5, new SKSize(width, height)),
                new Player(form, form.resourceManager.playersImages["Green_right"], new Keys[] { Keys.U, Keys.H, Keys.J, Keys.K }, new SKPoint(form.Width / 2, 50), 5, new SKSize(width, height)),
                new Player(form, form.resourceManager.playersImages["Blue_right"], new Keys[] { Keys.Up, Keys.Left, Keys.Down, Keys.Right }, new SKPoint(form.Width / 2, 50), 5, new SKSize(width, height))
            };
            foreach (Player player in players) player.Kill();

            timer.Interval = 20;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!form.uiManager.isMenuVisible && !form.uiManager.isOptionsVisible)
            {
                form.managerNPC.UpdateNPC();
                foreach (Player player in players)
                {
                    if (player.Level != 0)
                    {
                        player.UpdatePlayer();
                        CheckCollisions(player);
                    }
                }
                Player.MaxLevel = Math.Max(Math.Max(players[0].Level, players[1].Level), players[2].Level);
            }
            form.skControl.Invalidate();
        }

        private void CheckCollisions(Player player)
        {
            SKRect Pbounds = player.Bounds;
            Pbounds.Inflate(-Pbounds.Width / 5, -Pbounds.Height / 5);
            foreach (NPC fish in form.managerNPC.CreatedFishes)
            {
                SKRect Fbounds = fish.Bounds;
                Fbounds.Inflate(-Fbounds.Width / 6, -Fbounds.Height / 6);
                if (Fbounds.IntersectsWith(Pbounds))
                {
                    if (fish.Level <= player.Level)
                        EatFish(player, fish);
                    else
                    {
                        player.Kill();
                        form.soundManager.PlaySound("Death");
                        return;
                    }
                }
            }
        }

        private void EatFish(Player player, NPC fish)
        {
            player.Level += fish.Level > 1 ? Math.Log(fish.Level, 100) : 0.07;
            form.managerNPC.RemoveFish(fish);
            form.soundManager.PlaySound("Eat");
        }

        public void RestartGame()
        {
            foreach (NPC fish in form.managerNPC.CreatedFishes)
                form.managerNPC.RemoveFish(fish);
            foreach (Player player in players)
                player.Kill();
            Player.MaxLevel = 1;
        }

        public void UpdatePlayersSize()
        {
            foreach (Player player in players)
                player?.UpdatePlayerSize();
        }

        private void SkControl_KeyUp(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.M:
                    ManagerNPC.Max--;
                    break;
                case Keys.B:
                    ManagerNPC.Max++;
                    break;
                case Keys.R:
                    players[0].Kill();
                    players[0].Revive();
                    break;
                case Keys.W:
                    if (players[0].Level < 1)
                        players[0].Revive();
                    break;
                case Keys.U:
                    if (players[1].Level < 1)
                        players[1].Revive();
                    break;
                case Keys.Up:
                    if (players[2].Level < 1)
                        players[2].Revive();
                    break;
                case Keys.Z:
                    if (players[0].Level != 0)
                        players[0].Level += 10;
                    break;
                case Keys.X:
                    if (players[0].Level != 0)
                        players[0].Level--;
                    break;
            }
        }
    }
}
