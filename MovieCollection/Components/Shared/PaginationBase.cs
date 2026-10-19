using Microsoft.AspNetCore.Components;

namespace MovieCollection.Components.Shared
{
    public class PaginationBase : ComponentBase
    {
        [Parameter]
        public int CurrentPage { get; set; } = 1;
        [Parameter]
        public int TotalPages { get; set; }
        [Parameter]
        public string SearchTitle { get; set; } = string.Empty;

        public int StartPage => Math.Max(1, CurrentPage - 2);
        public int EndPage => Math.Min(TotalPages, CurrentPage + 2);

        [Parameter]
        public EventCallback<int> PageChanged { get; set; }

        public async Task LoadNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                await PageChanged.InvokeAsync(CurrentPage);
            }

        }

        public async Task LoadPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await PageChanged.InvokeAsync(CurrentPage);
            }
        }

        public async Task GoToPage(int page)
        {
            CurrentPage = page;
            await PageChanged.InvokeAsync(CurrentPage);
        }
    }

}