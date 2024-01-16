using System.Text.Json.Serialization;
namespace ResourceLoader.Models;

public class Stats{
    [JsonPropertyName("requests_served_total")]
    public ulong Requests {get;set;}

    [JsonPropertyName("bytes_transferred_total")]
    public ulong Bytes {get;set;}
}