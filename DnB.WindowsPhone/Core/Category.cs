using Newtonsoft.Json;

namespace DnB.WindowsPhone.Core
{
    [JsonObject]
    public class Category
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Code { get; set; }
    }
}
