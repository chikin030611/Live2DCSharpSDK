using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Collections.ObjectModel;
using System.Linq;

namespace Live2DCSharpSDK.Avalonia.Avatar.UI;

/// <summary>
/// A control for displaying and interacting with Live2D avatars.
/// </summary>
public partial class Live2dControl : UserControl
{
    private readonly Live2dRender _render;
    private readonly FpsTimer _renderTimer;

    /// <summary>
    /// Gets the collection of questions to be displayed.
    /// </summary>
    public ObservableCollection<QuestionItem> DisplayQuestions { get; } = new ObservableCollection<QuestionItem>();

    /// <summary>
    /// Represents a question item with an ID and a question text.
    /// </summary>
    public class QuestionItem
    {
        public int Id { get; set; }
        public string Question { get; set; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Live2dControl"/> class.
    /// </summary>
    public Live2dControl()
    {
        InitializeComponent();

        DataContext = this;

        // Create Live2D render and attach it to the decorator element in Live2dControl.axaml
        _render = new();
        Live2D.Child = _render;
        _renderTimer = new(_render);

        // Load questions from QnaAudioManager and add them to the display collection
        foreach (var item in QnaAudioManager.QnaList)
        {
            int id = item.Id;
            string question = item.Question;
            DisplayQuestions.Add(new QuestionItem { Id = id, Question = question });
        }

        App.OnClose += App_OnClose;
    }

    /// <summary>
    /// Handles the application close event to properly dispose of the render timer.
    /// </summary>
    private void App_OnClose()
    {
        _renderTimer.Close();
    }

    /// <summary>
    /// Handles the Loaded event of the QuestionsItemsControl to attach event handlers to buttons.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void QuestionsItemsControl_Loaded(object sender, RoutedEventArgs e)
    {
        // Attach event handler to buttons
        foreach (var item in QuestionsItemsControl.Items) // QuestionsItemControl is an element in Live2dControl.axaml
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

    /// <summary>
    /// Handles the Click event of the buttons to ask a question.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void AskQuestion(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int questionId)
        {
            _render.StartSpeaking(questionId);
        }
    }
}
