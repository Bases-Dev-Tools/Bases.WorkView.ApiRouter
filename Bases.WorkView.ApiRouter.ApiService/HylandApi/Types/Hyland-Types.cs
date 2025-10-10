using Refit;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the response to Hyland's Api GET Applications
/// </summary>
public class WorkViewApplications : IWorkViewResponse
{
    [JsonPropertyName("items")]
    public List<WorkViewApplication> Items { get; set; } = [];
    public WorkViewApplication GetbyId(string id) => Items.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewApplication GetbyName(string name) => Items.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

/// <summary>
/// Represents a single application item from Hyland's Api
/// </summary>
public class WorkViewApplication : IWorkViewResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; } 

    [JsonPropertyName("systemName")]
    public string? SystemName { get; set; } 

    [JsonPropertyName("defaultFilterId")]
    public string? DefaultFilterId { get; set; } 

    [JsonPropertyName("defaultCalendarId")]
    public string? DefaultCalendarId { get; set; } 

    [JsonPropertyName("fullTextCatalogId")]
    public string? FullTextCatalogId { get; set; }
    [JsonPropertyName("classes")]
    public List<WorkViewClass> Classes { get; set; } = [];
}

/// <summary>
/// Represents single class item from Hyland's Api
/// </summary>
public class WorkViewClass : IWorkViewResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; } 

    [JsonPropertyName("systemName")]
    public string? SystemName { get; set; }

    [JsonPropertyName("rootClassId")]
    public string? RootClassId { get; set; }

    [JsonPropertyName("attributes")]
    public List<WorkViewAttribute> Attributes { get; set; } = [];
    [JsonIgnore]
    public Type ClassType { get; set; }
}

/// <summary>
/// Represents the response to Hyland's Api GET classes
/// </summary>
public class WorkViewClasses : IWorkViewResponse
{
    [JsonPropertyName("items")]
    public List<WorkViewClass> Items { get; set; } = [];
    public WorkViewClass GetbyId(string id) => Items.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewClass GetbyName(string name) => Items.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

}

public interface IWorkViewResponse
{

}