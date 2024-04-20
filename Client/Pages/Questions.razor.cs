using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Reflectionnaire.Client.Modal;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Client.Pages;

public partial class Questions
{
    [Inject] private HttpClient Http { get; set; }
    [Parameter] public int? GameId { get; set; }
    
    private Answer[]? _answers;
    private CategoryTotal[]? _scores;

    private int _score1 = 0;
    private int _score2 = 0;
    private int _score3 = 0;
    private int _score4 = 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var questions = await Http.GetFromJsonAsync<Question[]>("/api/Questions") ?? [];
            _answers = questions.Select(question => new Answer() { Question = question, Score = 3 }).ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void OnSentClicked(MouseEventArgs obj)
    {
        _scores = _answers?.GroupBy(answer => answer.Question.Category)
            .Select(group => new CategoryTotal { TotalScore = group.Sum(g => g.Score), Category = group.Key })
            .ToArray();

        _score1 = _scores.First(s => s.Category == Category.Execution).TotalScore;
        _score2 = _scores.First(s => s.Category == Category.Things).TotalScore;
        _score3 = _scores.First(s => s.Category == Category.People).TotalScore;
        _score4 = _scores.First(s => s.Category == Category.Place).TotalScore;
    }
}