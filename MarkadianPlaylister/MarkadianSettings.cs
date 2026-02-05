using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarkadianPlaylister
{
    public class MarkadianSettings
    {
        [JsonPropertyName("bitRate")]
        public String bitRateSelector { get; set; }

        [JsonPropertyName("filePath")]
        public String filePath { get; set; }
        [JsonPropertyName("enableQueue")]
        public bool enableQueue { get; set; }

        [JsonPropertyName("theme")]
        public string theme { get; set; }


        [JsonPropertyName("searchCount")]
        public string searchCount { get; set; }

        [JsonPropertyName("enableUpdates")]
        public bool enableUpdates { get; set; }

        [JsonPropertyName("enableDragDrop")]
        public bool enableDragDrop { get; set; }


        [JsonPropertyName("resourceDirectory")]
        public String resourceDirectory { get; set; }
    }
}
