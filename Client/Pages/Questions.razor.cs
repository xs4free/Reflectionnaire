using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Reflectionnaire.Client.Modal;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Client.Pages;

public partial class Questions
{
    [Inject] private HttpClient ReflectionnaireService { get; set; }
    [Parameter] public int? GameId { get; set; }
    
    private IEnumerable<Answer> _answers = [];
    private CategoryTotal[]? _scores;

    private int _score1 = 0;
    private int _score2 = 0;
    private int _score3 = 0;
    private int _score4 = 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            string reflectionnaireId = Uri.EscapeDataString("121331d6-8c01-4dfc-b5fe-1776b1184baa");
            string url = $"/api/Questions?reflectionnaireId={reflectionnaireId}";

            var questions = await ReflectionnaireService.GetFromJsonAsync<Question[]>(url) ?? [];
            _answers = questions.Select(question => new Answer() { Question = question, Score = 3 }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void OnSentClicked(MouseEventArgs obj)
    {
        _scores = _answers
            .Where(answer => answer.Question != null)
            .GroupBy(answer => answer.Question?.Category)
            .Select(group => new CategoryTotal { TotalScore = group.Sum(g => g.Score), Category = group.Key.Value })
            .ToArray();

        _score1 = _scores.First(s => s.Category == Category.Execution).TotalScore;
        _score2 = _scores.First(s => s.Category == Category.Things).TotalScore;
        _score3 = _scores.First(s => s.Category == Category.People).TotalScore;
        _score4 = _scores.First(s => s.Category == Category.Place).TotalScore;
    }
}