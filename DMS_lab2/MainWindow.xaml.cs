using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Vlc.DotNet.Core;

namespace DMS_lab2
{
    public partial class MainWindow : Window
    {
        private VlcMediaPlayer player;
        public MainWindow()
        {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "vlc"));
            VlcControl.SourceProvider.CreatePlayer(libDirectory);
            player = VlcControl.SourceProvider.MediaPlayer;
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            if (openFileDialog.ShowDialog() is true)
            {
                player.Play(new Uri(openFileDialog.FileName));
                UpdateFileName();
            }
        }

        private void UpdateFileName()
        {
            fileNameTextBlock.Text = VlcControl.SourceProvider.MediaPlayer.GetMedia().Title;
        }
    }
}
