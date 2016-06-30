using System;
using Newtonsoft.Json;

namespace PlatformManagement.Hubs
{
    
    public class Message
    {
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("type")]
        public MessageType MessageType { get; set; }
    }
}