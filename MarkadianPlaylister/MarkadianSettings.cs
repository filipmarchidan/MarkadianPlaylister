using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarkadianPlaylister
{

    // the general parameters for the settings file.
    public class MarkadianSettings
    {
        [JsonPropertyName("bitRate")]
        required public String bitRateSelector { get; set; }

        [JsonPropertyName("filePath")]
        required public String filePath { get; set; }
        [JsonPropertyName("enableQueue")]
        required public bool enableQueue { get; set; }

        [JsonPropertyName("theme")]
        required public string theme { get; set; }


        [JsonPropertyName("searchCount")]
        required public string searchCount { get; set; }

        [JsonPropertyName("enableUpdates")]
        required public bool enableUpdates { get; set; }

        [JsonPropertyName("enableDragDrop")]
        required public bool enableDragDrop { get; set; }


        [JsonPropertyName("resourceDirectory")]
        required public String resourceDirectory { get; set; }


        [JsonPropertyName("fileType")]
        required public String fileType { get; set; }

        
        [JsonPropertyName("videoQuality")]
        required public String videoQuality { get; set; }
    }
}
