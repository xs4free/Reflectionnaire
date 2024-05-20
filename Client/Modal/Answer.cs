using Reflectionnaire.Shared;
using System.ComponentModel;

namespace Reflectionnaire.Client.Modal
{
    public class Answer : INotifyPropertyChanged
    {
        public Question? Question { get; set; }

        private int score;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Score 
        {
            get => score;
            set
            {
                score = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            } 
        }
    }
}