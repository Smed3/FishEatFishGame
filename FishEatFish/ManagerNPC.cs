using System;
using SkiaSharp;
using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishEatFish
{
    internal class ManagerNPC
    {
        private static readonly Random rnd = new Random();
        private static int max = 10;
        private readonly Form1 form;
        private readonly List<NPC> fishList = new List<NPC>();
        private Normal normalDistribution;
        private int bombCount = 0;

        public NPC[] CreatedFishes => fishList.ToArray();

        public static int Max
        {
            get { return max; }
            set { max = value >= 0 ? value : 0; }///////////// Delete after debugging
        }

        public ManagerNPC(Form1 form)
        {
            this.form = form;
        }

        private void MakeFish(int level)
        {
            List<string> names = form.gameUtility.FilterStringsByFirstNumber(form.resourceManager.names, ref level);
            string name = names[rnd.Next(names.Count)];
            SKBitmap image = form.resourceManager.images[name].Copy();
            int speed = rnd.Next(1, 4);
            string direction = rnd.Next(2) == 0 ? "left" : "right";
            SKPoint location = new SKPoint();
            if (direction == "left")
                location.X = rnd.Next(form.ClientSize.Width, form.ClientSize.Width + image.Width * 10);
            else
                location.X = rnd.Next(-10 * image.Width, -image.Width);
            location.Y = rnd.Next(50, form.ClientSize.Height - image.Height);
            NPC fish = new NPC(form, image, location, speed, level, direction);
            fishList.Add(fish);
        }

        private void MakeBomb()
        {
            int bombMax = (int)(Math.Round(Math.Log(Player.MaxLevel, 2)));
            if (bombCount >= bombMax || Player.MaxLevel < 5)
                return;

            SKBitmap image = SKBitmap.Decode(@"..\\..\\Resources\\bomb.png");
            string direction = rnd.Next(2) == 0 ? "left" : "right";
            SKPoint location = new SKPoint();
            if (direction == "left")
                location.X = rnd.Next(form.ClientSize.Width, form.ClientSize.Width + image.Width * 10);
            else
                location.X = rnd.Next(-10 * image.Width, -image.Width);
            location.Y = rnd.Next(40, form.ClientSize.Height - image.Height);
            NPC bomb = new NPC(form, image, location, 1, 99999999, direction);
            fishList.Add(bomb);
            bombCount++;
        }

        public int GenerateLevel()
        {
            // Calculate the mean based on the player's level
            double mean = Player.MaxLevel - 1;
            normalDistribution = new Normal(mean, 3);

            // Generate a random fish level from the normal distribution
            double fishLevel = normalDistribution.Sample();

            // Round the fish level to the nearest integer
            int roundedFishLevel = (int)Math.Round(fishLevel);

            // Ensure the fish level is within valid bounds
            roundedFishLevel = Math.Max(1, roundedFishLevel); // Minimum fish level
            roundedFishLevel = Math.Min(100, roundedFishLevel); // Maximum fish level

            // Create and return a fish of the determined level
            return roundedFishLevel < Player.MaxLevel ? rnd.Next(1, (int)Player.MaxLevel) : roundedFishLevel;
        }

        public void UpdateNPC()
        {
            MakeBomb();

            if (NPC.Count < max)
                MakeFish(GenerateLevel());

            bool isThereSmallFish = false;
            for (int i = 0; i < fishList.Count; i++)
            {
                if (fishList[i].Level == 1)
                    isThereSmallFish = true;

                if (fishList[i].UpdateNPC() || fishList[i].Bounds.Bottom > form.ClientSize.Height)
                    RemoveFish(fishList[i]);
            }
            if (!isThereSmallFish)
                MakeFish(1);
        }

        public void UpdateNPCSize()
        {
            foreach (NPC fish in fishList)
            {
                float width = form.ClientSize.Width * fish.Image.Width / 1536;
                float height = form.ClientSize.Height * fish.Image.Height / 864;
                fish.Bounds.Size = new SKSize(width, height);
            }
        }

        public void RemoveFish(NPC fish)
        {
            if (fish.Level > 99999)
                bombCount--;
            fish.Dispose();
            fishList.Remove(fish);
        }
    }

}
