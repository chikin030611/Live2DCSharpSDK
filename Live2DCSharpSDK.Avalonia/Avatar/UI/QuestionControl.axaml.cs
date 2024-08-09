using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Live2DCSharpSDK.Avalonia.Avatar.UI;

public partial class QuestionControl : UserControl
{
    public Dictionary<int, string> Questions = new Dictionary<int, string>();
    public ObservableCollection<QuestionItem> DisplayQuestions { get; } = new ObservableCollection<QuestionItem>();

    public class QuestionItem
    {
        public int Id { get; set; }
        public string Question { get; set; }
    }

    public QuestionControl()
    {
        InitializeComponent();
        DataContext = this;

        foreach (var item in QnaAudioManager.QnaList)
        {
            int id = item.Id;
            string question = item.Question;
            Questions.Add(id, question);
            DisplayQuestions.Add(new QuestionItem { Id = id, Question = question });
        }
    }

    private void QuestionButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            int questionId = (int)(button.Tag ?? -1);

            // Handle the question button click
        }
    }
}
