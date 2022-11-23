using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace DMS_lab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "vlc"));
            VlcControl.SourceProvider.CreatePlayer(libDirectory);
            VlcControl.SourceProvider.MediaPlayer.Play(new Uri("C:\\Users\\olex1\\OneDrive\\Desktop\\DMS\\Result H264.mp4"));
        }
    }
}
