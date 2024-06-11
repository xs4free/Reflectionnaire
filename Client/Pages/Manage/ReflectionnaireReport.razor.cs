using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QRCoder;
using Reflectionnaire.Shared;
using static QRCoder.PayloadGenerator;

namespace Reflectionnaire.Client.Pages.Manage;

public partial class ReflectionnaireReport
{
    [Inject] private HttpClient ReflectionnaireService { get; set; } = null!;
    [Inject] private ILogger<ReflectionnaireReport> Logger { get; set; } = null!;
    [Inject] IJSRuntime JSRuntime { get; set; } = null!;
    [Parameter] public Guid? ReflectionnaireId { get; set; }

    private ReflectionnaireAllUserAnswers? _reflectionnaire;

    private float _score1 = 0;
    private float _score2 = 0;
    private float _score3 = 0;
    private float _score4 = 0;
    private string _qrBytes = "";
    private string _urlReflectionnaire = "";

    protected override async Task OnInitializedAsync()
    {
        await UpdateRadarChart();

        _urlReflectionnaire = $"https://www.reflectionnaire.com/questions/{ReflectionnaireId}";

        GenerateQRCode(_urlReflectionnaire);
    }

    private async Task UpdateRadarChart()
    {
        try
        {
            string url = $"/api/AllUsersAnswers?reflectionnaireId={ReflectionnaireId}";

            _reflectionnaire = await ReflectionnaireService.GetFromJsonAsync<ReflectionnaireAllUserAnswers>(url);
            if (_reflectionnaire == null)
            {
                return;
            }

            _score1 = _reflectionnaire.CategoryTotals.FirstOrDefault(s => s.Category == Category.Execution)?.TotalScore ?? 0;
            _score2 = _reflectionnaire.CategoryTotals.FirstOrDefault(s => s.Category == Category.Things)?.TotalScore ?? 0;
            _score3 = _reflectionnaire.CategoryTotals.FirstOrDefault(s => s.Category == Category.People)?.TotalScore ?? 0;
            _score4 = _reflectionnaire.CategoryTotals.FirstOrDefault(s => s.Category == Category.Place)?.TotalScore ?? 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting all users answers");
        }
    }

    private async Task Refresh_Clicked()
    {
        await UpdateRadarChart();
    }

    private Task ShowAnswers_Clicked()
    {
        JSRuntime.InvokeVoidAsync("Reflectionnaire.scrollToAnswer", "radarChart").ConfigureAwait(false);
        
        return Task.CompletedTask;
    }

    private void GenerateQRCode(string url)
    {
        Url generator = new(url);
        string payload = generator.ToString();

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        using PngByteQRCode qrCode = new(qrCodeData);
        byte[] darkColorRgba = [12, 92, 109, 255]; // reflectionnaire green
        byte[] lightColorRgba = [0, 0, 0, 0]; // transparant
        byte[] qrCodeImage = qrCode.GetGraphic(20, darkColorRgba, lightColorRgba);

        string base64 = Convert.ToBase64String(qrCodeImage);
        _qrBytes = string.Format("data:image/png;base64,{0}", base64);
    }
}