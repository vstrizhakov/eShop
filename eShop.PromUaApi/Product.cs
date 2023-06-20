using Newtonsoft.Json;

namespace eShop.PromUaApi
{
    public class Product
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("external_id")]
        public string? ExternalId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_multilang")]
        public Multilang NameMultilang { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("keywords")]
        public string Keywords { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("description_multilang")]
        public Multilang DescriptionMultilang { get; set; }

        [JsonProperty("selling_type")]
        public SellingType SellingType { get; set; }

        [JsonProperty("presence")]
        public Presence Presence { get; set; }

        [JsonProperty("in_stock")]
        public bool InStock { get; set; }

        [JsonProperty("price")]
        public float? Price { get; set; }

        [JsonProperty("minimum_order_quantity")]
        public float? MinimumOrderQuantity { get; set; }

        [JsonProperty("discount")]
        public Discount? Discount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("prices")]
        public IEnumerable<ProductPrice> Prices { get; set; }

        [JsonProperty("main_image")]
        public string MainImage { get; set; }

        [JsonProperty("images")]
        public IEnumerable<ProductImage> Images { get; set; }

        [JsonProperty("status")]
        public ProductStatus Status { get; set; }

        [JsonProperty("quantity_in_stock")]
        public long? QuantityInStock { get; set; }

        [JsonProperty("measure_unit")]
        public string MeasureUnit { get; set; }

        [JsonProperty("is_variation")]
        public bool IsVariation { get; set; }

        [JsonProperty("variation_base_id")]
        public int? VariationBaseId { get; set; }

        [JsonProperty("variation_group_id")]
        public int? VariationGroupId { get; set; }
    }
}