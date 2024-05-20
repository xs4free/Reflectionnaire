using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Reflectionnaire.Client.Modal;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Client.Pages;

public partial class Questions
{
    [Inject] private HttpClient ReflectionnaireService { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Parameter] public int? GameId { get; set; }
    
    private List<Answer> _answers = [];
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

            foreach(var answer in _answers)
            {
                answer.PropertyChanged += Answer_PropertyChanged;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void Answer_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not Answer answer)
            return;

        var index = _answers.IndexOf(answer);
        if (index + 1 > _answers.Count - 1)
            return;
        
        var nextAnswer = _answers[index+1];

        JSRuntime.InvokeVoidAsync("Reflectionnaire.scrollToAnswer", nextAnswer.Question?.Id);
    }

    private async Task OnSentClicked(MouseEventArgs obj)
    {
        await SentAnswersAsync();
        UpdateRadarChart();
    }

    private async Task SentAnswersAsync()
    {
        string reflectionnaireId = Uri.EscapeDataString("121331d6-8c01-4dfc-b5fe-1776b1184baa");
        string url = $"/api/Answers";

        var answers = new ReflectionnaireAnswers
        {
            ReflectionnaireId = reflectionnaireId,
            UserId = Guid.NewGuid(),
            QuestionAnswers = _answers.Select(answer => new QuestionAnswer { QuestionId = answer.Question.Id, Score = answer.Score }).ToList(),
        };

        await ReflectionnaireService.PostAsJsonAsync(url, answers);
    }

    private void UpdateRadarChart()
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