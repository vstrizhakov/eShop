﻿using Newtonsoft.Json;

namespace eShopping.Identity.Models
{
    public class SignOutInfo
    {
        [JsonProperty("prompt")]
        public bool? Prompt { get; set; }

        [JsonProperty("postInfo", NullValueHandling = NullValueHandling.Ignore)]
        public PostSignOutInfo PostInfo { get; set; }
    }
}
