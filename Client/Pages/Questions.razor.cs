using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Reflectionnaire.Client.Modal;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Client.Pages;

public partial class Questions
{
    [Inject] private HttpClient ReflectionnaireService { get; set; } = default!;
    [Inject] IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] ILogger<Questions> Logger { get; set; } = default!;
    [Parameter] public Guid? ReflectionnaireId { get; set; }
    
    private List<Answer> _answers = [];
    private CategoryTotal[]? _scores;
    private ReflectionnaireData? _reflectionnaire;

    private double _questionsAnswered = 0;
    private double _questionsTotal = 0;
    private string _unit => $" van de {_questionsTotal} vragen zijn beantwoord";


    private float _score1 = 0;
    private float _score2 = 0;
    private float _score3 = 0;
    private float _score4 = 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            string url = $"/api/Reflectionnaires?reflectionnaireId={ReflectionnaireId}";

            _reflectionnaire = await ReflectionnaireService.GetFromJsonAsync<ReflectionnaireData>(url);
            if (_reflectionnaire == null)
            {
                return;
            }    

            _answers = _reflectionnaire.Questions.Select(question => new Answer() { Question = question, Score = 0 }).ToList();

            foreach(var answer in _answers)
            {
                answer.PropertyChanged += Answer_PropertyChanged;
            }
            UpdateProgressBar();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to get Reflectionnaire from API");
        }
    }

    private void UpdateProgressBar()
    {
        _questionsTotal = _answers.Count;
        _questionsAnswered = _answers.Count(answer => answer.Score != 0);
    }

    private void Answer_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not Answer answer)
        {
            return;
        }

        UpdateProgressBar();

        // scroll to first unanswered question
        var unansweredQuestion = _answers.FirstOrDefault(answer => answer.Score == 0);
        if (unansweredQuestion != null)
        {
            JSRuntime.InvokeVoidAsync("Reflectionnaire.scrollToAnswer", unansweredQuestion.Question?.Id).ConfigureAwait(false);
            return;
        }

        // scroll to sentBlock
        JSRuntime.InvokeVoidAsync("Reflectionnaire.scrollToAnswer", "sentBlock").ConfigureAwait(false);
    }

    private async Task OnSentClicked(MouseEventArgs obj)
    {
        var unansweredQuestion = _answers.FirstOrDefault(answer => answer.Score == 0);
        if (unansweredQuestion != null)
        {
            await DialogService.Alert("Nog niet alle vragen zijn beantwoord, we scrollen terug naar de eerste onbeantwoorde vraag. Zorg dat alle vragen minimaal 1 ster hebben.", "Niet alles beantwoord", new AlertOptions() { OkButtonText = "Oke" });
            await JSRuntime.InvokeVoidAsync("Reflectionnaire.scrollToAnswer", unansweredQuestion.Question?.Id).ConfigureAwait(false);
            return;
        }

        await SentAnswersAsync();
        UpdateRadarChart();
    }

    private async Task OnStartClicked(MouseEventArgs args)
    {
        if (_answers.Count == 0)
        {
            return;
        }

        await JSRuntime.InvokeVoidAsync("Reflectionnaire.scrollToAnswer", _answers[0].Question?.Id);
    }

    private async Task SentAnswersAsync()
    {
        if (ReflectionnaireId == null)
        {
            return;
        }

        string url = $"/api/UserAnswers";

        var answers = new ReflectionnaireUserAnswers
        {
            ReflectionnaireId = ReflectionnaireId.Value.ToString("D"),
            UserId = Guid.NewGuid(),
            QuestionAnswers = _answers.Select(answer => new QuestionAnswer { QuestionId = answer.Question?.Id ?? -1, Score = answer.Score }).ToList(),
        };

        await ReflectionnaireService.PostAsJsonAsync(url, answers);
    }

    private void UpdateRadarChart()
    {
        _scores = _answers
            .Where(answer => answer.Question != null)
            .GroupBy(answer => answer.Question.Category)
            .Select(group => new CategoryTotal { TotalScore = group.Sum(g => g.Score), Category = group.Key })
            .ToArray();

        _score1 = _scores.First(s => s.Category == Category.Execution).TotalScore;
        _score2 = _scores.First(s => s.Category == Category.Things).TotalScore;
        _score3 = _scores.First(s => s.Category == Category.People).TotalScore;
        _score4 = _scores.First(s => s.Category == Category.Place).TotalScore;
    }
}