using System;
using Newtonsoft.Json;

namespace DnB.WindowsPhone.Core
{
    [JsonObject]
    public class Campaign
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string User { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string CompanyId { get; set; }

        [JsonProperty]
        public string CompanyName { get; set; }

        [JsonProperty]
        public int Cost { get; set; }

        [JsonProperty]
        public int TargetGain { get; set; }

        [JsonProperty]
        public DateTime TargetDate { get; set; }

        [JsonProperty]
        public bool IsClosed { get; set; }

        [JsonProperty]
        public string Notes { get; set; }
    }
}
