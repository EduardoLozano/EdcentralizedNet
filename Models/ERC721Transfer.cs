namespace EdcentralizedNet.Models
{
    public class ERC721Transfer
    {
        public long blockNumber { get; set; }
        public long timeStamp { get; set; }
        public string hash { get; set; }
        public string nonce { get; set; }
        public string blockHash { get; set; }
        public string from { get; set; }
        public string contractAddress { get; set; }
        public string to { get; set; }
        public string tokenID { get; set; }
        public string tokenName { get; set; }
        public string tokenSymbol { get; set; }
        public string tokenDecimal { get; set; }
        public int transactionIndex { get; set; }
        public long gas { get; set; }
        public long gasPrice { get; set; }
        public long gasUsed { get; set; }
        public long cumulativeGasUsed { get; set; }
        public string input { get; set; }
        public long confirmations { get; set; }
        public EthTransaction transaction { get; set; }

    }
}
