using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class ClaudeRequest
    {
        public string Model { get; set; } = string.Empty;
        public int MaxTokens { get; set; }
        public string System { get; set; } = string.Empty;
        public List<ClaudeMessage> Messages { get; set; } = [];
    }

    public class ClaudeMessage
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
