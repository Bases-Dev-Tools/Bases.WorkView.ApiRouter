using System.Text.Json.Serialization;

/// <summary>
/// AttributeValuesModelV2
/// </summary>
public class WorkViewAttributeValues : Dictionary<string, dynamic> 
{
    
}

/// <summary>
/// AttributeModel
/// </summary>
public class WorkViewAttribute : IWorkViewResponse
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
    public Type? AttributeType { get => this.MapAttributeType(); }

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
public class WorkViewAttributes : IWorkViewResponse
{
    [JsonPropertyName("items")]
    public List<WorkViewAttribute> Items { get; set; } = [];
    public WorkViewAttribute GetbyId(string id) => Items.FirstOrDefault(a => a.Id.Equals(id));
    public WorkViewAttribute GetbyName(string name) => Items.FirstOrDefault(a => a.SystemName.Equals(name, StringComparison.OrdinalIgnoreCase)
    || a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

public class WorkViewDataSet : IWorkViewResponse
{
    [JsonPropertyName("dataSetId")]
    public string? DataSetId { get; set; }

    [JsonPropertyName("dataSetType")]
    public DataSetType DataSetType { get; set; }

    [JsonPropertyName("parentDataSetId")]
    public string? ParentDataSetId { get; set; }

    [JsonPropertyName("filterId")]
    public string? FilterId { get; set; }
}

public enum DataSetType
{
    DataSet,
    FilterBackEnd
}

/// <summary>
/// Represents DataSet values from Hyland's Api with pagination information
/// </summary>
public class WorkViewDataSetValues : IWorkViewResponse
{
    [JsonPropertyName("hasExceededMaxResults")]
    public bool HasExceededMaxResults { get; set; }

    [JsonPropertyName("maximumResultsAllowed")]
    public int MaximumResultsAllowed { get; set; }

    [JsonPropertyName("items")]
    public List<WorkViewDataSetValue> Items { get; set; } = [];
}

/// <summary>
/// DataSetValueModel
/// </summary>
public class WorkViewDataSetValue
{
    [JsonPropertyName("displayValue")]
    public string? DisplayValue { get; set; }

    [JsonPropertyName("backingValue")]
    public string? BackingValue { get; set; }
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