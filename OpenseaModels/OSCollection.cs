using System.Collections.Generic;

namespace EdcentralizedNet.OpenseaModels
{
    public class OSCollection
    {
        public string banner_image_url { get; set; }
        public string chat_url { get; set; }
        public string created_date { get; set; }
        public bool default_to_fiat { get; set; }
        public string description { get; set; }
        public int dev_buyer_fee_basis_points { get; set; }
        public int dev_seller_fee_basis_points { get; set; }
        public string discord_url { get; set; }
        public string external_url { get; set; }
        public bool featured { get; set; }
        public string featured_image_url { get; set; }
        public bool hidden { get; set; }
        public List<OSAssetContract> primary_asset_contracts { get; set; }
        public string safelist_request_status { get; set; }
        public string image_url { get; set; }
        public bool is_subject_to_whitelist { get; set; }
        public string large_image_url { get; set; }
        public string medium_username { get; set; }
        public OSStats stats { get; set; }
        public string name { get; set; }
        public bool only_proxied_transfers { get; set; }
        public int opensea_buyer_fee_basis_points { get; set; }
        public int opensea_seller_fee_basis_points { get; set; }
        public string payout_address { get; set; }
        public bool require_email { get; set; }
        public string short_description { get; set; }
        public string slug { get; set; }
        public string telegram_url { get; set; }
        public string twitter_username { get; set; }
        public string instagram_username { get; set; }
        public string wiki_url { get; set; }
        public bool is_nsfw { get; set; }
    }
}
