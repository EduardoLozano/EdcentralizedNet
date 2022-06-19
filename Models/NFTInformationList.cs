using System.Collections.Generic;

namespace EdcentralizedNet.Models
{
    public class NFTInformationList : List<NFTInformation>
    {
        public string NextPageCursor { get; set; }
        public string PrevPageCursor { get; set; }
    }
}
