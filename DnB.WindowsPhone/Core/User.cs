using Newtonsoft.Json;

namespace DnB.WindowsPhone.Core
{
    [JsonObject]
    public class User
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string UserId { get; set; }

        [JsonProperty]
        public int Points { get; set; }

        public User()
        {
            
        }

        public User(string userId)
        {
            UserId = userId;
        }
    }
}
