namespace EdcentralizedNet.Models
{
    public class OSAssetContract
    {
        public string address { get; set; }
        public string asset_contract_type { get; set; }
        public string created_date { get; set; }
        public string name { get; set; }
        public string nft_version { get; set; }
        public string opensea_version { get; set; }
        public object owner { get; set; }
        public string schema_name { get; set; }
        public string symbol { get; set; }
        public string total_supply { get; set; }
        public string description { get; set; }
        public string external_link { get; set; }
        public string image_url { get; set; }
        public bool default_to_fiat { get; set; }
        public int dev_buyer_fee_basis_points { get; set; }
        public int dev_seller_fee_basis_points { get; set; }
        public bool only_proxied_transfers { get; set; }
        public int opensea_buyer_fee_basis_points { get; set; }
        public int opensea_seller_fee_basis_points { get; set; }
        public int buyer_fee_basis_points { get; set; }
        public int seller_fee_basis_points { get; set; }
        public string payout_address { get; set; }
    }
}
