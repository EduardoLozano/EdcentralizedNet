using System;
using System.Globalization;

namespace EdcentralizedNet.Models
{
    public class EthTransaction
    {
        public string blockHash { get; set; }
        public long blockNumber { get; set; }
        public string from { get; set; }
        public long gas { get; set; }
        public long gasPrice { get; set; }
        public long maxFeePerGas { get; set; }
        public long maxPriorityFeePerGas { get; set; }
        public string hash { get; set; }
        public string input { get; set; }
        public long nonce { get; set; }
        public string to { get; set; }
        public long transactionIndex { get; set; }
        public long value { get; set; }
        public long type { get; set; }
        public long chainId { get; set; }
        public long v { get; set; }
        public string r { get; set; }
        public string s { get; set; }
    }
}
