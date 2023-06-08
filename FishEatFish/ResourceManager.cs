using System;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishEatFish
{
    internal class ResourceManager
    {
        public SKBitmap[] digitImages = new SKBitmap[10];
        public Dictionary<string, SKBitmap> playersImages = new Dictionary<string, SKBitmap>();
        public Dictionary<string, SKBitmap> npe = new Dictionary<string, SKBitmap>();
        public Dictionary<string, SKRect> npeBounds = new Dictionary<string, SKRect>(); // stores bounds of each interactive npe
        public Dictionary<string, SKBitmap> images = new Dictionary<string, SKBitmap>(); // stores images that represent npcs
        public List<string> names = new List<string>(); // strores names of images that represent npcs
        public Dictionary<string, string> soundPaths = new Dictionary<string, string>(); // strores sound paths by sound name
        public string[] backMelodies; // strores background melodies paths

        public ResourceManager()
        {
            LoadPlayers(@"..\\..\\Resources\\players");

            LoadNPE(@"..\\..\\Resources\\npe");

            LoadDigits(@"..\\..\\Resources\\npe\\digits");

            LoadNPC(@"..\\..\\Resources\\npc");

            names = names.OrderBy(s => int.Parse(new string(s.TakeWhile(char.IsDigit).ToArray()))).ToList(); // sort names by fish level

            LoadSounds(@"..\\..\\Resources\\media");

            backMelodies = Directory.GetFiles(@"..\\..\\Resources\\media\\background");
        }

        private void LoadPlayers(string directory)
        {
            string[] paths = Directory.GetFiles(directory);
            foreach (string path in paths)
            {
                string name = Path.GetFileNameWithoutExtension(path);
                SKBitmap bp = SKBitmap.Decode(path);
                playersImages.Add(name, bp);
            }
        }

        private void LoadNPE(string directory)
        {
            string[] paths = Directory.GetFiles(directory);
            foreach (string path in paths)
            {
                string name = Path.GetFileNameWithoutExtension(path);
                SKBitmap bp = SKBitmap.Decode(path);
                npe.Add(name, bp);
                npeBounds.Add(name, new SKRect());
            }
        }

        private void LoadNPC(string directory)
        {
            string[] paths = Directory.GetFiles(directory);
            foreach (string path in paths)
            {
                string name = Path.GetFileNameWithoutExtension(path);
                names.Add(name);
                images.Add(name, SKBitmap.Decode(path));
            }
        }

        private void LoadDigits(string directory)
        {
            for (int i = 0; i < 10; i++)
                digitImages[i] = SKBitmap.Decode($"{directory}\\{i}.png");
        }

        private void LoadSounds(string directory)
        {
            string[] paths = Directory.GetFiles(directory);
            foreach (string path in paths)
            {
                string name = Path.GetFileNameWithoutExtension(path);
                soundPaths.Add(name, path);
            }
        }
    }
}
