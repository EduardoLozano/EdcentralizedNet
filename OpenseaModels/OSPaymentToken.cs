namespace EdcentralizedNet.OpenseaModels
{
    public class OSPaymentToken
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public string address { get; set; }
        public string image_url { get; set; }
        public string name { get; set; }
        public int decimals { get; set; }
        public string eth_price { get; set; }
        public string usd_price { get; set; }
    }
}
