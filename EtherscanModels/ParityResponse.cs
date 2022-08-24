namespace EdcentralizedNet.EtherscanModels
{
    public class ParityResponse<T>
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }
        public T result { get; set; }
    }
}
