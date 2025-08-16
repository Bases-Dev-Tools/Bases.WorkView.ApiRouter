using Refit;
using System.Text.Json.Serialization;

public class WorkViewApplication
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

public class WorkViewApplications
{
    [JsonPropertyName("items")]
    public List<WorkViewApplication> Applications { get; set; } = [];    
    public WorkViewApplication? GetbyId(string id) => Applications.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewApplication? GetbyName(string name) => Applications.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}
public class WorkViewClass
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
}

public class WorkViewClasses
{
    [JsonPropertyName("items")]
    public List<WorkViewClass> Classes { get; set; } = [];
    public WorkViewClass? GetbyId(string id) => Classes.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewClass? GetbyName(string name) => Classes.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

}

public class WorkViewAttribute
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("systemName")]
    public string? SystemName { get; set; }

    [JsonPropertyName("dataType")]
    public string? DataType { get; set; } 

    [JsonPropertyName("dataSetId")]
    public string? DataSetId { get; set; } 

    [JsonPropertyName("classId")]
    public string? ClassId { get; set; } 

    [JsonPropertyName("relatedClassId")]
    public string? RelatedClassId { get; set; }

    [JsonPropertyName("relatedClassName")]
    public string? RelatedClass { get; set; }

    [JsonPropertyName("isMutable")]
    public bool IsMutable { get; set; }
}

public class WorkViewAttributes
{
    [JsonPropertyName("items")]
    public List<WorkViewAttribute> Attributes { get; set; } = [];
    public WorkViewAttribute? GetbyId(string id) => Attributes.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewAttribute? GetbyName(string name) => Attributes.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

