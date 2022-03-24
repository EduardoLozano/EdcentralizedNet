using System.Collections.Generic;

namespace EdcentralizedNet.Models
{
    public class OSCollection
    {
        public List<OSAssetContract> primary_asset_contracts { get; set; }
        public OSStats stats { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string twitter_username { get; set; }
        public string instagram_username { get; set; }
    }
}
