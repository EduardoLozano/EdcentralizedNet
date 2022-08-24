using System.Collections.Generic;

namespace EdcentralizedNet.OpenseaModels
{
    public class OSEventList
    {
        public string next { get; set; }
        public string previous { get; set; }
        public List<OSEvent> asset_events { get; set; }
    }
}
