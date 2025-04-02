using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using YoutubeExplode;
using YoutubeExplode.Converter;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System;

namespace YTDL;

public partial class MainPage : ContentPage
{
    public string AppVersion { get; } = AppInfo.VersionString;
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            StatusLabel.Text = "Starting download ...";
            var youtube = new YoutubeClient();
            var videoUrl = UrlEntry.Text;

            // Retrieve video metadata
            var video = await youtube.Videos.GetAsync(videoUrl);
            //var sanitizedTitle = Regex.Replace(video.Title, @"[^\w\-]", "_");
            var OriginalTitle = video.Title
                .Replace("\\","-")
                .Replace(":","-")
                .Replace("/","-");

            // Get the steam manifest
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

            // Get the highest bitrate audio-only stream
            var AudioStreamInfo = streamManifest.GetAudioOnlyStreams()
                .OrderByDescending(s => s.Bitrate)
                .FirstOrDefault();

            if(AudioStreamInfo == null)
            {
                StatusLabel.Text = "No suitable video stream found!";
                return;
            }

            // Platform specific paths
            string filePath;
            string basePath;

        #if ANDROID
            // Android: Sabve to Documents/YTDL
            var docsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;

            if (string.IsNullOrEmpty(docsPath))
            {
                StatusLabel.Text = "Could not accesss Documents directory.";
                return;
            }

            basePath = Path.Combine(docsPath, "YTDL");
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if(status != PermissionStatus.Granted)
            {
                StatusLabel.Text = "Storage permission denied.";
                return;
            }
#       elif WINDOWS
            basePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "YTDL");

        #else

            basePath = FileSystem.AppDataDirectory;

        #endif
            // Build full file path
            Directory.CreateDirectory(basePath);

            filePath = Path.Combine(
                basePath,
                $"{OriginalTitle}.mp3"
                //$"{sanitizedTitle}.{AudioStreamInfo.Container.Name}"
                );

            // Download audio stream
            await youtube.Videos.Streams.DownloadAsync(AudioStreamInfo, filePath);
            StatusLabel.Text = $"Downloaded to: {filePath}";
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
    }
}