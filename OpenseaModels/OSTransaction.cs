namespace EdcentralizedNet.OpenseaModels
{
    public class OSTransaction
    {
        public string block_hash { get; set; }
        public string block_number { get; set; }
        public OSAccount from_account { get; set; }
        public int id { get; set; }
        public string timestamp { get; set; }
        public OSAccount to_account { get; set; }
        public string transaction_hash { get; set; }
        public string transaction_index { get; set; }
    }
}
