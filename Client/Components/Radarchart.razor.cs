using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;

namespace Reflectionnaire.Client.Components;

public partial class Radarchart : ComponentBase
{
    private bool _imageLoaded;
    private BlazorCanvas Canvas { get; set; }
    [Parameter] public float Width { get; set; } = 800;
    [Parameter] public float Height { get; set; } = 800;
    [Parameter] public int ScoreCategory1 { get; set; }
    [Parameter] public int ScoreCategory2 { get; set; }
    [Parameter] public int ScoreCategory3 { get; set; }
    [Parameter] public int ScoreCategory4 { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await DrawRadarChartAsync();
        
        await base.OnInitializedAsync();
    }

    private async Task DrawRadarChartAsync()
    {
        while (Canvas is not { Ready : true })
        {
            await Task.Delay(10);
        }
        
        await Redraw();

        await InvokeAsync(StateHasChanged);
    }

    private async Task Redraw()
    {
        Canvas.ClearRect(0,0, Width, Height);
        
        await DrawBackgroundImage();
        DrawChartAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await Redraw();
        
        await base.OnParametersSetAsync();
    }

    private void DrawChartAsync()
    {
        if (!Canvas.Ready)
        {
            return;
        }
        
        var centerX = Width / 2;
        var centerY = Height / 2;
        var maxLeft = 83 / ImageWidth * Width;
        var maxRight = 847 / ImageWidth * Width;
        var maxTop = 105 / ImageHeight * Height;
        var maxBottom = 835 / ImageHeight * Height;

        var left = centerX - (Math.Min(15, ScoreCategory1) / 15f * (centerX - maxLeft));
        var top = centerY - (Math.Min(15, ScoreCategory2) / 15f * (centerY - maxTop));
        var right = centerX + (Math.Min(15, ScoreCategory3) / 15f * (maxRight - centerX));
        var bottom = centerY + (Math.Min(15, ScoreCategory4) / 15f * (maxBottom - centerY));
        
        //Path
        Canvas.StrokeStyle("black");
        Canvas.LineWidth(4);
        Canvas.BeginPath();
        Canvas.MoveTo(left, centerY);
        Canvas.LineTo(centerX, top);
        Canvas.LineTo(right, centerY);
        Canvas.LineTo(centerX, bottom);
        Canvas.LineTo(left, centerY);
        Canvas.Stroke();    
    }

    private const float ImageWidth = 934;
    private const float ImageHeight = 934;
    
    private async Task DrawBackgroundImage()
    {
        const string imageSource = "Radar-background.png";
        if (!_imageLoaded)
        {
            _imageLoaded = true;
        
            await Canvas.LoadImage(imageSource);
        }

        var width = Width;
        var widthFactor = ImageWidth / Width;
        var height = (int)Math.Round(ImageHeight / widthFactor);
        
        Canvas.DrawImage(imageSource, 0, 0, width, height);
    }
}