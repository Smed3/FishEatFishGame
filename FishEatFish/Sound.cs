using System;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishEatFish
{
    public class Sound : IDisposable
    {
        private readonly Form1 form;
        private readonly Dictionary<string, WaveOut> sfxPlayers = new Dictionary<string, WaveOut>();
        private WaveOutEvent musicPlayer = new WaveOutEvent(); // Represents the audio output device
        private AudioFileReader musicFile; // Represents the audio file to be played
        private bool isPlaying = false; // Flag to track if the audio is currently playing
        private int currentMusicIndex = 0; // Index of the currently playing music
        private bool isNeedToShowAudioError = true;
        private float musicVolume;
        private float sfxVolume;

        public float MusicVolume
        {
            get { return musicVolume; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 1)
                    value = 1;
                musicVolume = value;
                musicPlayer.Volume = musicVolume;
            }
        }
        public float SfxVolume
        {
            get { return sfxVolume; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 1)
                    value = 1;
                sfxVolume = value;
            }
        }

        public Sound(Form1 form)
        {
            this.form = form;
            MusicVolume = 0.5f;
            SfxVolume = 0.5f;
        }

        public void PlayBackgroundMusic()
        {
            StopBackgroundMusic();

            musicPlayer = new WaveOutEvent();
            musicPlayer.PlaybackStopped += WaveOut_PlaybackStopped;

            musicFile = new AudioFileReader(form.resourceManager.backMelodies[currentMusicIndex]);

            try
            {
            musicPlayer.Init(musicFile);
            musicPlayer.Play();
            isPlaying = true;
        }
            catch
            {
                if(isNeedToShowAudioError)
                {
                    MessageBox.Show("Error loading audio files.\n\nPlease restart your computer to fix the error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isNeedToShowAudioError = false;
                }
            }
        }

        private void StopBackgroundMusic()
        {
            if (isPlaying)
            {
                // Stop playback and wait for the background thread to finish
                musicPlayer.Stop();

                // Unregister the PlaybackStopped event
                musicPlayer.PlaybackStopped -= WaveOut_PlaybackStopped;

                // Clean up resources
                musicPlayer.Dispose();
                musicFile.Dispose();
                musicPlayer = null;
                musicFile = null;
                isPlaying = false;
            }
        }

        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Switch to the next music track
            currentMusicIndex++;
            if (currentMusicIndex >= form.resourceManager.backMelodies.Length)
            {
                currentMusicIndex = 0; // Start from the beginning if reached the end
            }

            // Play the next music track
            PlayBackgroundMusic();
        }


        public void PlaySound(string soundName)
        {
            if (!form.resourceManager.soundPaths.ContainsKey(soundName))
                return;

            StopAllSounds();

            string soundPath = form.resourceManager.soundPaths[soundName];

            // Create a new WaveOut  and AudioFileReader
            WaveOut sfxPlayer = new WaveOut();
            AudioFileReader sfxFile = new AudioFileReader(soundPath);
            WaveChannel32 sfxChannel = new WaveChannel32(sfxFile);

            // Connect the AudioFileReader to the WaveOutEvent
            sfxChannel.Volume = SfxVolume;
            sfxPlayer.Init(sfxChannel);

            try
            {
                sfxPlayer.Init(sfxChannel);
            sfxPlayer.Play();
            sfxPlayers[soundName] = sfxPlayer;
        }
            catch
            {
                if (isNeedToShowAudioError)
                {
                    MessageBox.Show("Error loading audio files.\n\nPlease restart your computer to fix the error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isNeedToShowAudioError = false;
                }
            }
        }

        public void StopSound(string soundName)
        {
            if (sfxPlayers.TryGetValue(soundName, out WaveOut sfxPlayer))
            {
                sfxPlayer.Stop();
                sfxPlayer.Dispose();
                sfxPlayers.Remove(soundName);
            }
        }

        public void StopAllSounds()
        {
            foreach (var soundPlayer in sfxPlayers.Values)
            {
                soundPlayer.Stop();
                soundPlayer.Dispose();
            }
            sfxPlayers.Clear();
        }

        public void Dispose()
        {
            StopAllSounds();
            musicPlayer.Dispose();
            musicFile.Dispose();
        }
    }
}
