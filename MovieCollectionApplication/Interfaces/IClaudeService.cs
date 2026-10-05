using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionApplication.Interfaces
{
    public interface IClaudeService
    {
        Task<string> AskMovieQuestionAsync(string question);
    }
}