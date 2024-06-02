using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Client.Pages.Manage;

public partial class ReflectionnaireReport
{
    [Inject] private HttpClient ReflectionnaireService { get; set; }
    [Inject] private ILogger<ReflectionnaireReport> Logger { get; set; }
    [Parameter] public Guid? ReflectionnaireId { get; set; }

    private ReflectionnaireAllUserAnswers? _reflectionnaire;

    private float _score1 = 0;
    private float _score2 = 0;
    private float _score3 = 0;
    private float _score4 = 0;

    protected override async Task OnInitializedAsync()
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
}