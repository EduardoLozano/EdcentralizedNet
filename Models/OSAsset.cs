using System.Collections.Generic;

namespace EdcentralizedNet.Models
{
    public class OSAsset
    {
        public int id { get; set; }
        public int num_sales { get; set; }
        public string background_color { get; set; }
        public string image_url { get; set; }
        public string image_preview_url { get; set; }
        public string image_thumbnail_url { get; set; }
        public string image_original_url { get; set; }
        public string animation_url { get; set; }
        public string animation_original_url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string external_link { get; set; }
        public OSAssetContract asset_contract { get; set; }
        public string permalink { get; set; }
        public OSCollection collection { get; set; }
        public int? decimals { get; set; }
        public string token_metadata { get; set; }
        public bool is_nsfw { get; set; }
        public OSAccount owner { get; set; }
        public string sell_orders { get; set; }
        public string seaport_sell_orders { get; set; }
        public OSAccount creator { get; set; }
        public List<OSTrait> traits { get; set; }
        public OSEvent last_sale { get; set; }
        public string top_bid { get; set; }
        public string listing_date { get; set; }
        public bool is_presale { get; set; }
        public string transfer_fee_payment_token { get; set; }
        public string transfer_fee { get; set; }
        public string token_id { get; set; }
    }
}
