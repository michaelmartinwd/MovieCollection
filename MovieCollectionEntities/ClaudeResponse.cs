using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class ClaudeResponse
    {
        public List<ClaudeContent> Content { get; set; } = [];
    }

    public class ClaudeContent
    {
        public string Type { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
