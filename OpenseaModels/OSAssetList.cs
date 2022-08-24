using System.Collections.Generic;

namespace EdcentralizedNet.OpenseaModels
{
    public class OSAssetList
    {
        public string next { get; set; }
        public string previous { get; set; }
        public List<OSAsset> assets { get; set; }
    }
}
