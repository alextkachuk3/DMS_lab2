using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Vlc.DotNet.Core;

namespace DMS_lab2
{
    public partial class MainWindow : Window
    {
        Player player;
        double media_pos;
        public MainWindow()
        {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "vlc"));
            VlcControl.SourceProvider.CreatePlayer(libDirectory);
            player = new Player(VlcControl.SourceProvider.MediaPlayer);
            VlcControl.SourceProvider.MediaPlayer.PositionChanged += player_PositionChanged;
            media_pos = 0.0;
        }

        private void player_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                media_pos = e.NewPosition;
                TimeSpan currentPosition = TimeSpan.FromSeconds(e.NewPosition * player.GetMediaDuration().TotalSeconds);
                timePassedTextBlock.Text = currentPosition.ToString(@"hh\:mm\:ss");
                timeLeftTextBlock.Text = player.GetMediaDuration().Subtract(currentPosition).ToString(@"hh\:mm\:ss");

                playerSlider.Value = e.NewPosition;
            }
            ));
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            if (openFileDialog.ShowDialog() is true)
            {
                player.Play(openFileDialog.FileName);
                playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                UpdateFileName();
            }
        }

        private void UpdateFileName()
        {
            fileNameTextBlock.Text = player.GetMediaTitle();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.IsPaused() is true)
            {
                player.Continue();
                playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            }
            else
            {
                player.Pause();
                playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            }
        }

        private void playerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(e.NewValue != media_pos)
            {
                player.SetPosition(e.NewValue);
            }
        }
    }
}
