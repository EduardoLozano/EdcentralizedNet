namespace EdcentralizedNet.OpenseaModels
{
    public class OSEvent
    {
        public string event_type { get; set; }
        public string event_timestamp { get; set; }
        public string auction_type { get; set; }
        public string total_price { get; set; }
        public OSPaymentToken payment_token { get; set; }
        public OSTransaction transaction { get; set; }
        public string created_date { get; set; }
        public string quantity { get; set; }
        public string approved_account { get; set; }
        public string bid_amount { get; set; }
        public string collection_slug { get; set; }
        public string contract_address { get; set; }
        public string custom_event_name { get; set; }
        public string dev_fee_payment_event { get; set; }
        public int? dev_seller_fee_basis_points { get; set; }
        public string duration { get; set; }
        public string ending_price { get; set; }
        public OSAccount from_account { get; set; }
        public long id { get; set; }
        public bool? is_private { get; set; }
        public OSAccount owner_account { get; set; }
        public OSAccount seller { get; set; }
        public string starting_price { get; set; }
        public OSAccount to_account { get; set; }
        public OSAccount winner_account { get; set; }
        public string listing_time { get; set; }
    }
}
