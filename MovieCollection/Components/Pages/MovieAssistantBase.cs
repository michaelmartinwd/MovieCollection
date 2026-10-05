using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;

namespace MovieCollection.Components.Pages
{
    public class MovieAssistantBase : ComponentBase
    {
        [Inject]
        public IClaudeService MovieAssistantService { get; set; } = null!;

        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";

        public bool IsLoading { get; set; }

        protected async override void OnInitialized()
        {
            base.OnInitialized();
        }

        public async Task AskQuestion()
        {
            if (string.IsNullOrWhiteSpace(Question))
                return;

            try
            {
                IsLoading = true;
                Answer = "";

                Answer = await MovieAssistantService.AskMovieQuestionAsync(Question);
            }
            catch
            {
                Answer =
                    "Sorry, there was a problem answering your question.";
            }
            finally
            {
                IsLoading = false;
            }

        }
    }
}