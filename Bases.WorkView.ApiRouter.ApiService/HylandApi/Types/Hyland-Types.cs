using Refit;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the response to Hyland's Api GET Applications
/// </summary>
public class WorkViewApplications : IWorkViewResponce
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
public class WorkViewApplication : IWorkViewResponce
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
public class WorkViewClass : IWorkViewResponce
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
public class WorkViewClasses : IWorkViewResponce
{
    [JsonPropertyName("items")]
    public List<WorkViewClass> Items { get; set; } = [];
    public WorkViewClass GetbyId(string id) => Items.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewClass GetbyName(string name) => Items.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

}
/// <summary>
/// Represents an individual AttrbuteItem from Hyland's Api
/// </summary>
public class WorkViewAttribute : IWorkViewResponce
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("systemName")]
    public string? SystemName { get; set; }

    [JsonPropertyName("dataType")]
    public string? DataType { get; set; }

    [JsonIgnore]
    public Type? AttributeType => this.MapAttributeType();

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

    public dynamic? CreateInstance<T>(T type)
    {
        return Activator.CreateInstance(AttributeType, default(T));
    }

    public dynamic? TryCreateAttribute(object value)
    { 
        try
        {
            var convertedValue = Convert.ChangeType(value, AttributeType);
            return convertedValue;
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Represents the response to the Hyland Api GET Attributes.
/// </summary>
public class WorkViewAttributes : IWorkViewResponce
{
    [JsonPropertyName("items")]
    public List<WorkViewAttribute> Items { get; set; } = [];
    public WorkViewAttribute GetbyId(string id) => Items.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewAttribute GetbyName(string name) => Items.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

public class WorkViewDataSet : IWorkViewResponce
{
    [JsonPropertyName("dataSetId")]
    public string? DataSetId { get; set; }

    [JsonPropertyName("dataSetType")]
    public string? DataSetType { get; set; }

    [JsonPropertyName("parentDataSetId")]
    public string? ParentDataSetId { get; set; }

    [JsonPropertyName("filterId")]
    public string? FilterId { get; set; }
}

/// <summary>
/// Represents DataSet values from Hyland's Api with pagination information
/// </summary>
public class WorkViewDataSetValues : IWorkViewResponce
{
    [JsonPropertyName("hasExceededMaxResults")]
    public bool HasExceededMaxResults { get; set; }

    [JsonPropertyName("maximumResultsAllowed")]
    public int MaximumResultsAllowed { get; set; }

    [JsonPropertyName("items")]
    public List<WorkViewDataSetValue> Items { get; set; } = [];
}

/// <summary>
/// Represents a single DataSet value item from Hyland's Api
/// </summary>
public class WorkViewDataSetValue
{
    [JsonPropertyName("displayValue")]
    public string? DisplayValue { get; set; }

    [JsonPropertyName("backingValue")]
    public string? BackingValue { get; set; }
}

public class WorkViewObject
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("attributeValues")]
    public Dictionary<string,dynamic>? AttributeValues {  get; set; }
}

public enum AttributeTypeName
{
    Char,
    Text,
    FormattedText,
    LargeInt,
    Float,
    Decimal,
    Currency,
    EncryptedAlphanumeric,
    Date,
    DateTime,
    Boolean,
    Relation,
    Document
}
public interface IWorkViewResponce
{

}