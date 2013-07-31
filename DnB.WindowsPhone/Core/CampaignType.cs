using Newtonsoft.Json;

namespace DnB.WindowsPhone.Core
{
    [JsonObject]
    public class CampaignType
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Cost { get; set; }
    }
}
