using System;
using System.Globalization;

namespace EdcentralizedNet.EtherscanModels
{
    public class EtherscanTransaction
    {
        public long maxFeePerGas { get; set; }
        public long maxPriorityFeePerGas { get; set; }
        public long gas { get; set; }
        public long gasPrice { get; set; }
        public long value { get; set; }
        public long type { get; set; }
        public long chainId { get; set; }
        public long v { get; set; }
        public string r { get; set; }
        public string s { get; set; }
    }
}
