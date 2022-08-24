using System.Collections.Generic;

namespace EdcentralizedNet.EtherscanModels
{
    public class EtherscanResponse<T>
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<T> result { get; set; }
    }
}
