using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTDL;

public partial class AboutPage : ContentPage
{
    public string VersionText { get; } = $"Version {AppInfo.VersionString}";

    public AboutPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void ViewSourceClicked(object sender, EventArgs e)
    {
        try
        {
            await Launcher.Default.OpenAsync("https://github.com/Spectre71");
        }
        catch(Exception ex)
        {
            await DisplayAlert("Error", $"Could not open repository: {ex.Message}", "OK");
        }
    }
}
