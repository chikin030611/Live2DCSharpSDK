using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Live2DCSharpSDK.Avalonia.Avatar.UI;

public partial class Live2dControl : UserControl
{
    private readonly Live2dRender _render;
    private readonly FpsTimer _renderTimer;

    public ObservableCollection<QuestionItem> DisplayQuestions { get; } = new ObservableCollection<QuestionItem>();
    public class QuestionItem
    {
        public int Id { get; set; }
        public string Question { get; set; }
    }

    public Live2dControl()
    {
        InitializeComponent();

        DataContext = this;

        _render = new();
        Live2D.Child = _render;
        _renderTimer = new(_render);

        foreach (var item in QnaAudioManager.QnaList)
        {
            int id = item.Id;
            string question = item.Question;
            DisplayQuestions.Add(new QuestionItem { Id = id, Question = question });
        }

        //App.OnClose += App_OnClose;
    }

    private void QuestionsItemsControl_Loaded(object sender, RoutedEventArgs e)
    {
        // Attach event handler to buttons
        foreach (var item in QuestionsItemsControl.Items)
        {
            var container = QuestionsItemsControl.ContainerFromItem(item) as ContentPresenter;
            if (container != null)
            {
                var button = container.GetVisualDescendants().OfType<Button>().FirstOrDefault();
                if (button != null)
                {
                    button.Click += AskQuestion!;
                }
            }
        }
    }

    private void AskQuestion(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int questionId)
        {
            Console.WriteLine("Asked! Question " + questionId);
        }
    }

    private void App_OnClose()
    {
        _renderTimer.Close();
    }
}
