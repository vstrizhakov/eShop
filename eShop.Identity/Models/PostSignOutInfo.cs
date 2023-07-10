using Newtonsoft.Json;

namespace eShop.Identity.Models
{
    public class PostSignOutInfo
    {
        [JsonProperty("iframeUrl")]
        public string IFrameUrl { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }
    }
}
