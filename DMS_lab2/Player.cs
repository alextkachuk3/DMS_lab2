using System;
using Vlc.DotNet.Core;

namespace DMS_lab2
{
    internal class Player
    {
        private VlcMediaPlayer player;
        private bool paused;
        public Player(VlcMediaPlayer player)
        {
            this.player = player;
            paused = false;
        }

        public string GetMediaTitle()
        {
            return player.GetMedia().Title;
        }

        public TimeSpan GetMediaDuration()
        {
            return player.GetMedia().Duration;
        }

        public void Play(string URI)
        {
            player.Play(new Uri(URI));
            paused = false;
        }

        public void SetPosition(double position)
        {
            player.Position = (float)position;
        }

        public void Continue()
        {
            if (paused is true)
            {
                paused = false;
                player.Play();
            }
        }

        public void Pause()
        {
            if (paused is false)
            {
                paused = true;
                player.Pause();
            }
        }

        public bool IsPaused()
        {
            return paused;
        }
    }
}
