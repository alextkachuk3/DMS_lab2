using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Vlc.DotNet.Core;

namespace DMS_lab2
{
    public partial class MainWindow : Window
    {
        Player player;
        string ulr;
        double currentMediaPosition;
        bool rewind;
        bool forward;
        double rewindingSpeed;
        bool local;
        bool stream;
        public MainWindow()
        {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "vlc"));
            VlcControl.SourceProvider.CreatePlayer(libDirectory);
            player = new Player(VlcControl.SourceProvider.MediaPlayer);
            VlcControl.SourceProvider.MediaPlayer.PositionChanged += player_PositionChanged;
            currentMediaPosition = 0.0;
            rewindingSpeed = 0.025;
            rewind = false;
            forward = false;
            local = false;
            stream = false;
        }

        private void player_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                if (forward)
                {
                    player.SetPosition(e.NewPosition + rewindingSpeed);
                }
                else if (rewind)
                {
                    player.SetPosition(e.NewPosition - rewindingSpeed);
                }

                currentMediaPosition = e.NewPosition;
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
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.RMI;*.MKV";
            if (openFileDialog.ShowDialog() is true)
            {
                ulr = openFileDialog.FileName;
                player.Play(openFileDialog.FileName);
                playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                UpdateFileName();
                local = true;
                stream = false;
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
                if (!player.IsPlaying() && local)
                {
                    player.Play(ulr);
                    playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                }
                else
                {
                    player.Pause();
                    playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                }
            }
        }

        private void playerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue != currentMediaPosition)
            {
                player.SetPosition(e.NewValue);
            }
        }

        private void openStreamButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new UrlInputWindow();
            if (window.ShowDialog() is true)
            {
                player.Play(window.GetUrl());
                playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                UpdateFileName();
                stream = true;
                local = false;
            }
        }

        private void rewindButton_Click(object sender, RoutedEventArgs e)
        {
            if (rewind)
            {
                rewind = false;
                rewindButton.Background = null;
            }
            else
            {
                rewind = true;
                player.SetPosition(currentMediaPosition - rewindingSpeed);
                rewindButton.Background = new SolidColorBrush(Color.FromRgb(103, 58, 183));
            }

            if (forward)
            {
                forward = false;
                forwardButton.Background = null;
            }
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (forward)
            {
                forward = false;
                forwardButton.Background = null;
            }
            else
            {
                forward = true;
                player.SetPosition(currentMediaPosition + rewindingSpeed);
                forwardButton.Background = new SolidColorBrush(Color.FromRgb(103, 58, 183));
            }

            if (rewind)
            {
                rewind = false;
                rewindButton.Background = null;
            }
        }
    }
}
