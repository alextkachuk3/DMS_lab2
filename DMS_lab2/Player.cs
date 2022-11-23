using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vlc.DotNet.Core;
using Vlc.DotNet.Wpf;

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

        public void Play(string URI)
        {
            player.Play(new Uri(URI));
            paused = false;
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
