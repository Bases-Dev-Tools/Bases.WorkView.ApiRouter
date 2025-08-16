using Refit;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(PostWorkViewObjectRequest), TypeInfoPropertyName = "", GenerationMode = JsonSourceGenerationMode.Default)]
public class PostWorkViewObjectRequest
{
    [JsonInclude]
    [JsonPropertyName("attributes")]
    public Dictionary<string, dynamic> Attributes { get; set; } = [];
    [JsonPropertyName("activateObject")]
    [JsonInclude]
    public bool ActivateObject { get; set; } = true;
}

public class UpdateWorkViewObjectRequest
{
    [JsonInclude]
    [JsonPropertyName("attributes")]
    public Dictionary<string, dynamic> Attributes { get; set; } = [];
}

public class GetWorkViewObjectRequest
{
    [JsonPropertyName("attributes")]
    public Dictionary<string, dynamic> Attributes { get; set; } = [];

    [Query("=","hydrate", "bool")]
    [JsonIgnore]
    public bool Hydrate { get; set; }

    [Query(CollectionFormat.Multi)]
    [JsonIgnore]
    public List<string>? QueryParams { get; set; }
}

public class DeleteWorkViewObjectRequest
{
   
}
